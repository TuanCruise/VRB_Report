using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using FIS.Base;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using Aspose.Cells;
using System.Xml.Serialization;
using System.IO;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace FIS.AppClient.Controls
{
    public partial class ucSearchExport : ucModule,
        ICommonFieldSupportedModule
    {
        public BaseEdit cboExportType { get; set; }
        public GridControl PrintGrid { get; set; }
        //TuDQ them
        public List<GridBand> Bands { get; set; }
        public List<string[]> listLayout { get; set; }
        public int levelMaxMergeExport = 1;
        public int modExport = 0;
        public DataTable columnRemove { get; set; }
        //End
        internal DataTable ResultTable { get; set; }
        internal string LastSearchResultKey { get; set; }
        public string FileName { get; set; }
        internal DateTime LastSearchTime { get; set; }
        public bool ExportSuccess { get; set; }

        public ucSearchExport()
        {
            InitializeComponent();
        }

        private void ExportDataTable(DataTableExporter exporter, Workbook book, WorkerThread thread)
        {
            //TudqThem
            if (modExport == 1 & columnRemove != null)
            {
                for (var j = 0; j < columnRemove.Rows.Count; j++)
                {
                    for (var i = 0; i < ResultTable.Columns.Count; i++)
                    {
                        if (ResultTable.Columns[i].ColumnName == columnRemove.Rows[j]["Value"].ToString())
                        {
                            ResultTable.Columns.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            //End
            var sheet = book.Worksheets[book.Worksheets.Add()];
            sheet.Name = string.Format(Language.SheetName, book.Worksheets.Count);

            thread.JobName = string.Format(Language.SaveToSheetStatus, book.Worksheets.Count);
            exporter.ExportDataTable(sheet, ResultTable);

            thread.JobName = string.Format(Language.ApplyFormatToSheetStatus, book.Worksheets.Count);
            sheet.AutoFitColumns();
        }

        private void ClearExportedRows()
        {
            while(ResultTable.Rows.Count > 0)
            {
                ResultTable.Rows.RemoveAt(0);                
            }
        }

        protected override void BuildButtons()
        {
            base.BuildButtons();
#if DEBUG
            SetupContextMenu(mainLayout);
            SetupLanguageTool();
            SetupSaveLayout(mainLayout);
#endif
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            lbTitle.Text = Language.Title;
        }

        protected override void LoadCommandFields()
        {
            base.LoadCommandFields();
            CommonFields.Clear();
            CommonFields.AddRange(
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleType,
                    CODES.DEFMODFLD.FLDGROUP.SEARCH_EXPORT
                ));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Execute();
        }

        public override void Execute()
        {
            base.Execute();

            if(ValidateModule())
            {
                string exportFileName;                
                exportFileName = (string)this["E02"];
                
                                
                var gridView = PrintGrid.DefaultView as GridView;
                if (gridView != null)
                {
                    lnkFile.Visible = false;

                    foreach (var control in CommonControlByID.Values)
                    {
                        control.Enabled = false;
                    }

                    var columns = new List<string>();
                    var fields = new List<ModuleFieldInfo>();
                    var headers = new List<string>();
                    foreach (GridColumn column in gridView.Columns)
                    {
                         var flagAddColumn = true;
                         if (column.Visible && column.VisibleIndex >= 0)
                         {
                             //TUDQ them
                             if (modExport == 1 & columnRemove != null)
                             {
                                 for (var j = 0; j < columnRemove.Rows.Count; j++)
                                 {
                                     if (columnRemove.Rows[j]["Value"].ToString() == column.FieldName)
                                     {
                                         flagAddColumn = false;
                                     }
                                 }
                             }
                             if (flagAddColumn)
                             {
                                 var field = (ModuleFieldInfo)column.Tag;
                                 if (field != null)
                                 {
                                     fields.Add(field);
                                     columns.Add(column.FieldName);
                                     headers.Add(column.ToolTip);
                                 }
                             }
                         }
                        //if (column.Visible && column.VisibleIndex >= 0)
                        //{
                        //    var field = (ModuleFieldInfo)column.Tag;
                        //    if (field != null)
                        //    {
                        //        fields.Add(field);
                        //        columns.Add(column.FieldName);
                        //        headers.Add(column.Caption);
                        //    }
                        //}
                    }

                    if ((string)this["E01"] == CODES.EXPORT.EXPORTTYPE.XML || (string)this["E01"] == CODES.EXPORT.EXPORTTYPE.TXT)
                    {
                          CurrentThread = new WorkerThread(
                            delegate(WorkerThread thread)
                            {
                                LockUserAction();
                                ExportSuccess = false;
                                    try
                                    {
                                        using (var ctrlSA = new SAController())
                                        {
                                            DataContainer container;
                                            DataSet ds = new DataSet("main");
                                            DataTable tempTable;
                                            ctrlSA.FetchAllSearchResult(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, LastSearchResultKey, LastSearchTime, 0);

                                            tempTable = container.GetTable(
                                                FieldUtils.GetModuleFields(
                                                    ModuleInfo.ModuleID,
                                                    CODES.DEFMODFLD.FLDGROUP.SEARCH_COLUMN
                                                ));
                                            tempTable.TableName = "DATA_RECORD";                                                  
                                            ds.Tables.Add(tempTable);
                                            //ds.Tables.Add(table);
                                            if ((string)this["E01"] == CODES.EXPORT.EXPORTTYPE.XML)
                                            {
                                                ds.WriteXml((string)this["E02"]);
                                            }
                                            else
                                            {
                                                WriteFileText(ds.Tables[0], (string)this["E02"]);
                                            }
                                            
                                            
                                            CurrentThread.JobName = string.Format(Language.SaveToFileStatus, exportFileName);
                                            thread.ExecuteUpdateGUI(true);                                            
                                        }
                                        thread.JobName = Language.CompletedStatus;
                                        thread.PercentComplete = 100;
                                        ExportSuccess = true;    
                                    }
                                    catch (Exception ex)
                                    {                            
                                        ShowError(ex);
                                    }
                                    finally
                                    {
                                        UnLockUserAction();
                                    }
                                    thread.ExecuteUpdateGUI(true);
                            }, this);

                          btnExport.Enabled = false;
                          CurrentThread.ProcessComplete += thread_ProcessComplete;
                          CurrentThread.DoUpdateGUI += thread_DoUpdateGUI;
                          CurrentThread.Start();          
                    }
                    else
                    {
                        var exporter = new DataTableExporter(columns.ToArray(), headers.ToArray(), ModuleInfo, fields.ToArray());
                        CurrentThread = new WorkerThread(
                            delegate(WorkerThread thread)
                            {
                                LockUserAction();
                                ExportSuccess = false;

                                FileFormatType exportType;
                                int maxRowCount;

                                switch ((string)this["E01"])
                                {
                                    case CODES.EXPORT.EXPORTTYPE.EXCEL_XLSX:
                                        exportType = FileFormatType.Excel2007Xlsx;
                                        maxRowCount = CONSTANTS.MAX_ROWS_IN_EXPORT_XLSX;
                                        break;
                                    default:
                                        exportType = FileFormatType.Excel2003;
                                        maxRowCount = CONSTANTS.MAX_ROWS_IN_EXPORT_XLS;
                                        break;
                                }

                                try
                                {
                                    using (var ctrlSA = new SAController())
                                    {
                                        DataContainer container;
                                        DataTable tempTable;
                                        var book = exporter.CreateWorkBook();

                                        var fromRow = 0;
                                        var rowCount = 0;

                                        ResultTable = null;
                                        do
                                        {
                                            ctrlSA.FetchAllSearchResult(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, LastSearchResultKey, LastSearchTime, fromRow);
                                            tempTable = container.GetTable(
                                                FieldUtils.GetModuleFields(
                                                    ModuleInfo.ModuleID,
                                                    CODES.DEFMODFLD.FLDGROUP.SEARCH_COLUMN
                                                ));

                                            if (tempTable.Rows.Count > 0)
                                            {
                                                fromRow = fromRow + tempTable.Rows.Count;
                                                rowCount = rowCount + tempTable.Rows.Count;

                                                if (ResultTable == null)
                                                {
                                                    ResultTable = tempTable;
                                                }
                                                else
                                                {
                                                    foreach (DataRow row in tempTable.Rows)
                                                    {
                                                        ResultTable.ImportRow(row);
                                                    }

                                                    // TODO: CHANGE NAME OF THREAD
                                                    thread.JobName = string.Format("{0} rows buffered", rowCount);
                                                    thread.ExecuteUpdateGUI();
                                                }
                                            }

                                            if (ResultTable != null && ResultTable.Rows.Count > maxRowCount)
                                            {
                                                ExportDataTable(exporter, book, thread);
                                                ClearExportedRows();
                                            }
                                        }
                                        while (tempTable.Rows.Count > 0);

                                        if (ResultTable != null && ResultTable.Rows.Count > 0)
                                        {
                                            ExportDataTable(exporter, book, thread);
                                        }
                                        else
                                        {
                                            book.Worksheets.Add("No result");
                                        }

                                        CurrentThread.JobName = string.Format(Language.SaveToFileStatus, exportFileName);
                                        thread.ExecuteUpdateGUI(true);
                                        book.Save(exportFileName, exportType);
                                    }

                                    thread.JobName = Language.CompletedStatus;
                                    thread.PercentComplete = 100;
                                    ExportSuccess = true;
                                }
                                catch (Exception ex)
                                {
                                    thread.PercentComplete = 100;
                                    thread.JobName = Language.ErrorStatus;
                                    ShowError(ex);
                                }
                                finally
                                {
                                    UnLockUserAction();
                                }

                                thread.ExecuteUpdateGUI(true);
                            }, this);
                                       
                        btnExport.Enabled = false;
                        CurrentThread.ProcessComplete += thread_ProcessComplete;
                        CurrentThread.DoUpdateGUI += thread_DoUpdateGUI;
                        CurrentThread.Start();
                    }
                }
            }
        }

        static void WriteFileText(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;
                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;
                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2));
                }
                sw.WriteLine();
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2));
                        }
                        else
                        {
                            sw.Write(new string(' ', maxLengths[i] + 2));
                        }
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }
        public override void LockUserAction()
        {
            base.LockUserAction();
            if (!InvokeRequired)
            {
                ShowWaitingBox();
                Enabled = false;
            }
        }

        public override void UnLockUserAction()
        {
            base.UnLockUserAction();

            if (!InvokeRequired)
            {
                HideWaitingBox();
                Enabled = true;
            }
        }

        void thread_DoUpdateGUI(object sender, EventArgs e)
        {
            lbStatus.Text = ((WorkerThread) sender).JobName;
        }

        void thread_ProcessComplete(object sender, EventArgs e)
        {
            if(ExportSuccess)
            {
                lnkFile.Text = (string)this["E02"];
                lnkFile.Visible = true;      
                // Update Feedback
                if (ModuleInfo.ExecuteMode == CODES.DEFMOD.EXECMODE.FEEDBACK)
                {
                    using (SAController ctrlSA = new SAController())
                    {
                        List<string> values = new List<string>();
                        values.Add(ModuleInfo.ModuleID);
                        ctrlSA.ExecuteStoreProcedure("sp_feedback", values);                       
                    }   
                }
            }
            foreach (var control in CommonControlByID.Values)
            {
                control.Enabled = true;
            }
            btnExport.Enabled = true;
        }

        public bool ValidateRequire
        {
            get { return true; }
        }

        public LayoutControl CommonLayout
        {
            get { return mainLayout; }
        }

        public string CommonLayoutStoredData
        {
            get { return Language.ExportLayout; }
        }

        //TuDq them
        private string GetParentListExportMerge(string parent)
        {
            foreach (var item in listLayout)
            {
                if (item[0] == parent)
                {
                    parent = GetParentListExportMerge(item[1]);
                }
            }
            return parent;
        }

        private void GetAllChildExportMerge(string parent, DataTable dt1, GridView gridview, int level)
        {
            foreach (var item in listLayout)
            {
                string childValue = "";
                string childText = "";
                if (item[1] == parent)
                {
                    childValue = item[0];
                    foreach (var value in Bands)
                    {
                        if (childValue == value.Name)
                        {
                            childText = value.Caption;
                            break;
                        }
                    }
                    foreach (GridColumn column in gridview.Columns)
                    {
                        if (column.FieldName == childValue.Replace("band", ""))
                        {
                            childValue = "";
                        }
                    }
                    if (childValue != "")
                    {
                        dt1.Rows.Add(-1, childValue, level, GetMergeColumnExportMerge(childValue, gridview), childText);
                        GetAllChildExportMerge(childValue, dt1, gridview, level + 1);
                        if (level + 1 > levelMaxMergeExport)
                        {
                            levelMaxMergeExport = level + 1;
                        }
                    }
                }
            }
        }

        private int GetMergeColumnExportMerge(string value, GridView gridview)
        {
            var count = 0;
            foreach (var item in listLayout)
            {
                if (item[1] == value)
                {
                    foreach (GridColumn column in gridview.Columns)
                    {
                        if (item[0].Replace("band", "") == column.FieldName)
                            count++;
                    }
                    count = count + GetMergeColumnExportMerge(item[0], gridview);
                }
            }
            return count;
        }

        //End
    }
}

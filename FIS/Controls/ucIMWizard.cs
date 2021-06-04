using Aspose.Cells;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.AppClient.Threads;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FIS.AppClient.Controls
{
    public delegate void workerFunctionDelegate(string strStatusText);
    public delegate void poplateLableDelegate(string text);

    public partial class ucIMWizard : ucModule,
        IParameterFieldSupportedModule,
        ICommonFieldSupportedModule
    {
        #region Properties & Members
        private GridCheckMarksUtils m_CheckMarksUtils;
        private DataTable m_ExcelBufferTable;
        private ImageComboBoxItem[] m_ImportComboBoxItems;
        private string verifydata = string.Empty;
        private int _iReportID;
        private string _ReportDetailID;
        private int _RPTLOGSID;

        protected List<ModuleFieldInfo> ImportFields { get; set; }
        protected Dictionary<string, ImageComboBoxEdit> ImportControlByID { get; set; }

        public object this[string fieldID, DataRow row]
        {
            get
            {
                var field = FieldUtils.GetModuleFieldByID(
                    ModuleInfo.ModuleID, fieldID);

                var columnName = int.Parse(GetColumnName(field.FieldID));
                if (row[columnName] is string)
                {
                    return ((string)row[columnName]).Trim();
                }

                return row[columnName].DecodeAny(field);
            }
        }

        public ImportModuleInfo ImportInfo
        {
            get
            {
                return (ImportModuleInfo)ModuleInfo;
            }
        }

        public string ImportRowRange
        {
            get
            {
                if (this["ROW"] != null)
                    return this["ROW"].ToString();
                return string.Empty;
            }
            set
            {
                this["ROW"] = value;
            }
        }

        #endregion

        #region Override methods
        protected override void BuildButtons()
        {
#if DEBUG
            SetupContextMenu(mainLayout);
            SetupModuleEdit();
            SetupGenenerateScript();
            SetupSeparator();
            SetupParameterFields();
            SetupCommonFields();
            SetupSeparator();
            SetupFieldMaker();
            SetupFieldsSuggestion();
            SetupSeparator();
            SetupLanguageTool();
            SetupSaveLayout(mainLayout);
            SetupSaveAllLayout(mainLayout);
#endif
        }

        protected override void BuildFields()
        {
            base.BuildFields();
            BuildImportFields();
        }

        protected override void SetDefaultValues(List<ModuleFieldInfo> fields)
        {
            base.SetDefaultValues(fields);

            foreach (var field in fields)
            {
                if (field.DefaultValue != null)
                {
                    switch (field.FieldGroup)
                    {
                        case CODES.DEFMODFLD.FLDGROUP.IMPORT_COLUMN:
                            ImportControlByID[field.FieldID].EditValue = field.DefaultValue.Decode(field);
                            break;
                        case CODES.DEFMODFLD.FLDGROUP.COMMON:
                            CommonControlByID[field.FieldID].EditValue = field.DefaultValue.Decode(field);
                            break;
                    }
                }
            }
        }
        private void LoadData(string strProcedureName)
        {
            try
            {
                using (SAController ctrlSA = new SAController())
                {
                    DataContainer container;
                    List<string> values = new List<string>();
                    values.Add(Program.rptlogID);
                    ctrlSA.ExecuteProcedureFillDataset(out container, strProcedureName, values);
                    AssignFieldValuesFromResult(container);
                }
            }
            catch (FaultException ex)
            {
                ex.ToMessage();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading data", ex);
            }
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            try
            {
                StopCallback = true;
                if (ImportInfo.SelectStore != null && Program.rptlogID != null)
                    LoadData(ImportInfo.SelectStore);
                else
                    SetDefaultValues();
            }
            catch
            {
                CloseModule();
                throw;
            }
            finally
            {
                StopCallback = false;
            }
        }
        #endregion

        #region Module Utils
        private void BuildImportFields()
        {
            ImportFields =
                FieldUtils.GetModuleFields(
                    ModuleInfo.ModuleID,
                    CODES.DEFMODFLD.FLDGROUP.IMPORT_COLUMN
                );

            ImportControlByID = new Dictionary<string, ImageComboBoxEdit>();

            foreach (var field in ImportFields)
            {
                var control = CreateImportControl(field);
                control.Name = field.FieldName;

                if (!ImportControlByID.ContainsKey(field.FieldID))
                {
                    ImportControlByID.Add(field.FieldID, control);
                }
                else
                {
                    throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_MODULE_FIELD_NOT_FOUND_OR_DUPLICATE,
                        "GetModuleFieldByID", field.ModuleID, field.FieldGroup, field.FieldID);
                }
                //edit by TrungTT - 31.10.2012
                //mainLayout.Controls.Add(control); 
                //var item = mainLayout.AddItem(field.FieldName, control);
                //item.Name = "item" + field.FieldName;
                //mainLayout.Controls.Add(control); 
            }
        }

        private void GetOracleParameterValues(List<OracleParam> @params)
        {
            var fields = GetModuleFields();
            foreach (var param in @params)
            {
                param.Value = null;
                foreach (var field in fields)
                {
                    if (!string.IsNullOrEmpty(field.ParameterName) && param.Name == field.ParameterName)
                    {
                        param.Value = this[field.FieldID].Encode(field);
                    }

                    switch (param.Name)
                    {
                        case CONSTANTS.ORACLE_REPORT_ID:
                            param.Value = _iReportID;
                            break;
                        case CONSTANTS.ORACLE_REPORT_DETAILID:
                            param.Value = _ReportDetailID;
                            break;
                        case CONSTANTS.ORACLE_REPORT_RPTLOGSID:
                            param.Value = _RPTLOGSID;
                            break;
                    }
                }
            }
        }

        private void GetOracleParameterValues(List<OracleParam> @params, DataRow row)
        {
            var fields = FieldUtils.GetModuleFields(ModuleInfo.ModuleID);
            foreach (var param in @params)
            {
                foreach (var field in fields)
                {
                    if (!string.IsNullOrEmpty(field.ParameterName) && param.Name == field.ParameterName)
                    {
                        switch (field.FieldGroup)
                        {
                            case CODES.DEFMODFLD.FLDGROUP.IMPORT_COLUMN:
                                param.Value = this[field.FieldID, row].Encode(field);
                                break;
                            case CODES.DEFMODFLD.FLDGROUP.COMMON:
                                param.Value = this[field.FieldID].Encode(field);
                                break;
                        }
                    }
                    switch (param.Name)
                    {
                        case CONSTANTS.ORACLE_REPORT_ID:
                            param.Value = _iReportID;
                            break;
                        case CONSTANTS.ORACLE_REPORT_DETAILID:
                            param.Value = _ReportDetailID;
                            break;
                        case CONSTANTS.ORACLE_REPORT_RPTLOGSID:
                            param.Value = _RPTLOGSID;
                            break;
                    }
                    //if (param.Name == CONSTANTS.ORACLE_REPORT_ID)
                    //{
                    //    param.Value = _iReportID;
                    //}
                    //else if (param.Name == CONSTANTS.ORACLE_REPORT_DETAILID)
                    //{
                    //    param.Value = _ReportDetailID;
                    //}
                }
            }
        }

        public new void GetOracleParameterValues(out List<string> oracleValues, string storeName, DataRow dataRow)
        {
            var oracleParams = ModuleUtils.GetOracleParams(storeName);
            GetOracleParameterValues(oracleParams, dataRow);
            oracleValues = oracleParams.ToListString();
        }

        //public virtual void GetOracleParameterValues(out List<string> oracleValues, string storeName)
        //{
        //    var oracleParams = ModuleUtils.GetOracleParams(storeName);
        //    GetOracleParameterValues(oracleParams);
        //    oracleValues = oracleParams.ToListString();
        //}

        private new void GetOracleParameterValues(out List<string> oracleValues, string storeName)
        {
            var oracleParams = ModuleUtils.GetOracleParams(storeName);
            GetOracleParameterValues(oracleParams);
            oracleValues = oracleParams.ToListString();
        }

        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);

            wzpWelcome.Text = Language.Title;
            wzpWelcome.IntroductionText = Language.Info;
            wzpReadFile.Text = Language.Title;
            wzpSetting.Text = Language.Title;

            //m_CheckMarksUtils = new GridCheckMarksUtils(gvMain, false);
            //m_CheckMarksUtils.CheckMarkColumn.VisibleIndex = 0;
            //m_CheckMarksUtils.SelectChanged += CheckMarksUtils_SelectChanged;            
        }
        #endregion

        #region Field Utils
        public string GetColumnName(string fieldID)
        {
            return ImportControlByID[fieldID].EditValue.ToString();
        }

        public string GetColumnLabel(string fieldID)
        {
            return mainLayout.GetItemByControl(ImportControlByID[fieldID]).Text;
        }

        private static ImageComboBoxEdit CreateImportControl(ModuleFieldInfo fieldInfo)
        {
            // Edit Create
            var cboImport = new ImageComboBoxEdit
            {
                Name = string.Format(CONSTANTS.COMBOX_NAME_FORMAT, fieldInfo.FieldName),
                Tag = fieldInfo,
                EnterMoveNextControl = true
            };

            // Setting Repository
            cboImport.Properties.LargeImages = ThemeUtils.Image16;
            cboImport.Properties.SmallImages = ThemeUtils.Image16;
            return cboImport;
        }

        #endregion

        #region Events

        private void wzMain_CancelClick(object sender, CancelEventArgs e)
        {
            Program.blEnableImport = false;
            CloseModule();
        }

        private void wzpWelcome_PageCommit(object sender, EventArgs e)
        {
            wzpReadFile.AllowNext = false;
            wzpSetting.AllowNext = false;
            Program.strExecMod = ModuleInfo.ModuleID;

            //gcMain.DataSource = null;
        }

        void Thread_DoUpdateGUI(object sender, EventArgs e)
        {
            //var workerThread = (WorkerThread)sender;
            // pgbImport.EditValue = workerThread.PercentComplete;
            //pgbReadFile.EditValue = workerThread.PercentComplete;
            //lblStatusText.Text = workerThread.StatusText;

            wzpReadFile.AllowCancel = false;
        }

        private void txtFileName_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = IMPORTMASTER.IMPORT_FILE_EXTENSIONS
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                txtFileNameImp.Text = openDialog.FileName;
                //CurrentThread = new ImportReadFileThread(this)
                //                    {
                //                        FileName = txtFileNameImp.Text,
                //                        JobName = "Đang đọc file..."
                //                    };

                //CurrentThread.DoUpdateGUI += Thread_DoUpdateGUI;
                //CurrentThread.ProcessComplete += ReadFile_ProcessComplete;
                //CurrentThread.Start();
            }
        }

        //void ReadFile_ProcessComplete(object sender, EventArgs e)
        //{
        //    DataSet ds;
        //    string rptID = ImportInfo.ModuleID;
        //    // Lay thong tin cac sheet cua file Excel
        //    string[] sheetname;
        //    sheetname = getExcelSheets(txtFileNameImp.Text);
        //    using (var ctrlSA = new SAController())
        //    {

        //        ctrlSA.GetModImport(out ds, rptID);
        //        // Lay thong tin toan bo cac modimport tuong ung voi file Excel
        //        if (ds.Tables[0] != null)
        //        {
        //            //for (var j=0;j<ds.Tables[0].Rows.Count -1 ; j++)
        //            for (var j = 0; j < 1; j++)
        //            {
        //                for (var i = 0; i < sheetname.Length - 1; i++)
        //                {
        //                    //int iStr = sheetname[i].IndexOf(ds.Tables[0].Rows[j]["MODEXEC"].ToString());

        //                    //if (iStr > 0)
        //                    //{
        //                    //    ds = getXLData(sheetname[i], txtFileName.Text, " ");
        //                    //    m_ExcelBufferTable = ds.Tables[0];

        //                    //}                            
        //                }
        //            }

        //        }                
        //        //SYSTEM_STORE_PROCEDURES.MODULE_TREE = dt.Rows[0][0].ToString();
        //    }


        //    //var thread = sender as ImportReadFileThread;

        //    //if (thread != null && thread.ExcelBufferTable != null)
        //    //{
        //    //    m_ExcelBufferTable = thread.ExcelBufferTable;
        //    //    m_ImportComboBoxItems = thread.ImportComboBoxItems;

        //    //    foreach (var pair in ImportControlByID)
        //    //    {
        //    //        pair.Value.Properties.Items.Clear();
        //    //        pair.Value.Properties.Items.AddRange(m_ImportComboBoxItems);
        //    //    }

        //    //    var savedCursor = Cursor;
        //    //    Cursor = Cursors.WaitCursor;

        //    //    gvMain.Columns.Clear();
        //    //    gvMain.Columns.Add(m_CheckMarksUtils.CheckMarkColumn);

        //    //    var index = 1;
        //    //    gvMain.Columns.AddRange((
        //    //        from DataColumn column in m_ExcelBufferTable.Columns
        //    //        select new GridColumn
        //    //                   {
        //    //            Name = column.ColumnName,
        //    //            FieldName = column.ColumnName,
        //    //            Visible = true,
        //    //            VisibleIndex = index++
        //    //        }).ToArray());

        //    //    gcMain.DataSource = m_ExcelBufferTable;
        //    //    gvMain.BestFitColumns();

        //    //    Cursor = savedCursor;

        //    //    foreach (var item in ImportControlByID)
        //    //    {
        //    //        item.Value.Properties.Items.BeginUpdate();
        //    //        item.Value.Properties.Items.Clear();
        //    //        item.Value.Properties.Items.AddRange(m_ImportComboBoxItems);
        //    //        item.Value.Properties.Items.EndUpdate();
        //    //    }

        //    //    SetDefaultValues();
        //    //    RefreshGrid();

        //    //    wzpReadFile.AllowNext = true;
        //    //}
        //    //else
        //    //{
        //    //    txtFileName.Text = "";
        //    //}            
        //    wzpReadFile.AllowCancel = true;
        //}

        public override void SetDefaultValues()
        {
            base.SetDefaultValues();
            foreach (var importField in ImportFields)
            {
                ImportControlByID[importField.FieldID].EditValue = importField.DefaultValue;
            }
        }

        public override void LockUserAction()
        {
            base.LockUserAction();

            if (!InvokeRequired)
            {
                if (CurrentThread is ImportReadFileThread)
                {
                    ShowWaitingBox(Language.ReadingStatus);
                }
                else if (CurrentThread is ImportExportErrorThread)
                {
                    ShowWaitingBox(Language.ExportingStatus);
                }
                else
                {
                    ShowWaitingBox();
                }

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

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateModule())
                {
                    string filenameorg = txtFileNameImp.Text;

                    if (string.IsNullOrEmpty(filenameorg))
                    {
                        ShowError(ErrorUtils.CreateError(175));
                        return;
                    }

                    workerFunctionDelegate w = workerFunction;
                    File.AppendAllText("LastErrors.log", string.Format("Bắt đầu import : {0} \r\n-------------\r\n", System.DateTime.Now.ToLongTimeString()));
                    //w.BeginInvoke("Bắt đầu import dữ liệu vào hệ thống ....", null, null);
                    // Readfile 
                    DataSet ds;
                    // Lay thong tin cac sheet cua file Excel
                    List<string> sheetname;
                    string filename = ConvertFileExcel(filenameorg);
                    sheetname = getExcelSheets(filename);

                    //CheckRptMasterDetails(rptID, sheetname);

                    string StatusText;
                    bool bErr = true;

                    var impchkInfo = SysvarUtils.GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_IMPCHECK);
                    if (impchkInfo == CONSTANTS.Yes)
                    {
                        ImportCheck(ModuleInfo.ModuleID);
                    }

                    using (var ctrlSA = new SAController())
                    {
                        ctrlSA.GetModImport(out ds, ModuleInfo.ModuleID);
                        btnImport.Enabled = false;
                        wzpSetting.AllowNext = false;

                        if (bErr == true)
                        {
                            BuildImportFields();
                            foreach (var importField in ImportFields)
                            {
                                ImportControlByID[importField.FieldID].EditValue = importField.DefaultValue;
                            }
                            DataSet dsData = ReadFileExcel(sheetname[0], filename);

                            for (int k = 0; k < dsData.Tables.Count; k++)
                            {
                                //add by TrungTT - 4.6.2014 - break when importing false
                                if (bErr == false)
                                    break;
                                //end trungtt

                                m_ExcelBufferTable = null;
                                m_ExcelBufferTable = dsData.Tables[k];

                                var importRows = new List<DataRow>();
                                ImportRange[] ranges;
                                //var ranges = CalcRanges("0-" + m_ExcelBufferTable.Rows.Count);
                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[0]["ENDROW"].ToString()))
                                {
                                    ranges = CalcRanges("1-" + m_ExcelBufferTable.Rows.Count);
                                }
                                else
                                {
                                    ranges = CalcRanges("1-" + int.Parse(ds.Tables[0].Rows[0]["ENDROW"].ToString()));
                                }

                                for (var iCount = Math.Max(0, int.Parse(ds.Tables[0].Rows[0]["STARTROW"].ToString()) - 1); iCount < m_ExcelBufferTable.Rows.Count; iCount++)
                                {
                                    importRows.AddRange(from range in ranges where range.HaveToImport(iCount) select m_ExcelBufferTable.Rows[iCount]);
                                }
                                int iCountRow = 1;
                                #region Thuc hien import du lieu vao DB
                                if (verifydata == CONSTANTS.Yes)
                                {
                                    _iReportID = getReportID();
                                }

                                List<string> valuesterm = new List<string>();
                                //ProccessFileName(out valuesterm, _filename);
                                //valuesterm.Add(ds.Tables[0].Rows[0]["RPTID"].ToString());
                                // Verify
                                //using (var ctrlComplete = new SAController())
                                //{
                                //    ctrlComplete.ExecuteStoreProcedure("SP_CHECK_IMP_REPORT", valuesterm);
                                //}

                                foreach (var importRow in importRows)
                                {
                                    try
                                    {
                                        using (var ctrlSAImport = new SAController())
                                        {
                                            List<string> values;
                                            GetOracleParameterValues(out values, ImportInfo.ImportStore, importRow);
                                            StatusText = "Import Sheet [" + sheetname[0] + "]:" + " dòng thứ " + iCountRow + "...";
                                            lblStatusText.ForeColor = Color.Blue;
                                            w.BeginInvoke(StatusText, null, null);
                                            ctrlSAImport.ExecuteImport(ImportInfo.ModuleID, ImportInfo.SubModule, values);
                                            iCountRow++;
                                        }
                                    }
                                    catch (Exception cex)
                                    {
                                        bErr = false;
                                        iCountRow++;
                                        var ex = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, cex.Message);
                                        StatusText = "Lỗi Sheet [" + sheetname[0] + "]:" + " dòng thứ " + iCountRow + " - " + string.Format("{0}\r\n", ex.ToMessage());
                                        lblStatusText.ForeColor = Color.Red;
                                        w.BeginInvoke(StatusText, null, null);
                                        bErr = false;
                                        btnImport.Enabled = true;
                                        break;
                                    }
                                    finally
                                    {

                                    }
                                }
                                // Set complete
                                if (bErr)
                                {
                                    using (var ctrlComplete = new SAController())
                                    {
                                        List<string> valuescomplete;
                                        GetOracleParameterValues(out valuescomplete, ds.Tables[0].Rows[0]["SETCOMPLETE"].ToString());
                                        ctrlComplete.ExecuteStoreProcedure(ds.Tables[0].Rows[0]["SETCOMPLETE"].ToString(), valuescomplete);
                                        StatusText = "Dữ liệu đã được Import thành công";
                                        lblStatusText.ForeColor = Color.Blue;
                                        w.BeginInvoke(StatusText, null, null);
                                        btnImport.Enabled = false;
                                        wzpSetting.AllowNext = true;
                                        wzpSetting.AllowCancel = false;
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    // Ket thuc import 
                    File.AppendAllText("LastErrors.log", string.Format("Kết thúc import : {0}\r\n-------------\r\n", System.DateTime.Now.ToLongTimeString()));
                    File.Delete(filename);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {

            }
        }
        private void RollBackData(string v_RPTID, string v_REPORTID)
        {
            try
            {
                using (SAController ctrlSA = new SAController())
                {
                    List<string> values = new List<string>();
                    values.Add(v_RPTID);
                    values.Add(v_REPORTID);
                    ctrlSA.ExecuteStoreProcedure("sp_rollback_data", values);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(IMPORTMASTER.IMP_MSG_CANNOT_ROLLBACK_DATA, ex);
            }
        }
        private int getReportID()
        {
            int result;
            var ctrlSA = new SAController();
            DataContainer container = null;
            List<string> lstParam = new List<string>();
            ctrlSA.ExecuteProcedureFillDataset(out container, "sp_get_reportid", lstParam);
            result = Convert.ToInt32(container.DataTable.Rows[0][0].ToString());
            return result;
        }
        private int getRptlogsID()
        {
            int result;
            var ctrlSA = new SAController();
            DataContainer container = null;
            List<string> lstParam = new List<string>();
            ctrlSA.ExecuteProcedureFillDataset(out container, "sp_get_rptlogsid", lstParam);
            result = Convert.ToInt32(container.DataTable.Rows[0][0].ToString());
            return result;
        }
        //private Boolean SendFileToServer(string filename)
        //{
        //try
        //{
        //    var ctrlSA = new SAController();
        //    DataContainer container = null;
        //    DataContainer conSinger = null;
        //    List<string> lstParam = new List<string>();
        //    ctrlSA.ExecuteProcedureFillDataset(out container, "SP_CHECKSIGNER", lstParam);
        //    ctrlSA.ExecuteProcedureFillDataset(out conSinger, "SP_GETSIGNER", lstParam);

        //    // Xoa het file trong Deffile trong session 
        //    ctrlSA.ExecuteStoreProcedure("sp_deffile_del_bysession", lstParam);

        //    string IsCert = CONSTANTS.No;

        //    // Tao file Zip
        //    var outPathname = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + RandomString(10, false) + ".zip"; ;
        //    FileStream fsOut = File.Create(outPathname);
        //    ZipOutputStream zipStream = new ZipOutputStream(fsOut);
        //    zipStream.SetLevel(9); //0-9, 9 being the highest level of compression                    

        //    string _filenamesigned;
        //    string _filetemplate = string.Empty;                                    
        //    // Ky file truoc khi nen                
        //    int result;
        //    int errcode=10;
        //    //if (IsCert == CONSTANTS.Yes)
        //    bool chkCA = false;
        //    if (container.DataSet.Tables[0].Rows.Count > 0)
        //    {
        //        IsCert = container.DataSet.Tables[0].Rows[0][0].ToString();
        //        if (IsCert == CONSTANTS.Yes)
        //        {
        //            try
        //            {
        //                string serial = VNPTLIB.CertificateInfos.getCertSerial();
        //                for (int i = 0; i < conSinger.DataSet.Tables[0].Rows.Count; i++)
        //                {
        //                    if (conSinger.DataSet.Tables[0].Rows[i]["serial"].ToString() == serial)
        //                    {
        //                        chkCA = true;
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                errcode = 16;
        //            }

        //        }
        //    }
        //    _filenamesigned = filename;
        //    if (IsCert == CONSTANTS.Yes)
        //    {
        //        if (chkCA)
        //        {
        //            // Lay gio tu timestamp server
        //            string datetime = VNPTLIB.TSA.checkTSA();
        //            if (datetime == null)
        //            {
        //                datetime = DateTime.Now.ToString("dd/MM/yyyy");
        //            }
        //            if (Path.GetExtension(_filenamesigned).ToUpper() == ".PDF")
        //            {
        //                result = VNPTLIB.SignPDF.SignDetached(filename, datetime);
        //                if (result == 0)
        //                {
        //                    _filenamesigned = VNPTLIB.SignPDF.fileSigned;
        //                }
        //                else
        //                {
        //                    ShowMessVNPTCA(result, VNPTLIB.SignPDF.errorPDF);
        //                    Program.FileName = null;
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                result = VNPTLIB.SignOffice.Sign2k7XLS(filename, datetime);
        //                if (result == 0)
        //                {
        //                    _filenamesigned = filename;
        //                }
        //                else
        //                {
        //                    ShowMessVNPTCA(result, VNPTLIB.SignOffice.error);
        //                    Program.FileName = null;
        //                    return false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            RollBackData(Program.rptid, _RPTLOGSID.ToString());                       
        //            var cex = ErrorUtils.CreateError(errcode);
        //            ShowError(cex);
        //            Program.FileName = null;
        //            return false;
        //        }
        //    }                                            

        //    FileInfo fi = new FileInfo(_filenamesigned);                    
        //    string entryName = System.IO.Path.GetFileName(_filenamesigned);
        //    entryName = ZipEntry.CleanName(entryName); 
        //    ZipEntry newEntry = new ZipEntry(entryName);
        //    newEntry.DateTime = fi.LastWriteTime; 

        //    newEntry.Size = fi.Length;
        //    zipStream.PutNextEntry(newEntry);

        //    // Zip the file in buffered chunks                                
        //    byte[] buffer = new byte[4096];
        //    using (FileStream streamReader = File.OpenRead(_filenamesigned))
        //    {
        //        StreamUtils.Copy(streamReader, zipStream, buffer);
        //    }
        //    zipStream.CloseEntry();                                                                                                            

        //    zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
        //    zipStream.Close();

        //    // Send file to server                                     
        //    var _streamAttr = File.OpenRead(outPathname);                        
        //    var fileUpload = new UploadFileStream(_streamAttr);

        //    FileUpload upload = new FileUpload();           

        //    List<string> value = new List<string>();
        //    GetOracleParameterValues(out value, "sp_reportfilelogs_ins");
        //    // Đoạn này thằng hâm nào thay đổi thứ tự thì vẹo con mẹ luôn                
        //    upload.SecID = Convert.ToInt32(value[0].ToString());
        //    upload.RptID = value[1].ToString();
        //    upload.Term = value[2].ToString();
        //    try
        //    {
        //        upload.TermNo = value[3].ToString();
        //    }
        //    catch
        //    {
        //        upload.TermNo = null;
        //    }
        //    upload.RYear = Convert.ToInt32(value[4].ToString());
        //    // Ket thuc
        //    upload.FileName = System.IO.Path.GetFileName(outPathname);
        //    upload.UploadStream = fileUpload;
        //    ctrlSA.SaveFile(upload);
        //    _streamAttr.Dispose();                


        //    File.Delete(outPathname);
        //    return true;                                 
        //}
        //catch (Exception ex)
        //{
        //    ShowError(ex);
        //    RollBackData(Program.rptid, _iReportID.ToString());
        //    return false;
        //}
        //}


        private void ImportCheck(string v_modid)
        {
            try
            {
                using (SAController ctrlSA = new SAController())
                {
                    //1. Check mod import
                    List<string> modid = new List<string>();
                    modid.Add(v_modid);

                    DataContainer dc;
                    ctrlSA.ExecuteProcedureFillDataset(out dc, "sp_modimport_by_modid", modid);
                    DataTable dt = dc.DataTable;
                    verifydata = dt.Rows[0]["VERIFYDATA"].ToString();
                    List<string> values = new List<string>();
                    GetOracleParameterValues(out values, dt.Rows[0]["VERIFYSTORE"].ToString());
                    ctrlSA.ExecuteStoreProcedure(dt.Rows[0]["VERIFYSTORE"].ToString(), values);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int GetSubType(string v_rptid)
        {
            using (var ctrlSA = new SAController())
            {
                int SubType = 0;
                List<string> fldid = new List<string>();
                fldid.Add(v_rptid);
                DataContainer dc;
                ctrlSA.ExecuteProcedureFillDataset(out dc, "SP_GET_SUBTYPE", fldid);
                DataTable dt = dc.DataTable;
                if (dt.Rows.Count > 0)
                {
                    SubType = int.Parse(dt.Rows[0][0].ToString());
                }

                return SubType;
            }
        }

        private void CheckRptMasterDetails(string v_rptid, List<string> v_sheetname)
        {
            using (var ctrlSA = new SAController())
            {
                Boolean bCheck = true;
                List<string> fldid = new List<string>();
                fldid.Add(v_rptid);
                DataContainer dc;
                ctrlSA.ExecuteProcedureFillDataset(out dc, "sp_modimport_sel", fldid);
                DataTable dt = dc.DataTable;
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < v_sheetname.Count; j++)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (v_sheetname[j].IndexOf("_") > 0)
                            {
                                if (v_sheetname[j].IndexOf(dt.Rows[i]["MODEXEC"].ToString()) < 0)
                                {
                                    bCheck = false;
                                }
                                else
                                {
                                    bCheck = true;
                                    break;
                                }
                            }
                        }
                        if (bCheck == false)
                        {
                            throw ErrorUtils.CreateError(172);
                        }
                    }
                }

            }
        }
        void ExecuteImportThread_ProcessComplete(object sender, EventArgs e)
        {
            //var thread = sender as ImportExecuteThread;

            //if (thread != null)
            //{
            //foreach (var importRow in thread.ImportRows)
            //{
            //    if (thread.ErrorInfos.ContainsKey(importRow))
            //    {
            //        importRow.RowError = thread.ErrorInfos[importRow];
            //        importRow["COLUMN_ERROR"] = thread.ErrorInfos[importRow];
            //    }
            //    else 
            //    {
            //        //add by trungtt - 20130508 - verify data before import
            //        if(Program.blVerifyImport == false)
            //            m_ExcelBufferTable.Rows.Remove(importRow);
            //    }
            //}
            //if (Program.blEnableImport == true)
            //{
            //    btnImport.Enabled = true;
            //    wzpSetting.AllowNext = true;
            //}
            //else if (Program.blEnableImport == false && ImportInfo.VerifyData == "N") 
            //{
            //    btnImport.Enabled = true;
            //    wzpSetting.AllowNext = false;
            //}
            //else
            //{
            //    btnImport.Enabled = false;
            //    wzpSetting.AllowNext = false;
            //}

            wzpSetting.AllowBack = true;
            wzpSetting.AllowCancel = true;
            //}
        }

        private void wzpReadFile_PageCommit(object sender, EventArgs e)
        {
            wzpSetting.AllowNext = false;
        }

        //private void gvMain_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        //{
        //    var rowIndex = e.RowHandle;
        //    if (rowIndex >= 0)
        //    {
        //        e.Info.DisplayText = rowIndex.ToString();
        //        e.Appearance.TextOptions.HAlignment = HorzAlignment.Far;

        //        var row = (DataRowView)gvMain.GetRow(rowIndex);
        //        if (row.Row.HasErrors)
        //        {
        //            e.Info.ImageIndex = 4;
        //        }
        //        else
        //        {
        //            e.Info.ImageIndex = -1;
        //        }
        //    }
        //}

        //private void gvMain_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        //{
        //    if(e.Column != null)
        //    {
        //        if (e.Column.FieldName == "COLUMN_ERROR")
        //        {
        //            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold | FontStyle.Italic);
        //            e.Appearance.ForeColor = Color.Red;
        //        }
        //        else if (1 == ImportFields.Count(
        //                    item => 
        //                        ImportControlByID[item.FieldID].EditValue != null &&
        //                        ImportControlByID[item.FieldID].EditValue.ToString() == e.Column.FieldName))
        //        {
        //            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
        //            e.Appearance.ForeColor = Color.Blue;
        //        }
        //        else
        //        {
        //            e.Appearance.Reset();
        //        }
        //    }
        //}

        #endregion

        public ucIMWizard()
        {
            InitializeComponent();
            m_ExcelBufferTable = new DataTable();
            lblStatusText.Text = null;

        }

        private static ImportRange[] CalcRanges(string importRows)
        {
            var ranges = new List<ImportRange>();
            var strRanges = importRows.Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var strRange in strRanges)
            {
                if (strRange.Contains("-"))
                {
                    var values = strRange.Split(new[] { "-" }, StringSplitOptions.None);
                    var min = string.IsNullOrEmpty(values[0]) ? 0 : int.Parse(values[0]);
                    var max = string.IsNullOrEmpty(values[1]) ? int.MaxValue : int.Parse(values[1]);
                    ranges.Add(new ImportRange(min, max));
                }
                else
                {
                    ranges.Add(new ImportRange(int.Parse(strRange), int.Parse(strRange)));
                }
            }

            return ranges.ToArray();
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
            get { return Language.Layout; }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = IMPORTMASTER.EXPORT_FILE_EXTENSIONS
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentThread = new ImportExportErrorThread(this)
                {
                    FileName = saveDialog.FileName,
                    ExcelBufferTable = m_ExcelBufferTable,
                    JobName = "Đang đọc file..."
                };

                CurrentThread.DoUpdateGUI += Thread_DoUpdateGUI;
                CurrentThread.Start();
            }
        }

        private void wzMain_Click(object sender, EventArgs e)
        {

        }

        private void wzpWelcome_Click(object sender, EventArgs e)
        {

        }

        private void gcMain_Click(object sender, EventArgs e)
        {

        }

        private void wzMain_Leave(object sender, EventArgs e)
        {
            Program.blEnableImport = false;
            CloseModule();
        }
        #region Import Excel New
        public static string getConnectionString(string mFile)
        {

            string strXlsConnString = null;
            FileInfo fi = new FileInfo(mFile);
            strXlsConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                  mFile + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1'";
            return strXlsConnString;
        }
        public static List<string> getExcelSheets(string mFile)
        {
            try
            {
                var workbook = new Workbook();
                workbook.Open(mFile);
                Worksheets worksheets = workbook.Worksheets;
                List<string> strExcelSheetNames = new List<string>();
                string sheetName;
                for (var i = 0; i < worksheets.Count; i++)
                {
                    sheetName = worksheets[i].Name;
                    strExcelSheetNames.Add(sheetName);
                }
                return strExcelSheetNames;
            }
            catch (Exception exp)
            {
                throw new Exception("Không lấy được thông tin các sheet của file Excel " + exp.Message, exp);
            }
        }

        public static DataSet getXLData(string xlSheetName, string xlFileName, string AdditionalFields)
        {
            try
            {
                DataSet xlTDS = new DataSet("xlDataSet");
                OleDbConnection xlConn;
                OleDbDataAdapter xlDA;


                if (xlSheetName.Substring(0, 1) == "@")
                {
                    DataTable xltDT = new DataTable();

                    xlConn = new OleDbConnection(getConnectionString(xlFileName));
                    xlConn.Open();
                    xlDA = new OleDbDataAdapter("Select" + AdditionalFields + " * from [" + xlSheetName + "$]", xlConn);
                    xlDA.Fill(xltDT);
                    xlConn.Close();
                    xlConn.Dispose();

                    xltDT.Columns.Add("InvestorName", typeof(String));
                    xltDT.Columns.Add("AccountPlace", typeof(String));
                    xltDT.Columns.Add("InvestorAccount", typeof(String));

                    int n = 1;
                    string strName = null;
                    string strAccount = null;
                    string strAccountPlace = null;
                    DataTable dt = null;
                    foreach (DataRow rows in xltDT.Rows)
                    {
                        if (rows[0].ToString() == "@" && n == 3)
                        {
                            strAccount = rows[2].ToString();
                            n = 1;
                        }
                        else if (rows[0].ToString() == "@" && n == 2)
                        {
                            strAccountPlace = rows[2].ToString();
                            n++;
                        }
                        else if (rows[0].ToString() == "@" && n == 1)
                        {
                            dt = xltDT.Clone();
                            xlTDS.Tables.Add(dt);
                            strName = rows[2].ToString();
                            n++;
                        }
                        else if (dt != null && rows[0].ToString() != "@" && rows[0].ToString() != "STT")
                        {
                            rows["InvestorName"] = strName;
                            rows["AccountPlace"] = strAccountPlace;
                            rows["InvestorAccount"] = strAccount;
                            dt.ImportRow(rows);
                        }
                    }

                    return xlTDS;
                }
                else
                {
                    xlConn = new OleDbConnection(getConnectionString(xlFileName));

                    xlConn.Open();
                    xlDA = new OleDbDataAdapter("Select" + AdditionalFields +
                                            " * from [" + xlSheetName + "$]", xlConn);
                    xlDA.Fill(xlTDS);
                    xlConn.Close();
                    xlConn.Dispose();

                    RemoveEmptyRows(xlTDS.Tables[0], (AdditionalFields.Length -
                              AdditionalFields.ToLower().Replace(" as ", "").Length) / 4, xlSheetName);
                    return xlTDS;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đọc file Excel ! ", ex);
            }
        }

        private static DataSet ReadFileExcel(string xlSheetName, string xlFileName)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable ExcelBufferTable = new DataTable();

                var workbook = new Workbook();
                workbook.Open(xlFileName);
                Worksheet worksheet = workbook.Worksheets[xlSheetName];

                var rowLength = worksheet.Cells.MaxRow + 1;
                var colLength = worksheet.Cells.MaxColumn + 1;

                var befcol = 'A' - 1;
                var col = 'A';
                for (var i = 0; i < colLength; i++)
                {
                    if (col > 'Z')
                    {
                        col = 'A';
                        befcol++;
                    }

                    if (befcol == 'A' - 1)
                        ExcelBufferTable.Columns.Add(new DataColumn(string.Format("{0}", char.ConvertFromUtf32(col)), typeof(object)));
                    else
                        ExcelBufferTable.Columns.Add(new DataColumn(string.Format("{0}{1}", char.ConvertFromUtf32(befcol), char.ConvertFromUtf32(col)), typeof(object)));

                    col++;
                }

                for (var i = 0; i < rowLength; i++)
                {
                    var row = ExcelBufferTable.NewRow();
                    ExcelBufferTable.Rows.Add(row);

                    for (var j = 0; j < colLength; j++)
                    {
                        // Convert value 
                        switch (worksheet.Cells[i, j].Type)
                        {
                            case CellValueType.IsString:
                                row[j] = worksheet.Cells[i, j].StringValue;
                                break;
                            case CellValueType.IsNumeric:
                                row[j] = worksheet.Cells[i, j].DoubleValue;
                                break;
                            case CellValueType.IsBool:
                                row[j] = worksheet.Cells[i, j].BoolValue;
                                break;
                            case CellValueType.IsDateTime:
                                row[j] = worksheet.Cells[i, j].DateTimeValue.ToShortDateString();
                                break;
                            case CellValueType.IsUnknown:
                                row[j] = worksheet.Cells[i, j].StringValue;
                                break;
                            case CellValueType.IsNull:
                                break;
                        }

                        //row[j] = worksheet.Cells[i, j].Value;
                    }
                }
                RemoveEmptyRows(ExcelBufferTable, 0, xlSheetName);

                ds.Tables.Add(ExcelBufferTable);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void RemoveEmptyRows(DataTable dtbl, System.Int32 intNumberOfFieldsToIgnore, string sheetname)
        {
            try
            {
                System.String strFilter = "";
                System.String strFilter1 = "";
                //Check at least 3/4th of the columns for null value
                System.Int32 intAvgColsToCheck =
                  Convert.ToInt32((dtbl.Columns.Count - intNumberOfFieldsToIgnore) * 1);
                //Can't entertain checking less than three columns.
                if (intAvgColsToCheck < 3)
                {
                    intAvgColsToCheck = dtbl.Columns.Count;
                }

                //Building the filter string that checks null
                //value in 3/4th of the total column numbers...

                //We will be doing it in reverse, checking the last three-quarter columns
                System.Int32 lngEnd = dtbl.Columns.Count;
                lngEnd = lngEnd - intAvgColsToCheck;
                for (int lngStartColumn = dtbl.Columns.Count;
                     lngStartColumn > lngEnd; lngStartColumn--)
                {
                    strFilter += "[" + dtbl.Columns[lngStartColumn - 1].ColumnName +
                                 "] IS NULL AND ";
                    //AND to concatenate the next column in the filter
                }

                //Remove the trailing AND
                if (strFilter.Length > 1)
                //At least one column is added (and thus, the trailing AND)
                {
                    strFilter = strFilter.Remove(strFilter.Length - 4);
                }
                DataRow[] drows;
                drows = dtbl.Select(strFilter);
                int SubType = 0;
                // Apply rule doi voi bao cao tai chinh SMS
                //if (Program.AppName == CONSTANTS.AppNameSMS)
                //{
                //    SubType = GetSubType(Program.rptid);
                //    if (SubType == 1)
                //    {
                //        for (int lngStartColumn = dtbl.Columns.Count; lngStartColumn > 2; lngStartColumn--)
                //        {
                //            strFilter1 += "[" + dtbl.Columns[lngStartColumn - 1].ColumnName +
                //                         "] IS NULL AND ";
                //        }
                //        if (strFilter1.Length > 1)
                //        {
                //            strFilter1 = strFilter1.Remove(strFilter1.Length - 4);
                //        }
                //        drows = dtbl.Select(strFilter1);
                //    }

                //}

                //Remove the rows that are empty...
                foreach (DataRow drow in drows)
                {
                    dtbl.Rows.Remove(drow);
                }

                if (SubType == 1)
                {
                    CheckData(dtbl, sheetname);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CheckData(DataTable data, string sheetname)
        {
            for (int i = 1; i < data.Rows.Count; i++)
            {
                string fldid = "";
                int count = 0;
                try
                {
                    fldid = data.Rows[i][1].ToString();
                }
                catch
                {

                }
                DataRowCollection dtrow = data.Rows;
                for (int j = 1; j < dtrow.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dtrow[j][1].ToString()))
                    {
                        if (dtrow[j][1].ToString() == fldid)
                        {
                            count = count + 1;
                            if (count == 2)
                            {
                                throw ErrorUtils.CreateErrorWithSubMessage(310, string.Format("Chỉ tiêu [{0}] của sheet [{1}] bị trùng ", fldid, sheetname));
                            }
                        }

                    }
                }

            }


        }
        #endregion
        void workerFunction(string strStatusText)
        {
            this.Invoke(new poplateLableDelegate(populateLable), new object[] { strStatusText });
            Thread.Sleep(10);
        }
        void populateLable(string text)
        {
            lblStatusText.Text = text;
        }

        private void wzpSetting_Click(object sender, EventArgs e)
        {

        }
        private string ConvertFileExcel(string filename)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                app.Visible = false;
                app.ScreenUpdating = false;
                app.DisplayAlerts = false;
                Microsoft.Office.Interop.Excel.Workbook workbook = null;
                workbook = app.Workbooks.Open(filename);
                var outPathname = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + RandomString(10, false) + ".xlsx";

                //workbook.SaveAs(outPathname, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12,
                //    System.Reflection.Missing.Value,
                //    System.Reflection.Missing.Value,
                //    false,
                //    false,
                //    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                //    false,
                //    false,
                //    System.Reflection.Missing.Value,
                //    System.Reflection.Missing.Value,
                //    System.Reflection.Missing.Value);

                workbook.SaveAs(outPathname, Type.Missing,
                                 Type.Missing, Type.Missing, true, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                 Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);

                workbook.Close(true, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                app.Quit();

                releaseObject(workbook);
                releaseObject(app);

                return outPathname;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                bool excelWasRunning = System.Diagnostics.Process.GetProcessesByName("Excel").Length > 0;
                if (!excelWasRunning)
                {
                    app.Quit();
                }
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
                return sb.ToString().ToLower();
            return sb.ToString();

        }
        private void ShowMessVNPTCA(int ErrCode, string ErrCodeString)
        {
            int iErr = 996;
            switch (ErrCode)
            {
                case 100:
                    iErr = 990;
                    break;
                case 101:
                    iErr = 991;
                    break;
                case 102:
                    iErr = 992;
                    break;
                case 400:
                    iErr = 993;
                    break;
                case 402:
                    iErr = 994;
                    break;
                case 403:
                    iErr = 995;
                    break;
                case 105:
                    iErr = 988;
                    break;
                case 106:
                    iErr = 989;
                    break;
                case 500:
                    iErr = 996;
                    File.AppendAllText("LastErrors.log", string.Format("{0} : {1}-{2}\r\n-------------\r\n", DateTime.Now.ToLongDateString(), "Lỗi 500 : ", ErrCodeString));
                    break;
            }
            var cex = ErrorUtils.CreateError(iErr);
            ShowError(cex);
        }
    }
}
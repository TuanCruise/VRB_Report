using DevExpress.XtraGrid;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using FIS.AppClient.Interface;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace FIS.AppClient.Controls
{
    public partial class ucTreeView : ucModule, IParameterFieldSupportedModule
    {
        public int moduleid;
        public ucTreeView()
        {
            InitializeComponent();
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            List<string> values = null;
            values = new List<string>();
            //values.Add(Program.treeModuleID.ToString());
            values.Add(ModuleInfo.ModuleID);
            using (var ctrlSA = new SAController())
            {
                DataContainer con;
                ctrlSA.GetTreeStore(out con, values);
                AssignFieldValuesFromResult(con);
                DataTable dt = con.DataTable;
                SYSTEM_STORE_PROCEDURES.MODULE_TREE = dt.Rows[0][0].ToString();
            }

            try
            {
                LoadData(SYSTEM_STORE_PROCEDURES.MODULE_TREE);
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

        private void LoadData(string storeName)
        {
            using (var ctrlSA = new SAController())
            {
                if (!string.IsNullOrEmpty(storeName))
                {
                    List<string> values;
                    GetOracleParameterValues(out values, storeName);
                    DataContainer con;
                    ctrlSA.ExecuteProcedureFillDataset(out con, storeName, values);
                    AssignFieldValuesFromResult(con);
                    DataTable dt = con.DataTable;
                    treeView.KeyFieldName = "ID";
                    treeView.ParentFieldName = "PARENTID";
                    treeView.DataSource = dt;
                    treeView.ExpandAll();
                    SetFormat();

                    //treeView.Columns[0].Visible = false;
                    //treeView.Columns[1].Visible = false;
                    //treeView.Columns[2].Visible = false;
                    //TransLanguage
                    List<string> values1 = values = new List<string>();
                    //values1.Add(Program.treeModuleID.ToString());
                    values1.Add(ModuleInfo.ModuleID);
                    DataContainer con1;
                    ctrlSA.GetTreeViewLang(out con1, values);
                    AssignFieldValuesFromResult(con1);
                    DataTable dt1 = con1.DataTable;
                    for (int j = 0; j <= dt1.Rows.Count - 1; j++)
                    {
                        string langname = Convert.ToString(dt1.Rows[j]["LANGNAME"]);
                        string langvalue = Convert.ToString(dt1.Rows[j]["LANGVALUE"]);
                        if (langname.Split('.')[1].ToUpper() == "TITLE")
                        {
                            lbTitle.Text = langvalue;
                            this.Tag = langvalue;
                        }
                        else
                        {
                            for (int k = 0; k <= treeView.Columns.Count - 1; k++)
                            {
                                if (treeView.Columns[k].Name + ".LABEL" == "col" + langname.Split('$')[1].ToUpper())
                                {
                                    treeView.Columns[k].Caption = langvalue;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

            var saveDialog = new SaveFileDialog
            {
                Filter = IMPORTMASTER.EXPORT_FILE_EXTENSIONS
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                options.ShowGridLines = true;
                options.ExportHyperlinks = true;
                options.Suppress256ColumnsWarning = true;
                options.Suppress65536RowsWarning = true;
                treeView.ExportToXls(saveDialog.FileName, options);
            }
        }


        private void SetFormat()
        {

            TreeListColumnCollection cols = treeView.Columns;
            TreeListColumn colLevel = cols["CAP"];
            colLevel.OptionsColumn.AllowEdit = true;
            colLevel.OptionsColumn.ReadOnly = false;
            colLevel.Visible = true;

            DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition f1 = treeView.FormatConditions[0];
            f1.Condition = FormatConditionEnum.Equal;
            f1.Value1 = "8";
            f1.ApplyToRow = true;
            DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition f2 = treeView.FormatConditions[1];
            f2.Condition = FormatConditionEnum.Equal;
            f2.Value1 = "9";
            f2.ApplyToRow = true;
            DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition f3 = treeView.FormatConditions[2];
            f3.Condition = FormatConditionEnum.Equal;
            f3.Value1 = "0";
            f3.ApplyToRow = true;

            treeView.BeginUnboundLoad();

            foreach (TreeListNode gpNode in treeView.Nodes)
            {
                gpNode.SetValue(0, 8);
                foreach (TreeListNode pNode in gpNode.Nodes)
                {
                    pNode.SetValue(0, 9);
                }
            }

            treeView.EndUnboundLoad();

            f1.Column = colLevel;
            f2.Column = colLevel;
            f3.Column = colLevel;

            colLevel.Visible = false;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Controllers;
using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucModuleFieldMaker : ucModule,
        IParameterFieldSupportedModule,
        ICommonFieldSupportedModule,
        IColumnFieldSupportedModule
    {
        public GridCheckMarksUtils CheckMarksUtils { get; set; }
        public ModuleInfo TargetModuleInfo { get; set; }
        public DataTable SchemaInfoTable { get; set; }
        public string SQL_SCRIPT
        {
            get
            {
                return (string)this["C03"];
            }
            set
            {
                this["C03"] = value;
            }
        }

        public ucModuleFieldMaker()
        {
            InitializeComponent();
        }

        protected override void BuildFields()
        {
            CheckMarksUtils = new GridCheckMarksUtils(gvMain, true);
            CheckMarksUtils.CheckMarkColumn.VisibleIndex = 0;
            CheckMarksUtils.CheckMarkColumn.Width = 30;
            CheckMarksUtils.CheckMarkColumn.OptionsColumn.FixedWidth = true;
            CheckMarksUtils.CheckMarkColumn.Fixed = FixedStyle.Left;

            base.BuildFields();
        }

        protected override void BuildButtons()
        {
            base.BuildButtons();
#if DEBUG
            SetupContextMenu(mainLayout);
            SetupSaveLayout(mainLayout);
            SetupSaveSize();
#endif
        }

        public void TestSQLScript()
        {
            try
            {
                CheckMarksUtils.ClearSelection();

                DataContainer container;
                using (var client = new SAController())
                {
                    client.ExecuteSQL(out container, SQL_SCRIPT);
                }

                var table = container.DataTable;

                SchemaInfoTable = new DataTable();
                SchemaInfoTable.Columns.Add(new DataColumn("FLDNAME", typeof(string)));
                SchemaInfoTable.Columns.Add(new DataColumn("FLDTYPE", typeof(string)));
                SchemaInfoTable.Columns.Add(new DataColumn("CTRLTYPE", typeof(string)));
                SchemaInfoTable.Columns.Add(new DataColumn("SCDTYPE", typeof(string)));

                foreach (DataColumn column in table.Columns)
                {
                    var row = SchemaInfoTable.NewRow();
                    row["FLDNAME"] = string.Format((string)this["C04"], column.ColumnName);
                    row["CTRLTYPE"] = "SP";
                    row["SCDTYPE"] = "NBC";

                    if (column.DataType == typeof(String))
                    {
                        row["CTRLTYPE"] = "TB";
                        row["FLDTYPE"] = "STR";
                        row["SCDTYPE"] = "TXC";
                    }

                    if (column.DataType == typeof(DateTime))
                    {
                        row["CTRLTYPE"] = "DT";
                        row["FLDTYPE"] = "DTE";
                        row["SCDTYPE"] = "DTC";
                    }

                    if (column.DataType == typeof(Int32) || column.DataType == typeof(Int16))
                    {
                        row["FLDTYPE"] = "INT";
                    }

                    if (column.DataType == typeof(Int64))
                    {
                        row["FLDTYPE"] = "LNG";
                    }

                    if (column.DataType == typeof(Single))
                    {
                        row["FLDTYPE"] = "FLT";
                    }

                    if (column.DataType == typeof(Double))
                    {
                        row["FLDTYPE"] = "DBL";
                    }

                    if (column.DataType == typeof(Decimal))
                    {
                        row["FLDTYPE"] = "DEC";
                    }

                    SchemaInfoTable.Rows.Add(row);
                }

                gcMain.DataSource = SchemaInfoTable;
                gvMain.BestFitColumns();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public void LoadFieldsFromModuleID()
        {
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

        public GridView GridView
        {
            get { return gvMain; }
        }

        public override void Execute()
        {
            if(ValidateModule())
            {
                base.Execute();
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        for (var i = 0; i < CheckMarksUtils.SelectedCount; i++)
                        {
                            var row = (DataRowView)CheckMarksUtils.GetSelectedRow(i);
                            ctrlSA.ExecuteStoreProcedure("DEV_SP_DEFMODFLD_INS_" + this["C02"], new List<string>
                             {
                                 (string)this["C01"],
                                 (string)row["FLDNAME"],
                                 (string)row["FLDTYPE"],
                                 (string)row["SCDTYPE"],
                                 (string)row["CTRLTYPE"]
                             });
                        }
                    }

                    CloseModule();
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        private void btnCreateFields_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void btnTestSQL_Click(object sender, EventArgs e)
        {
            TestSQLScript();
        }
    }
}

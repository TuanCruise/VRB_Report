using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace FIS.AppClient.Controls
{
    public partial class ucColumnExport : ucModule
    {
        internal DataTable ResultTable { get; set; }
        internal string LastSearchResultKey { get; set; }
        internal string MID { get; set; }
        internal DateTime LastSearchTime { get; set; }
        public GridControl gcMain { get; set; }
        public List<GridBand> Bands { get; set; }
        public List<string[]> listLayout { get; set; }

        public ucColumnExport()
        {
            InitializeComponent();
        }

        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);
            lstExecuteResult.ImageList = ThemeUtils.Image16;
            lbTitle.Text = Language.Title;
        }

        public void InitData()
        {
            var gridView = gcMain.DefaultView as GridView;
            foreach (GridColumn column in gridView.Columns)
            {
                chkLstColumnExport.Items.Add(new CheckedListBoxItem(column.FieldName, column.ToolTip));
            }
            chkLstColumnExport.CheckAll();
        }

        private void VisibleCheckList()
        {
            if (rdgExportTitle.Checked) { chkLstColumnExport.Enabled = false; chkCheckAll.Enabled = false; }
            else { chkLstColumnExport.Enabled = true; chkCheckAll.Enabled = true; }

        }
        private void ucColumnExport_Load(object sender, EventArgs e)
        {
            VisibleCheckList();
            InitData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var exportInfo = ModuleUtils.GetModuleInfo(MID, CODES.DEFMOD.SUBMOD.SEARCH_EXPORT);
                var ucExport = (ucSearchExport)FIS.AppClient.Utils.MainProcess.CreateModuleInstance(exportInfo.ModuleID, exportInfo.SubModule);
                ucExport.LastSearchResultKey = LastSearchResultKey;
                ucExport.LastSearchTime = LastSearchTime;
                ucExport.PrintGrid = gcMain;
                ucExport.listLayout = listLayout;
                ucExport.Bands = Bands;

                if (rdgExportColumn.Checked)
                {
                    var gridView = gcMain.DefaultView as GridView;
                    DataTable columnRemove = new DataTable();
                    columnRemove.Columns.Add("Value", typeof(string));
                    for (var i = 0; i < chkLstColumnExport.Items.Count; i++)
                    {
                        CheckState chkSate;
                        chkSate = chkLstColumnExport.GetItemCheckState(i);
                        if (chkSate == CheckState.Unchecked)
                        {
                            var columnValue = chkLstColumnExport.GetItemValue(i).ToString();
                            foreach (GridColumn column in gridView.Columns)
                            {
                                if (column.FieldName == columnValue)
                                {
                                    columnRemove.Rows.Add(columnValue);
                                    break;
                                }
                            }
                        }
                    }
                    ucExport.columnRemove = columnRemove;
                    ucExport.modExport = 1;
                }
                this.CloseModule();
                ucExport.ShowDialogModule(this.Parent);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void rdgExportColumn_CheckedChanged(object sender, EventArgs e)
        {
            VisibleCheckList();
        }

        private void rdgExportTitle_CheckedChanged(object sender, EventArgs e)
        {
            VisibleCheckList();
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckAll.Checked) chkLstColumnExport.CheckAll();
            else chkLstColumnExport.UnCheckAll();
        }
    }
}

using DevExpress.Data;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.AppClient.Properties;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace FIS.AppClient.Controls
{
    public partial class ucStatisticsMaster : ucModule,
        IGroupColumnFieldSupportedModule,
        ICommonFieldSupportedModule
    {
        #region ICommonFieldSupportedModule Members

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
        #endregion

        #region IColumnFieldSupportedModule Members
        public GridView GridView
        {
            get { return gvMain; }
        }

        public string GroupLayoutStoredData
        {
            get { return LangUtils.TranslateModuleItem(LangType.MODULE_LAYOUT, ModuleInfo, "Grid"); }
        }
        #endregion

        private readonly int m_MaxPageSize =
            App.Environment.ClientInfo.UserProfile.MaxPageSize;

        private BufferedResultManager BufferResult { get; set; }

        public int SelectedPage { get; set; }

        public StatisticsModuleInfo StatisticsInfo
        {
            get
            {
                return (StatisticsModuleInfo)ModuleInfo;
            }
        }

        public int LastFocusRow { get; set; }

        public ucStatisticsMaster()
        {
            InitializeComponent();

        }

        protected override void BuildButtons()
        {
#if DEBUG
            SetupContextMenu(mainLayout);
            SetupSaveLayout(mainLayout);
            SetupSaveGridLayout(gvMain, Bands);
#endif
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            btnAscending.Text = Language.AscendingCaption;
            btnDescending.Text = Language.DescendingCaption;
            btnNoSort.Text = Language.NoSortCaption;
        }

        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);

            if (StatisticsInfo.FullWidth == CODES.MODSEARCH.FULLWIDTH.YES)
            {
                gvMain.OptionsView.ColumnAutoWidth = true;
            }
        }

        protected override void BuildFields()
        {
            base.BuildFields();

            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
                ((ContainerControl)Parent).ActiveControl = mainLayout;
            }

            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.TABPAGE)
            {
                mainLayout.Focus();
            }

            BestFitColumns();
            checkFormatConditions();
        }

        #region DataGrid Utils
        private void BestFitColumns()
        {
            gvMain.BestFitColumns();
        }
        #endregion

        public void InitSearch()
        {
            try
            {
                //btnExport.Enabled = true;
                btnMail.Enabled = true;

                cboPages.SelectedIndexChanged -= cboPages_SelectedIndexChanged;
                cboPages.ButtonClick -= cboPages_ButtonClick;

                cboPages.SuspendLayout();
                cboPages.Properties.Items.Clear();
                cboPages.ResumeLayout();
                cboPages.Refresh();
                ChangeStatusText(Resources.Search, "Searching");
            }
            catch
            {
            }
        }

        public void ExecuteFetchResult()
        {
            CurrentThread = new WorkerThread(
                delegate
                    {
                        LockUserAction();
                        try
                        {
                            if (StatisticsInfo.PageMode == CODES.MODSEARCH.PAGEMODE.ALL_FROM_DATASET)
                            {
                                BufferResult.GetFullBuffer();
                            }
                            else if (StatisticsInfo.PageMode == CODES.MODSEARCH.PAGEMODE.APPEND_FROM_READER)
                            {
                                BufferResult.GetMoreRows(SelectedPage);
                            }
                            else
                            {
                                BufferResult.GetBuffer(SelectedPage);
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                        }
                        finally
                        {
                            UnLockUserAction();
                        }
                    }, this);

            CurrentThread.ProcessComplete += ExecuteFetchResult_ProcessComplete;
            CurrentThread.Start();
        }

        public void Execute_ProcessComplete(object sender, EventArgs e)
        {
            if (BufferResult != null && BufferResult.IsNotEmpty)
            {
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        var exportInfo = ModuleUtils.GetModuleInfo(ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.SEARCH_EXPORT);
                        ctrlSA.CheckRole(exportInfo);
                        var sendMailInfo = ModuleUtils.GetModuleInfo(ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.SEND_MAIL);
                        ctrlSA.CheckRole(sendMailInfo);
                        btnMail.Enabled = true;
                    }
                }
                catch
                {
                }
            }
        }

        public void ExecuteFetchResult_ProcessComplete(object sender, EventArgs e)
        {
            try
            {
                UpdatePageList();
                UpdateStatistic();
                UpdateDataSource();
            }
            catch (Exception ex)
            {
                ShowError(ex);
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

        public override void LockUserAction()
        {
            base.LockUserAction();

            if (!InvokeRequired)
            {
                ShowWaitingBox();
                gcMain.DataSource = null;
                Enabled = false;
            }
        }

        public void UpdatePageList()
        {
            cboPages.SelectedIndexChanged -= cboPages_SelectedIndexChanged;
            cboPages.ButtonClick -= cboPages_ButtonClick;

            if (BufferResult != null && BufferResult.IsNotEmpty)
            {
                var buttons = new List<EditorButton>();
                cboPages.SuspendLayout();

                while (cboPages.Properties.Buttons.Count > 1)
                {
                    cboPages.Properties.Buttons.RemoveAt(1);
                }

                var showBackButton = (SelectedPage != 0);
                var showNextButton = (SelectedPage != BufferResult.MaxPage);

                if (StatisticsInfo.PageMode == CODES.MODSEARCH.PAGEMODE.APPEND_FROM_READER)
                {
                    cboPages.Properties.Buttons[0].Visible = false;
                    cboPages.Properties.Items.Clear();
                    cboPages.Properties.NullText = string.Format(Language.MorePageInfo, BufferResult.Rows.Count);

                    var moreButton = new EditorButton
                    {
                        Kind = ButtonPredefines.Plus,
                        IsLeft = false,
                        Tag = SelectedPage + 1,
                        Caption = Language.MoreRowsCaption,
                        ImageLocation = ImageLocation.MiddleLeft
                    };
                    buttons.Add(moreButton);

                    showBackButton = false;
                    showNextButton = false;
                }
                else if (StatisticsInfo.PageMode == CODES.MODSEARCH.PAGEMODE.PAGE_FROM_READER)
                {
                    cboPages.Properties.Buttons[0].Visible = false;
                    cboPages.Properties.ShowDropDown = ShowDropDown.Never;
                    cboPages.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;

                    for (var i = BufferResult.MinPage; i <= BufferResult.MaxPage; i++)
                    {
                        var pageButton = new EditorButton { Kind = ButtonPredefines.Glyph, Tag = i };
                        if (i == SelectedPage)
                        {
                            pageButton.Caption = string.Format("[{0}]", i + 1);
                            pageButton.Enabled = false;
                        }
                        else
                        {
                            pageButton.Caption = string.Format("{0}", i + 1);
                            pageButton.Width = 20;
                        }
                        buttons.Add(pageButton);
                    }

                    // TODO: Optimize code
                    cboPages.Properties.NullText = string.Format(Language.PageInfo, SelectedPage + 1, BufferResult.StartRow + 1, BufferResult.StartRow + BufferResult.Rows.Count);
                }
                else if (StatisticsInfo.PageMode == CODES.MODSEARCH.PAGEMODE.ALL_FROM_DATASET)
                {
                    cboPages.Properties.Buttons[0].Visible = false;
                    cboPages.Properties.ShowDropDown = ShowDropDown.Never;
                    cboPages.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
                    cboPages.Properties.NullText = string.Format(Language.PageInfo, SelectedPage + 1, BufferResult.StartRow + 1, BufferResult.StartRow + BufferResult.Rows.Count);
                }
                else
                {
                    cboPages.Properties.Items.Clear();
                    var bottom = 1;
                    for (var i = 0; i <= BufferResult.MaxPage; i++)
                    {
                        cboPages.Properties.Items.Add(new ImageComboBoxItem
                        {
                            // TODO: Optimize code
                            Description = string.Format(Language.PageInfo, string.Format("{0}/{1}", i + 1, BufferResult.MaxPage + 1), bottom, Math.Min(bottom + m_MaxPageSize, BufferResult.BufferSize)),
                            Value = i,
                            ImageIndex = ThemeUtils.GetImage16x16Index("PAGE")
                        });
                        bottom = bottom + m_MaxPageSize;
                    }
                    cboPages.SelectedIndex = SelectedPage;
                }

                if (showBackButton)
                {
                    var pageButton = new EditorButton
                    {
                        Kind = ButtonPredefines.Left,
                        IsLeft = true,
                        Tag = (SelectedPage - 1),
                        Width = 20
                    };
                    buttons.Insert(0, pageButton);
                }

                if (showNextButton)
                {
                    var pageButton = new EditorButton
                    {
                        Kind = ButtonPredefines.Right,
                        Tag = (SelectedPage + 1),
                        Width = 20
                    };
                    buttons.Add(pageButton);
                }

                cboPages.Properties.Buttons.AddRange(buttons.ToArray());

                cboPages.SelectedIndexChanged += cboPages_SelectedIndexChanged;
                cboPages.ButtonClick += cboPages_ButtonClick;

                cboPages.ResumeLayout();
            }
            else
            {
                cboPages.Properties.Items.Clear();
                cboPages.EditValue = null;
            }
        }

        public void UpdateDataSource()
        {
            if (BufferResult != null && BufferResult.IsNotEmpty)
            {
                BufferResult.EndLoadData();

                LastFocusRow = gvMain.FocusedRowHandle;
                if (BufferResult.Rows.Count > 0)
                {
                    btnExport.Enabled = true;
                }

                gcMain.DataSource = BufferResult;
                gvMain.ExpandAllGroups();

                gvMain.FocusedRowHandle = LastFocusRow;
                gvMain.Focus();

                if (StatisticsInfo.AutoFitWidthColumns != CODES.MODSEARCH.AUTOFITWIDTH.NO)
                    BestFitColumns();
            }
            else
            {
                gcMain.DataSource = null;
                btnExport.Enabled = false;
            }
        }

        public void UpdateStatistic()
        {
            if (string.IsNullOrEmpty(StatisticsInfo.StatisticQuery))
            {
                if (StatisticsInfo.PageMode == CODES.MODSEARCH.PAGEMODE.PAGE_FROM_READER)
                {
                    ChangeStatusText(Resources.SearchResult, "Result", BufferResult.BufferSize);
                }
                else
                {
                    ChangeStatusText(Resources.SearchResult, "FullResult", BufferResult.BufferSize);
                }
            }
        }

        public void ChangeStatusText(Image statusImage, string status, params object[] objs)
        {
            var strStatusText = Language.GetSpecialStatus(status);
            txtSearchStatus.Text = string.Format(strStatusText, objs);

            btnCopyStatus.Image = statusImage;
            btnCopyStatus.Visible = true;
        }

        public void DisposeLastSearchResult()
        {
            if (BufferResult != null)
            {
                BufferResult.Dispose();
                BufferResult = null;
            }
        }

        public override void Execute()
        {
            if (ValidateModule())
            {
                DisposeLastSearchResult();
                InitSearch();

                List<string> values;
                GetOracleParameterValues(out values, StatisticsInfo.StoreName);

                ExecuteSearch(values);
            }
        }

        public void ExecuteSearch(List<string> values)
        {
            CurrentThread = new WorkerThread(
                delegate
                    {
                        LockUserAction();
                        try
                        {
                            using (var ctrlSA = new SAController())
                            {
                                string lastSearchResultKey;
                                DateTime lastSearchTime;

                                ctrlSA.ExecuteStatistics(out lastSearchResultKey, out lastSearchTime, ModuleInfo.ModuleID, ModuleInfo.SubModule, values);
                                BufferResult = new BufferedResultManager(ModuleInfo, lastSearchResultKey, lastSearchTime);
                                ExecuteFetchResult();
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                            UnLockUserAction();
                        }
                    }, this);

            CurrentThread.ProcessComplete += Execute_ProcessComplete;
            CurrentThread.Start();
        }

        private void cboPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedPage = cboPages.SelectedIndex;
            ExecuteFetchResult();
        }

        private void cboPages_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Tag != null)
                switch (e.Button.Tag.ToString())
                {
                    case "BACK":
                        SelectedPage--;
                        break;
                    case "NEXT":
                        SelectedPage++;
                        break;
                    default:
                        try
                        {
                            SelectedPage = int.Parse(e.Button.Tag.ToString());
                            ExecuteFetchResult();
                        }
                        catch
                        {
                        }
                        break;
                }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void btnCopyStatus_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtSearchStatus.Text);
        }

        private void gcMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.C && e.Control && e.Shift)
                {
                    var value = gvMain.GetFocusedValue();
                    if (value != null)
                        Clipboard.SetText(value.ToString(), TextDataFormat.UnicodeText);
                }
            }
            catch
            {

            }
        }

        private void mnuSort_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var gridColumn = mnuSort.Tag as GridColumn;
            if (gridColumn != null)
            {
                if (e.ClickedItem == btnAscending)
                {
                    gridColumn.SortOrder = ColumnSortOrder.Ascending;
                }
                if (e.ClickedItem == btnDescending)
                {
                    gridColumn.SortOrder = ColumnSortOrder.Descending;
                }
                if (e.ClickedItem == btnNoSort)
                {
                    gridColumn.SortOrder = ColumnSortOrder.None;
                }
            }
        }

        private void gvMain_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            var gridColumn = e.HitInfo.Column;

            e.Allow = false;
            if (gridColumn != null && e.HitInfo.InRowCell)
            {
                switch (gridColumn.SortOrder)
                {
                    case ColumnSortOrder.Ascending:
                        btnAscending.Enabled = false;
                        btnDescending.Enabled = true;
                        btnNoSort.Enabled = true;
                        break;
                    case ColumnSortOrder.Descending:
                        btnAscending.Enabled = true;
                        btnDescending.Enabled = false;
                        btnNoSort.Enabled = true;
                        break;
                    default:
                        btnAscending.Enabled = true;
                        btnDescending.Enabled = true;
                        btnNoSort.Enabled = false;
                        break;
                }

                mnuSort.Tag = gridColumn;
                e.Allow = true;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var expColInfo = SysvarUtils.GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_EXPORT_BY_COLUMN);
                if (expColInfo == CONSTANTS.Yes)
                {
                    var ucColumnEx = (ucColumnExport)FIS.AppClient.Utils.MainProcess.CreateModuleInstance("CLEXP");
                    ucColumnEx.gcMain = gcMain;
                    ucColumnEx.MID = ModuleInfo.ModuleID;
                    ucColumnEx.LastSearchResultKey = BufferResult.LastSearchResultKey;
                    ucColumnEx.LastSearchTime = BufferResult.LastSearchTime;
                    var groupColumnFieldSupportedModule = this as IGroupColumnFieldSupportedModule;
                    if (groupColumnFieldSupportedModule.GroupLayoutStoredData != null)
                    {
                        var sr = new StringReader(groupColumnFieldSupportedModule.GroupLayoutStoredData);
                        var serializer = new XmlSerializer(typeof(List<string[]>));
                        var listLayout = (List<string[]>)serializer.Deserialize(sr);
                        ucColumnEx.listLayout = listLayout;
                        ucColumnEx.Bands = Bands;
                    }
                    ucColumnEx.ShowDialogModule(this);
                }
                else
                {
                    var expInfo = SysvarUtils.GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_EXPORT);
                    var exportInfo = ModuleUtils.GetModuleInfo(ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.SEARCH_EXPORT);
                    var ucExport = (ucSearchExport)MainProcess.CreateModuleInstance(exportInfo.ModuleID, exportInfo.SubModule, "CALL_MODULE");
                    if (expInfo == CONSTANTS.Yes)
                    {
                        ucExport.LastSearchResultKey = BufferResult.LastSearchResultKey;
                        ucExport.LastSearchTime = BufferResult.LastSearchTime;
                        ucExport.PrintGrid = gcMain;
                        ucExport.ShowDialogModule(this);
                    }
                    else
                    {
                        var saveDialog = new SaveFileDialog
                        {
                            Filter = IMPORTMASTER.EXPORT_FILE_EXTENSIONS
                        };
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            ucExport.FileName = saveDialog.FileName;
                            ucExport.LastSearchResultKey = BufferResult.LastSearchResultKey;
                            ucExport.LastSearchTime = BufferResult.LastSearchTime;
                            ucExport.PrintGrid = gcMain;
                            ucExport.Execute();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnMail_Click(object sender, EventArgs e)
        {
            try
            {
                var sendMailInfo = ModuleUtils.GetModuleInfo(ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.SEND_MAIL);
                var ucSendMail = (ucSendMail)MainProcess.CreateModuleInstance(sendMailInfo.ModuleID, sendMailInfo.SubModule);
                //ucExport.LastSearchResultKey = BufferResult.LastSearchResultKey;
                //ucExport.LastSearchTime = BufferResult.LastSearchTime;
                //ucExport.PrintGrid = gcMain;
                ucSendMail.ShowDialogModule(this);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        //TUDQ them
        public void checkFormatConditions()
        {
            using (var ctrlSA = new SAController())
            {
                List<string> values = new List<string>();
                DataContainer con;
                values.Add(ModuleInfo.ModuleID);
                values.Add(App.Environment.ClientInfo.UserName);
                ctrlSA.ExecuteProcedureFillDataset(out con, "sp_formatconditions_selbyid", values);
                DataTable dt = con.DataTable;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        StyleFormatCondition condition1 = new DevExpress.XtraGrid.StyleFormatCondition();
                        if (Convert.ToString(dt.Rows[i]["BACKCOLOR"]) != "0")
                            condition1.Appearance.BackColor = Color.FromName(dt.Rows[i]["BACKCOLOR"].ToString());
                        else condition1.Appearance.BackColor = Color.Transparent;
                        if (Convert.ToString(dt.Rows[i]["FORECOLOR"]) != "0")
                            condition1.Appearance.ForeColor = Color.FromName(dt.Rows[i]["FORECOLOR"].ToString());
                        else condition1.Appearance.ForeColor = Color.Transparent;
                        condition1.Appearance.Options.UseBackColor = true;
                        condition1.Appearance.Options.UseForeColor = true;
                        if (dt.Rows[i]["BOLD"].ToString() == "Y" && dt.Rows[i]["ITALIC"].ToString() == "Y")
                            condition1.Appearance.Font = new Font(this.Font, FontStyle.Bold | FontStyle.Italic);
                        else if (dt.Rows[i]["BOLD"].ToString() == "Y")
                            condition1.Appearance.Font = new Font(this.Font, FontStyle.Bold);
                        else if ((dt.Rows[i]["ITALIC"].ToString() == "Y"))
                            condition1.Appearance.Font = new Font(this.Font, FontStyle.Italic);
                        condition1.Condition = FormatConditionEnum.Expression;
                        condition1.Expression = dt.Rows[i]["CONDITIONS"].ToString();
                        condition1.Appearance.Options.UseFont = true;
                        gvMain.FormatConditions.Add(condition1);
                    }
                }
            }
        }
        GridColumnSummaryItem sumItem;
        private void gvMain_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Summary)
            {
                sumItem = e.HitInfo.FooterCell.SummaryItem as GridColumnSummaryItem;
                DXMenuItem item = new DXMenuItem("Copy to clipboard");
                e.Menu.Items.Add(item);
                item.Click += new EventHandler(item_Click);
            }
        }
        void item_Click(object sender, EventArgs e)
        {
            if (sumItem != null)
            {
                string s = sumItem.SummaryValue.ToString();
                Clipboard.SetText(s);
            }
        }
        //
    }
}
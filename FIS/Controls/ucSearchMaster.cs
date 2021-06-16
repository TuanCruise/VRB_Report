using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraRichEdit;
using FIS.AppClient.Interface;
using FIS.AppClient.Properties;
using FIS.AppClient.Utils;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Extensions;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using Shortcut = System.Windows.Forms.Shortcut;

namespace FIS.AppClient.Controls
{
    public partial class ucSearchMaster : ucModule,
        IConditionFieldSupportedModule,
        IColumnFieldSupportedModule,
        ICommonFieldSupportedModule
    {
        #region IConditionFieldSupportedModule Members
        public LayoutControlGroup ConditionLayoutGroup
        {
            get { return rootConditionGroup; }
        }

        public void UpdateConditionQuery()
        {
            if (RootGroup != null)
            {
                var conditionInstance = RootGroup.GetConditionInstance();
                txtQuery.Text = ModuleUtils.BuildSearchConditionKey(ModuleInfo, conditionInstance);
            }
        }

        public UISearchGroup RootGroup { get; set; }
        #endregion

        #region IColumnFieldSupportedModule Members
        public GridView GridView
        {
            get { return gvMain; }
        }

        #endregion
        
        #region Properties & Members
        private readonly int m_MaxPageSize = 
            App.Environment.ClientInfo.UserProfile.MaxPageSize;
        private BufferedResultManager BufferResult { get; set; }
        private string m_ErrorStatistics;
        protected int SelectedPage { get; set; }
        protected ButtonInfo m_DefaultSearchButton { get; set; }
        
        public GridCheckMarksUtils CheckMarksUtils { get; set; }
        public RowFormattable SearchStatistic { get; set; }
        //add by trungtt - 20.5.2011
        RichEditControl richEdit;
        //end trungtt
        public SearchModuleInfo SearchInfo
        {
            get
            {
                return (SearchModuleInfo)ModuleInfo;
            }
        }

        public int LastFocusRow { get; set; }
        public string LookUpValues { get; set; }

        #endregion

        #region Override methods
        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);

            if (SearchInfo.GroupBox == CODES.MODSEARCH.GROUPBOX.NO)
            {
                gvMain.OptionsView.ShowGroupPanel = false;
            }

            if (SearchInfo.ShowGroupColumn == CODES.MODSEARCH.SHOWGRPCOL.YES)
            {
                gvMain.OptionsView.ShowGroupedColumns = true;                
                //gvMain.CollapseAllGroups();
            }

            if (SearchInfo.FullWidth == CODES.MODSEARCH.FULLWIDTH.YES)
            {
                gvMain.OptionsView.ColumnAutoWidth = true;
            }

            gvMain.OptionsView.RowAutoHeight = true;
            gvMain.Images = ThemeUtils.Image16;
        }

        protected override void BuildFields()
        {
            if (SearchInfo.ShowCheckBox == CODES.MODSEARCH.SHOWCHECKBOX.YES)
            {
                CheckMarksUtils = new GridCheckMarksUtils(gvMain, false);
                CheckMarksUtils.CheckMarkColumn.VisibleIndex = 0;
                CheckMarksUtils.CheckMarkColumn.Width = 30;
                CheckMarksUtils.CheckMarkColumn.OptionsColumn.AllowMerge = DefaultBoolean.False;
                CheckMarksUtils.CheckMarkColumn.OptionsColumn.FixedWidth = true;
                CheckMarksUtils.CheckMarkColumn.Fixed = FixedStyle.Left;
            }
            //add by trungtt - 20.5.2011
            var fldDetailColumn = SearchInfo.DetailColumn;
            if (fldDetailColumn != null)
            {
                richEdit = new RichEditControl
                {
                    Name = fldDetailColumn
                };
                mainLayout.Controls.Add(richEdit);

                if (CommonLayoutStoredData == null)
                {
                    mainLayout.AddItem(fldDetailColumn, richEdit);
                }
            }
            //end TrungTT
            base.BuildFields();

            if (ConditionFields.Count == 0 && CommonFields.Count == 0)
            {
                gpSearchCondition.Visibility = LayoutVisibility.Never;
            }

            BestFitColumns();            
        }
        
        public override void InitializeLayout()
        {
            base.InitializeLayout();
            txtSearchStatus = (EmptySpaceItem)mainLayout.Items.FindByName("txtSearchStatus");
            txtSearchStatus.TextVisible = false;

            gvMain.HiddenEditor += delegate
            {
                gvMain.UpdateCurrentRow();
            };

            gvMain.HorzScrollVisibility = ScrollVisibility.Auto;
        }

        public override void SetFieldValue(ModuleFieldInfo field, object value)
        {
            if (field.FieldGroup == CODES.DEFMODFLD.FLDGROUP.SEARCH_CONDITION)
            {
                foreach (var condition in RootGroup.Conditions)
                {
                    if (condition.FieldInfo == field)
                    {
                        condition.Value = value;
                    }
                }
            }
            else
            {
                base.SetFieldValue(field, value);
            }
        }

        protected override void BuildButtons()
        {
            List<ButtonInfo> buttons;
            
            if(!SearchInfo.ShowAsLookUpWindow)
            {
                buttons = new List<ButtonInfo>();
                //trungtt - 5.12.2013 - Add Bar Item for Role
                buttons = ModuleUtils.GetSearchButtons(ModuleInfo.ModuleID);
                //ButtonInfo item;
                //DataContainer container;
                //try
                //{
                //    using (var ctrlSA = new SAController())
                //    {
                //        List<string> values = new List<string>();
                //        values.Add(ModuleInfo.ModuleID);

                //        ctrlSA.ExecuteProcedureFillDataset(out container, "sp_list_button", values);
                //        foreach (DataRow rows in container.DataSet.Tables[0].Rows)
                //        {
                //            item = new ButtonInfo();
                //            item.ModuleID = rows[0].ToString();
                //            item.ButtonID = rows[1].ToString();
                //            item.ButtonName = rows[2].ToString();
                //            item.ButtonGroup = rows[3].ToString();
                //            item.BeginGroup = rows[4].ToString();
                //            item.CallModuleID = rows[5].ToString();
                //            item.CallSubModule = rows[6].ToString();
                //            item.ShowConfirm = rows[7].ToString();
                //            item.MultiExecute = rows[8].ToString();
                //            item.ShowOnToolbar = rows[9].ToString();
                //            item.ParameterMode = rows[10].ToString();
                //            item.DBClick = rows[11].ToString();                           
                //            buttons.Add(item);
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                //end TrungTT

            }
            else
            {
                buttons = ModuleUtils.GetSearchButtons(ModuleInfo.ModuleType);
            }

            foreach (var button in buttons)
            {
                if( SearchInfo.DefaultButton != null &&
                    button.ButtonName == SearchInfo.DefaultButton)
                {
                    m_DefaultSearchButton = button;
                    gvMain.RowClick += gvMain_RowClick;
                }

                var barButton = new BarButtonItem
                {
                    Name = button.ButtonName,
                    Tag = button,
                    PaintStyle = BarItemPaintStyle.CaptionGlyph
                };

                if (!string.IsNullOrEmpty(button.CallModuleID))
                {
                    try
                    {
                        var module = ModuleUtils.GetModuleInfo(button.CallModuleID, button.CallSubModule);

                        if (!string.IsNullOrEmpty(module.RoleID))
                        {
                            var role = (from Role r in MainProcess.Roles
                                        where r.RoleID == module.RoleID
                                        select r).FirstOrDefault();
                            if (role != null && role.RoleValue != "Y")
                                barButton.Enabled = false;
                        }
                    }
                    catch
                    {
                    }
                }

                mainBarManager.Items.Add(barButton);
                // barButton
                barButton.Caption = Language.GetButtonCaption(button.ButtonName);
                barButton.Glyph = Language.GetButtonIcon24(button.ButtonName);

                // Set hotkey
                var hotkey = Language.GetButtonHotkey(button.ButtonName);
                if (!string.IsNullOrEmpty(hotkey))
                {
                    var shortcut = (Shortcut)Enum.Parse(typeof(Shortcut), hotkey);
                    barButton.ItemShortcut = new BarShortcut(shortcut);
                }

                // Set tooltip
                var tip = Language.GetButtonToolTip(button.ButtonName);
                if (!string.IsNullOrEmpty(tip))
                {
                    barButton.SuperTip = new SuperToolTip();
                    barButton.SuperTip.Items.Add(tip);
                }

                if (button.ShowOnToolbar == CODES.DEFMODBTN.ONTOOLBAR.YES)
                {
                    // Group button if exists
                    if (!string.IsNullOrEmpty(button.ButtonGroup))
                    {
                        // Was created?
                        var groupItem = (from BarItem item in mainBarManager.Items
                                         where item is BarLinkContainerItem && item.Name == button.ButtonGroup
                                         select item as BarLinkContainerItem).SingleOrDefault();
                        // No --> Create it
                        if (groupItem == null)
                        {
                            groupItem = new BarLinkContainerItem
                            {
                                Name = button.ButtonGroup,
                                PaintStyle = BarItemPaintStyle.CaptionGlyph
                            };
                            mainBarManager.Items.Add(groupItem);
                            mainToolbar.LinksPersistInfo.Add(new LinkPersistInfo(groupItem, button.BeginGroup == CODES.DEFMODBTN.BEGINGROUP.YES));
                            // groupItem
                            groupItem.Caption = Language.GetButtonCaption(groupItem.Name);
                            groupItem.Glyph = Language.GetButtonIcon24(groupItem.Name);
                        }
                        // Add Button to Group
                        barButton.Glyph = Language.GetButtonIcon16(button.ButtonName);
                        groupItem.LinksPersistInfo.Add(new LinkPersistInfo(barButton, button.BeginGroup == CODES.DEFMODBTN.BEGINGROUP.YES));
                    }
                    else
                    {
                        // Add Button to ToolBar
                        mainToolbar.LinksPersistInfo.Add(new LinkPersistInfo(barButton, button.BeginGroup == CODES.DEFMODBTN.BEGINGROUP.YES));
                    }
                }
            } 
            
            if (mainToolbar.LinksPersistInfo.Count == 0)
            {
                layoutToolBar.Visibility = LayoutVisibility.Never;
            }
            else
            {
                layoutToolBar.Visibility = LayoutVisibility.Always;
            }
#if DEBUG
            SetupContextMenu(mainLayout);
            SetupModuleEdit();
            SetupAddButton();
            SetupRefreshModule();
            SetupSeparator();
            SetupConditionFields();
            SetupColumnFields();
            SetupCommonFields();
            SetupSeparator();
            SetupFieldMaker();
            SetupFieldsSuggestion();
            SetupSeparator();
            SetupLanguageTool();
            SetupSaveLayout(mainLayout);
#endif
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            try
            {
                ModuleUtils.GetModuleInfo(ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.SEARCH_EXPORT);
            }
            catch
            {
                mainLayout.HideItem(mainLayout.GetItemByControl(btnExport));
            }

            if(string.IsNullOrEmpty(SearchInfo.EditStore))
            {
                gvMain.OptionsBehavior.Editable = false;
                mainLayout.HideItem(mainLayout.GetItemByControl(btnEdit));
            }
            else
            {
                gvMain.OptionsBehavior.Editable = true;
            }
        }

        public override void LockUserAction()
        {
            base.LockUserAction();

            if(!InvokeRequired)
            {
                ShowWaitingBox();
                if (ModuleInfo.Expanded == "Y")
                {
                    gpSearchCondition.Expanded = true;
                }
                else
                {
                    gpSearchCondition.Expanded = false;
                }
                gcMain.DataSource = null;
                Enabled = false;
            }
        }

        public override void  UnLockUserAction()
        {
            base.UnLockUserAction();

            if(!InvokeRequired)
            {
                HideWaitingBox();
                Enabled = true;
                if (BufferResult != null && BufferResult.Rows.Count > 0)
                {
                    btnExport.Enabled = true;
                }
            }
        }
        #endregion

        #region Field Utils
        //ORG: BaseEdit --> Control
        public override Control CreateControl(ModuleFieldInfo fieldInfo)
        {
            var edit = base.CreateControl(fieldInfo);
            base.SetControlListSource(edit);
            edit.Dock = DockStyle.Top;
            return edit;
        }

        public void ChangeStatusText(Image statusImage,string status, params object[] objs)
        {
            var strStatusText = Language.GetSpecialStatus(status);
            txtSearchStatus.Text = string.Format(strStatusText, objs);
            txtSearchStatus.TextVisible = true;

            btnCopyStatus.StyleController = null;
            btnCopyStatus.ButtonStyle = BorderStyles.NoBorder;
            btnCopyStatus.Image = statusImage;
            mainLayout.GetItemByControl(btnCopyStatus).ContentVisible = true;
        }

        #endregion

        #region DataGrid Utils
        private void BestFitColumns()
        {
            gcMain.SuspendLayout();
            gvMain.BestFitColumns();
          
            if (SearchInfo.FullWidth == CODES.MODSEARCH.FULLWIDTH.NO)
            {
                foreach (GridColumn column in gvMain.Columns)
                {
                    if(!column.OptionsColumn.FixedWidth)
                        column.Width += CONSTANTS.INCREASE_COLUMN_WIDTH;
                    column.AppearanceHeader.TextOptions.HAlignment= HorzAlignment.Center;
                }
            }
            gcMain.ResumeLayout();
        }
        #endregion

        #region Events
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private List<DataRowView> GetSelectedRows()
        {
            var selectedRows = new List<DataRowView>();

            if (CheckMarksUtils != null)
            {
                if (CheckMarksUtils.SelectedCount == 0)
                {
                    if (gvMain.FocusedRowHandle >= 0)
                    {
                        selectedRows.Add((DataRowView)gvMain.GetRow(gvMain.FocusedRowHandle));
                    }
                }
                else
                {
                    for (var i = 0; i < CheckMarksUtils.SelectedCount; i++)
                    {
                        selectedRows.Add((DataRowView)CheckMarksUtils.GetSelectedRow(i));
                    }
                }
            }
            else
            {
                if(gvMain.FocusedRowHandle >= 0)
                    selectedRows.Add((DataRowView)gvMain.GetRow(gvMain.FocusedRowHandle));
            }

            return selectedRows;
        }


        private void DoClick(ButtonInfo button)
        {
            //TUDQ them
            Program.treeModuleID = ModuleInfo.ModuleID;
            //
            var selectedRows = GetSelectedRows();

            if (button.CallModuleID == SEARCHMASTER.MODULE_RETURN_ID)
            {
                try
                {
                    string[] list;
                    if(string.IsNullOrEmpty(SearchInfo.ColumnText))
                    {
                        list = (from row in selectedRows
                                    select
                                        row[SearchInfo.ColumnValue] == null ?
                                            string.Empty : row[SearchInfo.ColumnValue].ToString()).ToArray();                        
                    }
                    else
                    {
                        list = (from row in selectedRows
                                    select
                                        (row[SearchInfo.ColumnText] == null ?
                                            string.Empty : row[SearchInfo.ColumnText].ToString()) +
                                        "<" +
                                        (row[SearchInfo.ColumnValue] == null ?
                                            string.Empty : row[SearchInfo.ColumnValue].ToString()) +
                                        ">").ToArray();                        
                    }

                    LookUpValues = string.Join(",", list);
                    CloseModule();
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
                return;
            }

            if (button.CallModuleID != null)
            {
                var buttonParams = ModuleUtils.GetSearchButtonParams(button);
                var isRowRequire = (from item in buttonParams
                                    where !string.IsNullOrEmpty(item.ColumnName)
                                    select 1).Count() != 0;

                if (!isRowRequire)
                {
                    if (ConfirmExecuteClick(button))
                    {
                        ExecuteClickWithoutRow(button, buttonParams);
                    }
                }
                else
                {
                    if (selectedRows.Count == 0)
                    {
                        throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SELECT_DATAROW_FIRST);
                    }

                    if (selectedRows.Count == 1)
                    {
                        if (ConfirmExecuteClick(button))
                        {
                            ExecuteClickWithSingleRow(button, buttonParams, selectedRows[0], true);                            
                                              
                        }
                    }
                    
                    if (selectedRows.Count > 1)
                    {
                        if (button.MultiExecute == CODES.DEFMODBTN.MULTIEXEC.NO)
                        {
                            throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SELECT_MULTIROWS_NOT_ALLOW);
                        }

                        if (ConfirmExecuteClick(button))
                        {
                            if(button.MultiExecute == CODES.DEFMODBTN.MULTIEXEC.MULTI_ROWS)
                            {
                                ExecuteClickWithMultiRow(button, buttonParams, selectedRows, true);
                            }

                            if (button.MultiExecute == CODES.DEFMODBTN.MULTIEXEC.YES)
                            {
                                foreach (var selectedRow in selectedRows)
                                {
                                    ExecuteClickWithSingleRow(button, buttonParams, selectedRow, selectedRow == selectedRows.Last());
                                }
                            }
                        }
                    }
                }
            }
        }
       
        private bool ConfirmExecuteClick(ButtonInfo button)
        {
            var confirmResult = true;
            if (button.ShowConfirm == CODES.DEFMODBTN.SHOWCONFIRM.YES)
            {
                var form = new frmConfirm
                {
                    ModuleInfo = ModuleUtils.GetModuleInfo(
                        button.CallModuleID,
                        button.CallSubModule)
                };
                form.ShowDialog(this);
                confirmResult = form.ConfirmResult;
            }

            return confirmResult;
        }

        private void ExecuteClickWithSingleRow(ButtonInfo button, List<ButtonParamInfo> buttonParams, DataRowView selectedRow, bool autoRefresh)
        {         
            var targetModule = MainProcess.CreateModuleInstance(button.CallModuleID, button.CallSubModule);
            if (targetModule != null)
            {
                foreach (var param in buttonParams)
                {
                    AssignValueParam(targetModule, param);
                    AssignConditionParam(targetModule, param);
                    AssignColumnParam(targetModule, param, selectedRow);
                }

                if(autoRefresh) targetModule.ModuleClosed += SubModule_Closed;
                targetModule.ShowModule(this);
                 
            }
        }

        private void ExecuteClickWithMultiRow(ButtonInfo button, List<ButtonParamInfo> buttonParams, List<DataRowView> selectedRows, bool autoRefresh)
        {
            var targetModule = MainProcess.CreateModuleInstance(button.CallModuleID, button.CallSubModule);
            if (targetModule != null)
            {
                foreach (var param in buttonParams)
                {
                    AssignValueParam(targetModule, param);
                    AssignConditionParam(targetModule, param);
                    
                    if(!string.IsNullOrEmpty(param.ColumnName))
                    {
                        var fieldInfos = FieldUtils.GetModuleFieldsByName(targetModule.ModuleInfo.ModuleID, param.FieldName);
                        foreach (var fieldInfo in fieldInfos)
                        {
                            var values = new string[selectedRows.Count];
                            for (var i = 0; i < values.Length; i++)
                            {
                                values[i] = selectedRows[i][param.ColumnName].Encode(fieldInfo);
                            }
                            if (fieldInfo.WrapText == CONSTANTS.No)
                            {
                                targetModule[fieldInfo.FieldID] = string.Join(",", values);
                            }
                            else
                            {                                                               
                                targetModule[fieldInfo.FieldID] = values[0] ;
                            }                            
                        }
                    }
                }

                if(autoRefresh) targetModule.ModuleClosed += SubModule_Closed;
                targetModule.ShowModule(this);
            }
        }

        private void ExecuteClickWithoutRow(ButtonInfo button, List<ButtonParamInfo> buttonParams)
        {
            var targetModule = MainProcess.CreateModuleInstance(button.CallModuleID, button.CallSubModule);
            if (targetModule != null)
            {
                foreach (var param in buttonParams)
                {
                    AssignConditionParam(targetModule, param);
                    AssignValueParam(targetModule, param);
                }

                targetModule.ModuleClosed += SubModule_Closed;
                targetModule.ShowModule(this);
            }
        }

        private static void AssignValueParam(ucModule targetModule, ButtonParamInfo param)
        {
            if(!string.IsNullOrEmpty(param.Value))
            {
                var targetFields = FieldUtils.GetModuleFieldsByName(targetModule.ModuleInfo.ModuleID, param.FieldName);
                foreach(var field in targetFields)
                {
                    targetModule[field.FieldID] = param.Value.Decode(field);
                }
            }
        }

        private void AssignConditionParam(ucModule targetModule, ButtonParamInfo param)
        {
            if (!string.IsNullOrEmpty(param.ConditionName))
            {
                var field = FieldUtils.GetModuleFieldsByName(
                    ModuleInfo.ModuleID,
                    CODES.DEFMODFLD.FLDGROUP.COMMON,
                    param.ConditionName)[0];
                targetModule.SetFieldValue(param.FieldName, this[field.FieldID]);
            }
        }

        private static void AssignColumnParam(ucModule targetModule, ButtonParamInfo param, DataRowView row)
        {
            if (!string.IsNullOrEmpty(param.ColumnName))
            {
                targetModule.SetFieldValue(param.FieldName, row[param.ColumnName]);
            }            
        }

        private void mainBarManager_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var button = e.Item.Tag as ButtonInfo;
#if DEBUG
                if(button != null)
                {
                    if(!MainProcess.Instance.IsAdvanceDeveloperMode)
                    {
                        DoClick(button);
                    }
                    else
                    if (button.CallModuleID == SEARCHMASTER.MODULE_RETURN_ID)
                    {
                        DoClick(button);
                    }
                    else
                    {
                        var actionContext = new ContextMenuStrip();
                        actionContext.Items.Add("Execute Button", ThemeUtils.Image16.Images["EXECUTE_STORE"]).Click +=
                            delegate
                            {
                                try
                                {
                                    DoClick(button);
                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                }
                            };
                        actionContext.Items.Add(new ToolStripSeparator());
                        actionContext.Items.Add("Edit Button", ThemeUtils.Image16.Images["EDIT"]).Click +=
                            delegate
                            {
                                try
                                {
                                    var ucModule = MainProcess.CreateModuleInstance("02909", "MED");
                                    ucModule["C01"] = ModuleInfo.ModuleID;
                                    ucModule["C02"] = button.ButtonID;
                                    ucModule.ShowModule(MainProcess.GetMainForm());
                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                }
                            };
                        actionContext.Items.Add("Button Parameter", ThemeUtils.Image16.Images["PARAMETER"]).Click +=
                            delegate
                            {
                                try
                                {
                                    var ucModule = MainProcess.CreateModuleInstance("03908", "MMN");
                                    ucModule["C01"] = ModuleInfo.ModuleID;
                                    ucModule["C02"] = button.ButtonName;
                                    ucModule.ShowModule(MainProcess.GetMainForm());
                                    ucModule.Execute();
                                }
                                catch (Exception ex)
                                {
                                    ShowError(ex);
                                }
                            };
                        actionContext.Show(MousePosition.X, MousePosition.Y);                        
                    }
                }
#else
                if(button != null)
                {
                    DoClick(button);
                }
#endif
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        void SubModule_Closed(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(SubModule_Closed), sender, e);
            }
            else
            {
                if (SearchInfo.AutoRefresh == CODES.MODSEARCH.AUTOREFRESH.YES)
                {
                    if (((ucModule)sender).RequireRefresh) Execute();
                }
            }
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem.Tag is ButtonInfo)
                {
                    var button = e.ClickedItem.Tag as ButtonInfo;
                    DoClick(button);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {                
                var exportInfo = ModuleUtils.GetModuleInfo(ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.SEARCH_EXPORT);
                var ucExport = (ucSearchExport)MainProcess.CreateModuleInstance(exportInfo.ModuleID, exportInfo.SubModule,"CALL_MODULE");

                var expInfo = SysvarUtils.GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_EXPORT);
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
                        Filter = IMPORTMASTER.IMPORT_FILE_EXTENSIONS
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
            catch (Exception ex)
            {
                ShowError(ex);
            }
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

        #endregion

        #region Search Methods

        public void DisposeLastSearchResult()
        {
            if (BufferResult != null)
            {
                BufferResult.Dispose();
                BufferResult = null;
            }
        }

        public void InitSearch()
        {
            try
            {
                btnExport.Enabled = false;
                cboPages.SelectedIndexChanged -= cboPages_SelectedIndexChanged;
                cboPages.ButtonClick -= cboPages_ButtonClick;

                cboPages.SuspendLayout();
                cboPages.Properties.Items.Clear();
                cboPages.ResumeLayout();
                cboPages.Refresh();
                ChangeStatusText(Resources.Search, "Searching");

                m_ErrorStatistics = null;
            }
            catch
            {
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

                if(SearchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.APPEND_FROM_READER)
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
                else if (SearchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.PAGE_FROM_READER)
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

                    cboPages.Properties.NullText = string.Format(Language.PageInfo, SelectedPage + 1, BufferResult.StartRow + 1, BufferResult.StartRow + BufferResult.Rows.Count);
                }
                else if(SearchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.ALL_FROM_DATASET)
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

                gcMain.DataSource = BufferResult;

                //add by TrungTT - 21.2.2014 - Expand or Collapse all groups of Grid
                if(SearchInfo.ExpandGroup == "Y")
                    gvMain.ExpandAllGroups();
                else
                    gvMain.CollapseAllGroups();
                //end TrungTT

                gvMain.FocusedRowHandle = LastFocusRow;
                gvMain.Focus();

                if (SearchInfo.AutoFitWidthColumns == CODES.MODSEARCH.AUTOFITWIDTH.YES)
                    BestFitColumns();


                if (CheckMarksUtils != null)
                {
                    CheckMarksUtils.ClearSelection();
                    gvMain.FocusedRowHandle = -1;

                    for (var i = 0; i < BufferResult.Rows.Count; i++)
                    {
                        if (BufferResult.Columns.Contains(CHECKMARK.FIELDID))
                            if (BufferResult.Rows[i][CHECKMARK.FIELDID].ToString() == CHECKMARK.FIELDVALUE)
                            {
                                CheckMarksUtils.SelectRow(i, true);
                            } 
                    }
                }

            }
            else
            {
                gcMain.DataSource = null;

                if (CheckMarksUtils != null)
                {
                    CheckMarksUtils.ClearSelection();
                    gvMain.FocusedRowHandle = -1;
                }
            }
        }

        public void UpdateStatistic()
        {
            if (string.IsNullOrEmpty(SearchInfo.StatisticQuery))
            {
                if (SearchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.PAGE_FROM_READER)
                {
                    ChangeStatusText(Resources.SearchResult, "Result", BufferResult.BufferSize);
                }
                else
                {
                    ChangeStatusText(Resources.SearchResult, "FullResult", BufferResult.BufferSize);
                }
            }
        }

        public void ExecuteSearch_ProcessComplete(object sender, EventArgs e)
        {
            if (BufferResult != null && BufferResult.IsNotEmpty)
            {
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        var exportInfo = ModuleUtils.GetModuleInfo(ModuleInfo.ModuleID, CODES.DEFMOD.SUBMOD.SEARCH_EXPORT);
                        ctrlSA.CheckRole(exportInfo);
                        btnExport.Enabled = true;
                    }
                }
                catch
                {
                }
            }
        }

        void FetchSearchStatistic_ProcessComplete(object sender, EventArgs e)
        {
            if (SearchStatistic != null)
                ChangeStatusText(Resources.SearchResult, "StatisticResult", SearchStatistic, 0);
            else if(!string.IsNullOrEmpty(m_ErrorStatistics))
                ChangeStatusText(Resources.ErrorResult, "ErrorResult");
            else
                ChangeStatusText(Resources.SearchResult, "EmptyResult");
        }
        #endregion

        #region Fetch Result Methods

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

        #endregion

        public ucSearchMaster()
        {
            InitializeComponent();
            mainBarManager.Images = ThemeUtils.Image24;
        }

        public void Execute(DataRow modifiedRow)
        {
            try
            {
                List<string> values;
                GetOracleParameterValues(out values, SearchInfo.EditStore, modifiedRow);
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ExecuteSearchEdit(ModuleInfo.ModuleID, ModuleInfo.SubModule, values);
                    modifiedRow.ClearErrors();
                    modifiedRow.AcceptChanges();
                }
            }
            catch (FaultException ex)
            {
                modifiedRow.RowError = string.Format("{0}\r\n{1}", ex.ToMessage(), ex.Reason);
            }
            catch (Exception cex)
            {
                var ex = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, cex.Message);
                modifiedRow.RowError = string.Format("{0}\r\n{1}", ex.ToMessage(), ex.Reason);
            }
        }

        public override void Execute()
        {
            if(ValidateModule())
            {
                DisposeLastSearchResult();
                InitSearch();

                var conditionInstance = RootGroup.GetConditionInstance();
                var staticConditionInstances = CommonFields.Select(
                    condition => new SearchConditionInstance
                                    {
                                        ConditionID = condition.FieldID,
                                        Value = this[condition.FieldID].Encode(condition)
                                    }).ToList();

                ExecuteSearch(conditionInstance, staticConditionInstances);

                checkFormatConditions();

                if (!string.IsNullOrEmpty(SearchInfo.StatisticQuery))
                {
                    ExecuteStatistics(conditionInstance, staticConditionInstances);
                }   
            }
        }

        public void ExecuteSearch(SearchConditionInstance conditionInstance, List<SearchConditionInstance> staticConditionInstances)
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
                                
                                ctrlSA.ExecuteSearch(out lastSearchResultKey, out lastSearchTime, ModuleInfo.ModuleID, ModuleInfo.SubModule, conditionInstance, staticConditionInstances);
                                BufferResult = new BufferedResultManager(ModuleInfo, lastSearchResultKey, lastSearchTime);
                                SelectedPage = 0;
                                ExecuteFetchResult();
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex);
                            UnLockUserAction();
                        }
                    }, this);

            CurrentThread.ProcessComplete += ExecuteSearch_ProcessComplete;
            CurrentThread.Start();
        }

        public void ExecuteStatistics(SearchConditionInstance conditionInstance, List<SearchConditionInstance> staticConditionInstances)
        {
            var getSearchStatisticThread = new WorkerThread(delegate
            {
                try
                {
                    SearchStatistic = null;

                    using (var ctrlSA = new SAController())
                    {
                        DataContainer container;
                        ctrlSA.GetSearchStatistic(out container, ModuleInfo.ModuleID, ModuleInfo.SubModule, conditionInstance, staticConditionInstances);

                        if (container != null && container.DataTable.Rows.Count == 1)
                        {
                            SearchStatistic = new RowFormattable(container.DataTable.Rows[0]);
                        }
                    }
                }
                catch (FaultException ex)
                {
                    m_ErrorStatistics = string.Format("<b>{0}</b>\r\n{1}", ex.ToMessage(), ex.Reason);
                }
                catch (Exception cex)
                {
                    var ex = ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, cex.Message);
                    m_ErrorStatistics = string.Format("<b>{0}</b>\r\n{1}", ex.ToMessage(), ex.Reason);
                }
            }, this);

            getSearchStatisticThread.ProcessComplete += FetchSearchStatistic_ProcessComplete;
            getSearchStatisticThread.Start();
        }

        public void ExecuteFetchResult()
        {
            var currentThread = new WorkerThread(
                delegate
                {
                    LockUserAction();
                    try
                    {
                        if (SearchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.ALL_FROM_DATASET)
                        {
                            BufferResult.GetFullBuffer();
                        }
                        else if(SearchInfo.PageMode == CODES.MODSEARCH.PAGEMODE.APPEND_FROM_READER)
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

            currentThread.ProcessComplete += ExecuteFetchResult_ProcessComplete;
            currentThread.Start();
        }

        private void btnCalcColumnSize_Click(object sender, EventArgs e)
        {
            BestFitColumns();
        }

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

        private void gvMain_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            if(e.HitInfo.InRow &&  e.MenuType == GridMenuType.Row)
            {
                var selectedRows = GetSelectedRows();
                //trungtt - 5.12.2013 - View context menu for Role
                //var buttons = ModuleUtils.GetSearchButtons(ModuleInfo.ModuleID);
                List<ButtonInfo> buttons = new List<ButtonInfo>();
                ButtonInfo buttonInfo;
                DataContainer container;
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        List<string> values = new List<string>();
                        values.Add(ModuleInfo.ModuleID);

                        ctrlSA.ExecuteProcedureFillDataset(out container, "sp_list_button", values);
                        foreach (DataRow rows in container.DataSet.Tables[0].Rows)
                        {
                            buttonInfo = new ButtonInfo();
                            buttonInfo.ModuleID = rows[0].ToString();
                            buttonInfo.ButtonID = rows[1].ToString();
                            buttonInfo.ButtonName = rows[2].ToString();
                            buttonInfo.ButtonGroup = rows[3].ToString();
                            buttonInfo.BeginGroup = rows[4].ToString();
                            buttonInfo.CallModuleID = rows[5].ToString();
                            buttonInfo.CallSubModule = rows[6].ToString();
                            buttonInfo.ShowConfirm = rows[7].ToString();
                            buttonInfo.MultiExecute = rows[8].ToString();
                            buttonInfo.ShowOnToolbar = rows[9].ToString();
                            buttonInfo.ParameterMode = rows[10].ToString();
                            buttonInfo.DBClick = rows[11].ToString();

                            buttons.Add(buttonInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                //end trungtt
                var canCreateSeparator = false;

                contextMenu.Items.Clear();
                foreach (var button in buttons)
                {
                    var btnParams = ModuleUtils.GetSearchButtonParams(button);

                    if (btnParams.Count != 0 &&
                        (selectedRows.Count == 1 || (selectedRows.Count > 1 && button.MultiExecute == CODES.DEFMODBTN.MULTIEXEC.YES)))
                    {
                        if (button.BeginGroup == CODES.DEFMODBTN.BEGINGROUP.YES && canCreateSeparator)
                        {
                            contextMenu.Items.Add(new ToolStripSeparator());
                        }

                        // Context Item
                        var item = contextMenu.Items.Add(Language.GetButtonCaption(button.ButtonName));
                        item.Tag = button;
                        item.Image = Language.GetButtonIcon16(button.ButtonName);
                        canCreateSeparator = true;
                    }
                }

                e.Allow = (contextMenu.Items.Count != 0);
            }
            else
            {
                contextMenu.Visible = false;
                contextMenu.Items.Clear();
                e.Allow = true;
            }
        }

        private void gvMain_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (CheckMarksUtils != null && e.Column == CheckMarksUtils.CheckMarkColumn) return;
            if(!string.IsNullOrEmpty(SearchInfo.EditStore))
            {
                btnEdit.Enabled = true;
            }
        }

        private void gvMain_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowView = gvMain.GetRow(e.RowHandle) as DataRowView;

            if(rowView != null)
            {
                rowView.EndEdit();
                if ((rowView.Row.RowState & DataRowState.Modified) > 0)
                {
                    e.Info.State = DevExpress.Utils.Drawing.ObjectState.Pressed;
                }
                else
                {
                    e.Info.State = DevExpress.Utils.Drawing.ObjectState.Normal;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string strMess = "Bạn có chắc chắn muốn cập nhật thông tin này ?";
                string strTitle = "Cập nhật thông tin";
                switch(ModuleInfo.ModuleID)
                {
                    case "03276":
                        strMess = "Bạn có chắc chắn muốn cập nhật thông tin này ?";
                        strTitle= "Đánh giá thông tin";
                        break;
                    case "03262":
                        strMess = "Bạn có chắc chắn muốn phê duyệt báo cáo này ?";
                        strTitle = "Phê duyệt thông tin";
                        break;
                }

                if (MessageBox.Show(strMess, strTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (DataRow modifiedRow in BufferResult.Rows)
                    {
                        if ((modifiedRow.RowState & DataRowState.Modified) > 0)
                        {
                            Execute(modifiedRow);
                            gvMain.RefreshData();
                        }
                    }

                    var modifiedRows = BufferResult.GetChanges(DataRowState.Modified);
                    if (modifiedRows == null || modifiedRows.Rows.Count == 0)
                    {
                        btnEdit.Enabled = false;
                    }
                    Execute();
                }                                               
            }
            catch
            {
                
            }
        }

        private void gvMain_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            var row = gvMain.GetDataRow(e.RowHandle);
            if (row != null && (row.RowState & DataRowState.Modified) > 0)
            {
                e.Appearance.BackColor = ThemeUtils.EditRowColor;
            }
        }

        private void btnCopyStatus_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtSearchStatus.Text);
        }

        private void gvMain_RowClick(object sender, RowClickEventArgs e)
        {
            if(e.Clicks >= 2)
            {
                try
                {
                    DoClick(m_DefaultSearchButton);                    
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }
            }
        }

        private void gvMain_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {   
                var selectedRows = GetSelectedRows();
                string columnName = SearchInfo.DetailColumn;
                if (SearchInfo.DetailColumn != null && selectedRows.Count >= 0)
                {
                    if (selectedRows[0].Row["ORIENTION"].ToString() == "L")
                    {
                        richEdit.Document.HtmlText = selectedRows[0].Row[columnName].ToString();
                        richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                        richEdit.Document.Sections[0].Page.Landscape = true;
                    }
                    else
                    {
                        richEdit.Document.HtmlText = selectedRows[0].Row[columnName].ToString();
                        richEdit.Document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
                    }
                }
            }
        }

        private void ucSearchMaster_Load(object sender, EventArgs e)
        {
            Execute();
        }

        private void gcMain_DoubleClick(object sender, EventArgs e)
        {
            if (SearchInfo.Double_Click == "Y")
            {
                foreach (LinkPersistInfo item in mainToolbar.LinksPersistInfo)
                {
                    var button = item.Item.Tag as ButtonInfo;
                    try
                    {
                        if (button != null)
                        {
                            if (button.DBClick == CONSTANTS.Yes) { DoClick(button); }                    
                        }                        
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex);
                    }                    
                }
            }
        }
        
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
                        DevExpress.XtraGrid.StyleFormatCondition condition1 = new DevExpress.XtraGrid.StyleFormatCondition();                        
                        if (Convert.ToString(dt.Rows[i]["BACKCOLOR"]) != "")
                            condition1.Appearance.BackColor = Color.FromName(dt.Rows[i]["BACKCOLOR"].ToString());
                        else condition1.Appearance.BackColor = Color.Transparent;
                        if (Convert.ToString(dt.Rows[i]["FORECOLOR"]) != "")
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
                        condition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                        condition1.Expression = dt.Rows[i]["CONDITIONS"].ToString();
                        condition1.Appearance.Options.UseFont = true;
                        gvMain.FormatConditions.Add(condition1);
                    }
                }
            }
        }
    }
}
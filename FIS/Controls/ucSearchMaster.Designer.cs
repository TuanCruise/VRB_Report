namespace FIS.AppClient.Controls
{
    partial class ucSearchMaster
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (BufferResult != null) BufferResult.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSearchMaster));
            this.mainBarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.mainToolbar = new DevExpress.XtraBars.Bar();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.btnCopyStatus = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.btnCalcColumnSize = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtQuery = new DevExpress.XtraEditors.TextEdit();
            this.searchConditionLayout = new DevExpress.XtraLayout.LayoutControl();
            this.rootConditionGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.cboPages = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.cboPageItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.txtSearchStatus = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutToolBar = new DevExpress.XtraLayout.LayoutControlItem();
            this.gpSearchCondition = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.txtQueryItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutBtnEdit = new DevExpress.XtraLayout.LayoutControlItem();
            this.btnCopyStatusItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchConditionLayout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootConditionGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPages.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPageItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutToolBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpSearchCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutBtnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCopyStatusItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainBarManager
            // 
            this.mainBarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainToolbar});
            this.mainBarManager.DockControls.Add(this.barDockControlTop);
            this.mainBarManager.DockControls.Add(this.barDockControlBottom);
            this.mainBarManager.DockControls.Add(this.barDockControlLeft);
            this.mainBarManager.DockControls.Add(this.barDockControlRight);
            this.mainBarManager.DockControls.Add(this.standaloneBarDockControl1);
            this.mainBarManager.Form = this;
            this.mainBarManager.MaxItemId = 3;
            this.mainBarManager.MdiMenuMergeStyle = DevExpress.XtraBars.BarMdiMenuMergeStyle.WhenChildActivated;
            this.mainBarManager.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.mainBarManager_ItemClick);
            // 
            // mainToolbar
            // 
            this.mainToolbar.BarName = "Tools";
            this.mainToolbar.DockCol = 0;
            this.mainToolbar.DockRow = 0;
            this.mainToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.mainToolbar.FloatLocation = new System.Drawing.Point(361, 160);
            this.mainToolbar.FloatSize = new System.Drawing.Size(46, 24);
            this.mainToolbar.OptionsBar.UseWholeRow = true;
            this.mainToolbar.StandaloneBarDockControl = this.standaloneBarDockControl1;
            resources.ApplyResources(this.mainToolbar, "mainToolbar");
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.AutoSizeInLayoutControl = false;
            this.standaloneBarDockControl1.CausesValidation = false;
            resources.ApplyResources(this.standaloneBarDockControl1, "standaloneBarDockControl1");
            this.standaloneBarDockControl1.Manager = this.mainBarManager;
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
            this.barDockControlTop.Manager = this.mainBarManager;
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
            this.barDockControlBottom.Manager = this.mainBarManager;
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            resources.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
            this.barDockControlLeft.Manager = this.mainBarManager;
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            resources.ApplyResources(this.barDockControlRight, "barDockControlRight");
            this.barDockControlRight.Manager = this.mainBarManager;
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.StyleController = this.mainLayout;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // mainLayout
            // 
            this.mainLayout.AllowCustomization = false;
            this.mainLayout.Controls.Add(this.btnCopyStatus);
            this.mainLayout.Controls.Add(this.btnEdit);
            this.mainLayout.Controls.Add(this.btnCalcColumnSize);
            this.mainLayout.Controls.Add(this.standaloneBarDockControl1);
            this.mainLayout.Controls.Add(this.btnExport);
            this.mainLayout.Controls.Add(this.gcMain);
            this.mainLayout.Controls.Add(this.btnSearch);
            this.mainLayout.Controls.Add(this.txtQuery);
            this.mainLayout.Controls.Add(this.searchConditionLayout);
            this.mainLayout.Controls.Add(this.cboPages);
            resources.ApplyResources(this.mainLayout, "mainLayout");
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(769, 413, 450, 350);
            this.mainLayout.OptionsCustomizationForm.ShowLoadButton = false;
            this.mainLayout.OptionsCustomizationForm.ShowPropertyGrid = true;
            this.mainLayout.OptionsCustomizationForm.ShowSaveButton = false;
            this.mainLayout.OptionsSerialization.RestoreAppearanceItemCaption = true;
            this.mainLayout.OptionsSerialization.RestoreGroupPadding = true;
            this.mainLayout.OptionsSerialization.RestoreLayoutItemPadding = true;
            this.mainLayout.OptionsView.AllowItemSkinning = false;
            this.mainLayout.Root = this.layoutControlGroup1;
            // 
            // btnCopyStatus
            // 
            this.btnCopyStatus.AllowFocus = false;
            this.btnCopyStatus.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            resources.ApplyResources(this.btnCopyStatus, "btnCopyStatus");
            this.btnCopyStatus.Name = "btnCopyStatus";
            this.btnCopyStatus.StyleController = this.mainLayout;
            this.btnCopyStatus.TabStop = false;
            this.btnCopyStatus.Click += new System.EventHandler(this.btnCopyStatus_Click);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.StyleController = this.mainLayout;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnCalcColumnSize
            // 
            this.btnCalcColumnSize.ImageOptions.Image = global::FIS.AppClient.Properties.Resources.AutoSize;
            this.btnCalcColumnSize.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            resources.ApplyResources(this.btnCalcColumnSize, "btnCalcColumnSize");
            this.btnCalcColumnSize.Name = "btnCalcColumnSize";
            this.btnCalcColumnSize.StyleController = this.mainLayout;
            this.btnCalcColumnSize.TabStop = false;
            this.btnCalcColumnSize.Click += new System.EventHandler(this.btnCalcColumnSize_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.StyleController = this.mainLayout;
            this.btnExport.TabStop = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // gcMain
            // 
            resources.ApplyResources(this.gcMain, "gcMain");
            this.gcMain.ContextMenuStrip = this.contextMenu;
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.mainBarManager;
            this.gcMain.Name = "gcMain";
            this.gcMain.TabStop = false;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            this.gcMain.DoubleClick += new System.EventHandler(this.gcMain_DoubleClick);
            this.gcMain.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.gcMain_PreviewKeyDown);
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenu, "contextMenu");
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenu_ItemClicked);
            // 
            // gvMain
            // 
            this.gvMain.GridControl = this.gcMain;
            resources.ApplyResources(this.gvMain, "gvMain");
            this.gvMain.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.AutoPopulateColumns = false;
            this.gvMain.OptionsCustomization.AllowFilter = false;
            this.gvMain.OptionsNavigation.EnterMoveNextColumn = true;
            this.gvMain.OptionsPrint.AutoWidth = false;
            this.gvMain.OptionsPrint.ExpandAllGroups = false;
            this.gvMain.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.EnableAppearanceEvenRow = true;
            this.gvMain.OptionsView.EnableAppearanceOddRow = true;
            this.gvMain.OptionsView.ShowAutoFilterRow = true;
            this.gvMain.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowOnlyInEditor;
            this.gvMain.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvMain.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvMain_CustomDrawRowIndicator);
            this.gvMain.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvMain_CustomDrawCell);
            this.gvMain.ShowGridMenu += new DevExpress.XtraGrid.Views.Grid.GridMenuEventHandler(this.gvMain_ShowGridMenu);
            this.gvMain.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvMain_FocusedRowChanged);
            this.gvMain.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvMain_CellValueChanged);
            // 
            // txtQuery
            // 
            resources.ApplyResources(this.txtQuery, "txtQuery");
            this.txtQuery.MenuManager = this.mainBarManager;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Properties.AllowFocused = false;
            this.txtQuery.Properties.ReadOnly = true;
            this.txtQuery.StyleController = this.mainLayout;
            this.txtQuery.TabStop = false;
            // 
            // searchConditionLayout
            // 
            this.searchConditionLayout.AllowCustomization = false;
            resources.ApplyResources(this.searchConditionLayout, "searchConditionLayout");
            this.searchConditionLayout.Name = "searchConditionLayout";
            this.searchConditionLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(52, 206, 250, 350);
            this.searchConditionLayout.Root = this.rootConditionGroup;
            // 
            // rootConditionGroup
            // 
            resources.ApplyResources(this.rootConditionGroup, "rootConditionGroup");
            this.rootConditionGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.rootConditionGroup.GroupBordersVisible = false;
            this.rootConditionGroup.Name = "Root";
            this.rootConditionGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.rootConditionGroup.Size = new System.Drawing.Size(819, 159);
            this.rootConditionGroup.TextVisible = false;
            // 
            // cboPages
            // 
            resources.ApplyResources(this.cboPages, "cboPages");
            this.cboPages.MenuManager = this.mainBarManager;
            this.cboPages.Name = "cboPages";
            this.cboPages.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.cboPages.Properties.Appearance.Options.UseBackColor = true;
            this.cboPages.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("cboPages.Properties.Buttons"))))});
            this.cboPages.StyleController = this.mainLayout;
            // 
            // layoutControlGroup1
            // 
            resources.ApplyResources(this.layoutControlGroup1, "layoutControlGroup1");
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.cboPageItem,
            this.layoutControlItem2,
            this.txtSearchStatus,
            this.layoutControlItem6,
            this.layoutToolBar,
            this.gpSearchCondition,
            this.layoutControlItem3,
            this.txtQueryItem,
            this.layoutControlItem4,
            this.layoutBtnEdit,
            this.btnCopyStatusItem,
            this.emptySpaceItem1});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(829, 494);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // cboPageItem
            // 
            this.cboPageItem.AppearanceItemCaption.Options.UseTextOptions = true;
            this.cboPageItem.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cboPageItem.Control = this.cboPages;
            resources.ApplyResources(this.cboPageItem, "cboPageItem");
            this.cboPageItem.ImageOptions.Image = global::FIS.AppClient.Properties.Resources.PAGE;
            this.cboPageItem.Location = new System.Drawing.Point(199, 468);
            this.cboPageItem.MaxSize = new System.Drawing.Size(600, 24);
            this.cboPageItem.MinSize = new System.Drawing.Size(600, 24);
            this.cboPageItem.Name = "cboPageItem";
            this.cboPageItem.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2);
            this.cboPageItem.Size = new System.Drawing.Size(600, 26);
            this.cboPageItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.cboPageItem.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.cboPageItem.TextSize = new System.Drawing.Size(0, 0);
            this.cboPageItem.TextToControlDistance = 20;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnSearch;
            resources.ApplyResources(this.layoutControlItem2, "layoutControlItem2");
            this.layoutControlItem2.FillControlToClientArea = false;
            this.layoutControlItem2.Location = new System.Drawing.Point(744, 223);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(85, 26);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(85, 26);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 5, 2, 2);
            this.layoutControlItem2.Size = new System.Drawing.Size(85, 26);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem2.TrimClientAreaToControl = false;
            // 
            // txtSearchStatus
            // 
            this.txtSearchStatus.AllowHotTrack = false;
            this.txtSearchStatus.AllowHtmlStringInCaption = true;
            this.txtSearchStatus.AppearanceItemCaption.Font = ((System.Drawing.Font)(resources.GetObject("txtSearchStatus.AppearanceItemCaption.Font")));
            this.txtSearchStatus.AppearanceItemCaption.Options.UseFont = true;
            resources.ApplyResources(this.txtSearchStatus, "txtSearchStatus");
            this.txtSearchStatus.Location = new System.Drawing.Point(34, 223);
            this.txtSearchStatus.Name = "txtSearchStatus";
            this.txtSearchStatus.Size = new System.Drawing.Size(266, 26);
            this.txtSearchStatus.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnExport;
            resources.ApplyResources(this.layoutControlItem6, "layoutControlItem6");
            this.layoutControlItem6.Location = new System.Drawing.Point(580, 223);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(82, 26);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(82, 26);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(82, 26);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutToolBar
            // 
            this.layoutToolBar.Control = this.standaloneBarDockControl1;
            resources.ApplyResources(this.layoutToolBar, "layoutToolBar");
            this.layoutToolBar.Location = new System.Drawing.Point(0, 0);
            this.layoutToolBar.MaxSize = new System.Drawing.Size(0, 32);
            this.layoutToolBar.MinSize = new System.Drawing.Size(1, 32);
            this.layoutToolBar.Name = "layoutToolBar";
            this.layoutToolBar.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutToolBar.Size = new System.Drawing.Size(829, 32);
            this.layoutToolBar.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutToolBar.TextSize = new System.Drawing.Size(0, 0);
            this.layoutToolBar.TextVisible = false;
            // 
            // gpSearchCondition
            // 
            resources.ApplyResources(this.gpSearchCondition, "gpSearchCondition");
            this.gpSearchCondition.ExpandButtonVisible = true;
            this.gpSearchCondition.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.gpSearchCondition.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.gpSearchCondition.Location = new System.Drawing.Point(0, 32);
            this.gpSearchCondition.Name = "gpSearchCondition";
            this.gpSearchCondition.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.gpSearchCondition.Size = new System.Drawing.Size(829, 191);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.searchConditionLayout;
            resources.ApplyResources(this.layoutControlItem1, "layoutControlItem1");
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(0, 163);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(104, 163);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(823, 163);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.gcMain;
            resources.ApplyResources(this.layoutControlItem3, "layoutControlItem3");
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 249);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(829, 219);
            this.layoutControlItem3.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // txtQueryItem
            // 
            this.txtQueryItem.Control = this.txtQuery;
            resources.ApplyResources(this.txtQueryItem, "txtQueryItem");
            this.txtQueryItem.ImageOptions.Image = global::FIS.AppClient.Properties.Resources.FILTER;
            this.txtQueryItem.Location = new System.Drawing.Point(0, 468);
            this.txtQueryItem.Name = "txtQueryItem";
            this.txtQueryItem.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2);
            this.txtQueryItem.Size = new System.Drawing.Size(199, 26);
            this.txtQueryItem.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.txtQueryItem.TextSize = new System.Drawing.Size(0, 0);
            this.txtQueryItem.TextToControlDistance = 20;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnCalcColumnSize;
            resources.ApplyResources(this.layoutControlItem4, "layoutControlItem4");
            this.layoutControlItem4.Location = new System.Drawing.Point(799, 468);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(30, 26);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(30, 26);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(30, 26);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutBtnEdit
            // 
            this.layoutBtnEdit.Control = this.btnEdit;
            resources.ApplyResources(this.layoutBtnEdit, "layoutBtnEdit");
            this.layoutBtnEdit.Location = new System.Drawing.Point(662, 223);
            this.layoutBtnEdit.MaxSize = new System.Drawing.Size(82, 26);
            this.layoutBtnEdit.MinSize = new System.Drawing.Size(82, 26);
            this.layoutBtnEdit.Name = "layoutBtnEdit";
            this.layoutBtnEdit.Size = new System.Drawing.Size(82, 26);
            this.layoutBtnEdit.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutBtnEdit.TextSize = new System.Drawing.Size(0, 0);
            this.layoutBtnEdit.TextVisible = false;
            // 
            // btnCopyStatusItem
            // 
            this.btnCopyStatusItem.AllowHide = false;
            resources.ApplyResources(this.btnCopyStatusItem, "btnCopyStatusItem");
            this.btnCopyStatusItem.Control = this.btnCopyStatus;
            this.btnCopyStatusItem.Location = new System.Drawing.Point(0, 223);
            this.btnCopyStatusItem.MaxSize = new System.Drawing.Size(34, 26);
            this.btnCopyStatusItem.MinSize = new System.Drawing.Size(34, 26);
            this.btnCopyStatusItem.Name = "btnCopyStatusItem";
            this.btnCopyStatusItem.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 2, 2, 2);
            this.btnCopyStatusItem.Size = new System.Drawing.Size(34, 26);
            this.btnCopyStatusItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.btnCopyStatusItem.TextSize = new System.Drawing.Size(0, 0);
            this.btnCopyStatusItem.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            resources.ApplyResources(this.emptySpaceItem1, "emptySpaceItem1");
            this.emptySpaceItem1.Location = new System.Drawing.Point(300, 223);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(280, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ucSearchMaster
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "ucSearchMaster";
            this.Load += new System.EventHandler(this.ucSearchMaster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQuery.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchConditionLayout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootConditionGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPages.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPageItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutToolBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpSearchCondition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutBtnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCopyStatusItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager mainBarManager;
        private DevExpress.XtraBars.Bar mainToolbar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private DevExpress.XtraEditors.ImageComboBoxEdit cboPages;
        private DevExpress.XtraLayout.LayoutControlItem cboPageItem;
        private DevExpress.XtraLayout.EmptySpaceItem txtSearchStatus;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutToolBar;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        internal DevExpress.XtraLayout.LayoutControl searchConditionLayout;
        internal DevExpress.XtraLayout.LayoutControlGroup rootConditionGroup;
        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraLayout.LayoutControlGroup gpSearchCondition;
        private DevExpress.XtraEditors.TextEdit txtQuery;
        private DevExpress.XtraLayout.LayoutControlItem txtQueryItem;
        private DevExpress.XtraEditors.SimpleButton btnCalcColumnSize;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btnEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutBtnEdit;
        private DevExpress.XtraEditors.SimpleButton btnCopyStatus;
        private DevExpress.XtraLayout.LayoutControlItem btnCopyStatusItem;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}

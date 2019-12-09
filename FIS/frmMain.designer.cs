namespace FIS.AppClient
{
    partial class frmMain
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mainStatusBar = new DevExpress.XtraBars.Bar();
            this.txtModuleID = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.mainBar = new DevExpress.XtraBars.BarManager(this.components);
            this.mainMenu = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.hideContainerRight = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.JobQueue = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemPopupContainerEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.tabMain = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.hideContainerRight.SuspendLayout();
            this.JobQueue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.SuspendLayout();
            // 
            // mainStatusBar
            // 
            this.mainStatusBar.BarName = "Status bar";
            this.mainStatusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.mainStatusBar.DockCol = 0;
            this.mainStatusBar.DockRow = 0;
            this.mainStatusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.mainStatusBar.OptionsBar.AllowQuickCustomization = false;
            this.mainStatusBar.OptionsBar.DisableCustomization = true;
            this.mainStatusBar.OptionsBar.DrawDragBorder = false;
            this.mainStatusBar.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.mainStatusBar, "mainStatusBar");
            // 
            // txtModuleID
            // 
            this.txtModuleID.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.txtModuleID.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtModuleID.Edit = this.repositoryItemTextEdit2;
            this.txtModuleID.Glyph = global::FIS.AppClient.Properties.Resources.ICON_MODULE;
            this.txtModuleID.Id = 11;
            this.txtModuleID.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.txtModuleID.Name = "txtModuleID";
            this.txtModuleID.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            resources.ApplyResources(this.txtModuleID, "txtModuleID");
            this.txtModuleID.EditValueChanged += new System.EventHandler(this.txtModuleID_EditValueChanged);
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.Appearance.Options.UseTextOptions = true;
            this.repositoryItemTextEdit2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.repositoryItemTextEdit2.AppearanceFocused.Options.UseTextOptions = true;
            this.repositoryItemTextEdit2.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            resources.ApplyResources(this.repositoryItemTextEdit2, "repositoryItemTextEdit2");
            this.repositoryItemTextEdit2.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // mainBar
            // 
            this.mainBar.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainMenu,
            this.mainStatusBar});
            this.mainBar.DockControls.Add(this.barDockControlTop);
            this.mainBar.DockControls.Add(this.barDockControlBottom);
            this.mainBar.DockControls.Add(this.barDockControlLeft);
            this.mainBar.DockControls.Add(this.barDockControlRight);
            this.mainBar.DockManager = this.dockManager;
            this.mainBar.Form = this;
            this.mainBar.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.txtModuleID});
            this.mainBar.MainMenu = this.mainMenu;
            this.mainBar.MaxItemId = 26;
            this.mainBar.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemPopupContainerEdit1});
            this.mainBar.StatusBar = this.mainStatusBar;
            this.mainBar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.mainBar_ItemClick);
            // 
            // mainMenu
            // 
            this.mainMenu.BarName = "Main menu";
            this.mainMenu.DockCol = 0;
            this.mainMenu.DockRow = 0;
            this.mainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.mainMenu.OptionsBar.MultiLine = true;
            this.mainMenu.OptionsBar.UseWholeRow = true;
            resources.ApplyResources(this.mainMenu, "mainMenu");
            // 
            // barDockControlTop
            // 
            resources.ApplyResources(this.barDockControlTop, "barDockControlTop");
            // 
            // barDockControlBottom
            // 
            resources.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
            // 
            // barDockControlLeft
            // 
            resources.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
            // 
            // barDockControlRight
            // 
            resources.ApplyResources(this.barDockControlRight, "barDockControlRight");
            // 
            // dockManager
            // 
            this.dockManager.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerRight});
            this.dockManager.Form = this;
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            this.dockManager.ValidateFloatFormChildrenOnDeactivate = false;
            // 
            // hideContainerRight
            // 
            this.hideContainerRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
            this.hideContainerRight.Controls.Add(this.JobQueue);
            resources.ApplyResources(this.hideContainerRight, "hideContainerRight");
            this.hideContainerRight.Name = "hideContainerRight";
            // 
            // JobQueue
            // 
            this.JobQueue.Controls.Add(this.dockPanel1_Container);
            this.JobQueue.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.JobQueue.ID = new System.Guid("c6722686-1d9b-4918-9520-f2bb1733554a");
            resources.ApplyResources(this.JobQueue, "JobQueue");
            this.JobQueue.Name = "JobQueue";
            this.JobQueue.Options.ShowCloseButton = false;
            this.JobQueue.OriginalSize = new System.Drawing.Size(318, 516);
            this.JobQueue.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.JobQueue.SavedIndex = 0;
            this.JobQueue.TabStop = false;
            this.JobQueue.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel1_Container
            // 
            resources.ApplyResources(this.dockPanel1_Container, "dockPanel1_Container");
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            // 
            // repositoryItemTextEdit1
            // 
            resources.ApplyResources(this.repositoryItemTextEdit1, "repositoryItemTextEdit1");
            this.repositoryItemTextEdit1.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemPopupContainerEdit1
            // 
            resources.ApplyResources(this.repositoryItemPopupContainerEdit1, "repositoryItemPopupContainerEdit1");
            this.repositoryItemPopupContainerEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemPopupContainerEdit1.Buttons"))))});
            this.repositoryItemPopupContainerEdit1.Name = "repositoryItemPopupContainerEdit1";
            // 
            // tabMain
            // 
            resources.ApplyResources(this.tabMain, "tabMain");
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabMain_SelectedPageChanged);
            this.tabMain.CloseButtonClick += new System.EventHandler(this.tabMain_CloseButtonClick);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.hideContainerRight);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.hideContainerRight.ResumeLayout(false);
            this.JobQueue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar mainMenu;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        internal DevExpress.XtraTab.XtraTabControl tabMain;
        internal DevExpress.XtraBars.Bar mainStatusBar;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem txtModuleID;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerRight;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupContainerEdit1;
        internal DevExpress.XtraBars.Docking.DockPanel JobQueue;
        private DevExpress.XtraBars.BarManager mainBar;
    }
}
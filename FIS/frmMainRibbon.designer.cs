namespace FIS.AppClient
{
    partial class frmMainRibbon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainRibbon));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.imageCollection3 = new DevExpress.Utils.ImageCollection(this.components);
            this.txtModuleID = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.mainDocumentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.imageCollection2 = new DevExpress.Utils.ImageCollection(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.applicationMenu1 = new DevExpress.XtraBars.Ribbon.ApplicationMenu(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainDocumentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.AllowTrimPageText = false;
            this.ribbon.ApplicationButtonText = null;
            this.ribbon.ApplicationIcon = global::FIS.AppClient.Properties.Resources.vrb_logo_final;
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Images = this.imageCollection3;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.txtModuleID,
            this.barSubItem2,
            this.barButtonItem1,
            this.barStaticItem1,
            this.barStaticItem2,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barStaticItem3,
            this.barStaticItem4,
            this.barButtonItem6});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ribbon.MaxItemId = 31;
            this.ribbon.Name = "ribbon";
            this.ribbon.PageHeaderItemLinks.Add(this.txtModuleID);
            this.ribbon.PageHeaderItemLinks.Add(this.barSubItem2);
            this.ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemMemoEdit1,
            this.repositoryItemButtonEdit1,
            this.repositoryItemDateEdit1});
            this.ribbon.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.True;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(515, 52);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            this.ribbon.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
            this.ribbon.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ribbon_ItemClick);
            this.ribbon.ApplicationButtonClick += new System.EventHandler(this.ribbon_ApplicationButtonClick);
            this.ribbon.ShowCustomizationMenu += new DevExpress.XtraBars.Ribbon.RibbonCustomizationMenuEventHandler(this.ribbon_ShowCustomizationMenu);
            this.ribbon.Click += new System.EventHandler(this.ribbon_Click);
            // 
            // imageCollection3
            // 
            this.imageCollection3.ImageSize = new System.Drawing.Size(32, 32);
            this.imageCollection3.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection3.ImageStream")));
            this.imageCollection3.Images.SetKeyName(0, "exit32.png");
            this.imageCollection3.Images.SetKeyName(1, "logout32.png");
            this.imageCollection3.Images.SetKeyName(2, "user-password32.png");
            this.imageCollection3.Images.SetKeyName(3, "market32.png");
            this.imageCollection3.Images.SetKeyName(4, "time32.png");
            this.imageCollection3.Images.SetKeyName(5, "contact32.png");
            this.imageCollection3.Images.SetKeyName(6, "login.png");
            this.imageCollection3.Images.SetKeyName(7, "ABOUTUS.png");
            this.imageCollection3.Images.SetKeyName(8, "mail_new.png");
            this.imageCollection3.Images.SetKeyName(9, "PB7icon.gif");
            // 
            // txtModuleID
            // 
            this.txtModuleID.Edit = this.repositoryItemTextEdit1;
            this.txtModuleID.Id = 2;
            this.txtModuleID.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
            this.txtModuleID.Name = "txtModuleID";
            this.txtModuleID.Width = 180;
            this.txtModuleID.EditValueChanged += new System.EventHandler(this.txtModuleID_EditValueChanged);
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.Appearance.Options.UseTextOptions = true;
            this.repositoryItemTextEdit1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.repositoryItemTextEdit1.AppearanceFocused.Options.UseTextOptions = true;
            this.repositoryItemTextEdit1.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.ContextImage = global::FIS.AppClient.Properties.Resources.idea_256;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.repositoryItemTextEdit1.NullText = "Tell me what you want to do ..";
            this.repositoryItemTextEdit1.NullValuePromptShowForEmptyValue = true;
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Giao diện";
            this.barSubItem2.Hint = "Giao diện";
            this.barSubItem2.Id = 13;
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = 19;
            this.barButtonItem1.ImageIndex = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem1.Id = 22;
            this.barStaticItem1.ImageIndex = 4;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem2.Id = 23;
            this.barStaticItem2.ImageIndex = 5;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Thông tin cá nhân";
            this.barButtonItem2.Id = 24;
            this.barButtonItem2.ImageIndex = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Đăng ký chứng thư số";
            this.barButtonItem3.Id = 25;
            this.barButtonItem3.ImageIndex = 9;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Thoát";
            this.barButtonItem4.Id = 26;
            this.barButtonItem4.ImageIndex = 0;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "Đăng ký chứng thư số";
            this.barButtonItem5.Id = 27;
            this.barButtonItem5.ImageIndex = 9;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem3.Id = 28;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Id = 29;
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barStaticItem4_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "Đăng nhập";
            this.barButtonItem6.Enabled = false;
            this.barButtonItem6.Id = 30;
            this.barButtonItem6.ImageIndex = 6;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barButtonItem1);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItem2);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItem1);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItem3);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItem4);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 522);
            this.ribbonStatusBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(515, 31);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "market.png");
            this.imageCollection1.Images.SetKeyName(1, "markets.png");
            this.imageCollection1.Images.SetKeyName(2, "logout.png");
            this.imageCollection1.Images.SetKeyName(3, "clock.png");
            // 
            // mainDocumentManager
            // 
            this.mainDocumentManager.MdiParent = this;
            this.mainDocumentManager.MenuManager = this.ribbon;
            this.mainDocumentManager.View = this.tabbedView1;
            this.mainDocumentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // imageCollection2
            // 
            this.imageCollection2.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection2.ImageStream")));
            this.imageCollection2.Images.SetKeyName(0, "SKINS_Black---16x16.png");
            this.imageCollection2.Images.SetKeyName(1, "SKINS_Blue---16x16.png");
            this.imageCollection2.Images.SetKeyName(2, "SKINS_Blueprint.png");
            this.imageCollection2.Images.SetKeyName(3, "SKINS_Caramel---16x16.png");
            this.imageCollection2.Images.SetKeyName(4, "SKINS_Coffee---16x16.png");
            this.imageCollection2.Images.SetKeyName(5, "SKINS_Darkroom---16x16.png");
            this.imageCollection2.Images.SetKeyName(6, "SKINS_Dark-Side---16x16.png");
            this.imageCollection2.Images.SetKeyName(7, "SKINS_DevExpress-2010-Style---16x16.png");
            this.imageCollection2.Images.SetKeyName(8, "SKINS_DevExpress-Dark.png");
            this.imageCollection2.Images.SetKeyName(9, "SKINS_Foggy---16x16.png");
            this.imageCollection2.Images.SetKeyName(10, "SKINS_Glass-Oceans---16x16.png");
            this.imageCollection2.Images.SetKeyName(11, "SKINS_High-Contrast---16x16.png");
            this.imageCollection2.Images.SetKeyName(12, "SKINS_iIMaginary---16x16.png");
            this.imageCollection2.Images.SetKeyName(13, "SKINS_Lilian---16x16.png");
            this.imageCollection2.Images.SetKeyName(14, "SKINS_Liqiud-Sky---16x16.png");
            this.imageCollection2.Images.SetKeyName(15, "SKINS_London-Liquid-Sky---16x16.png");
            this.imageCollection2.Images.SetKeyName(16, "SKINS_McSkin---16x16.png");
            this.imageCollection2.Images.SetKeyName(17, "SKINS_Money-Twins---16x16.png");
            this.imageCollection2.Images.SetKeyName(18, "SKINS_Office-2007-Black---16x16.png");
            this.imageCollection2.Images.SetKeyName(19, "SKINS_Office-2007-Blue---16x16.png");
            this.imageCollection2.Images.SetKeyName(20, "SKINS_Office-2007-Green---16x16.png");
            this.imageCollection2.Images.SetKeyName(21, "SKINS_Office-2007-Pink---16x16.png");
            this.imageCollection2.Images.SetKeyName(22, "SKINS_Office-2007-Silver---16x16.png");
            this.imageCollection2.Images.SetKeyName(23, "SKINS_Office-2010-Black---16x16.png");
            this.imageCollection2.Images.SetKeyName(24, "SKINS_Office-2010-Blue---16x16.png");
            this.imageCollection2.Images.SetKeyName(25, "SKINS_Office-2010-Silver---16x16.png");
            this.imageCollection2.Images.SetKeyName(26, "SKINS_Pumpkin---16x16.png");
            this.imageCollection2.Images.SetKeyName(27, "SKINS_Seven---16x16.png");
            this.imageCollection2.Images.SetKeyName(28, "SKINS_Seven-Classic---16x16.png");
            this.imageCollection2.Images.SetKeyName(29, "SKINS_Sharp---16x16.png");
            this.imageCollection2.Images.SetKeyName(30, "SKINS_Sharp-Plus---16x16.png");
            this.imageCollection2.Images.SetKeyName(31, "SKINS_Springtime---16x16.png");
            this.imageCollection2.Images.SetKeyName(32, "SKINS_Stardust---16x16.png");
            this.imageCollection2.Images.SetKeyName(33, "SKINS_Summer---16x16.png");
            this.imageCollection2.Images.SetKeyName(34, "SKINS_The-Asphalt-World---16x16.png");
            this.imageCollection2.Images.SetKeyName(35, "SKINS_Valentine---16x16.png");
            this.imageCollection2.Images.SetKeyName(36, "SKINS_VS2010---16x16.png");
            this.imageCollection2.Images.SetKeyName(37, "SKINS_Whiteprint.png");
            this.imageCollection2.Images.SetKeyName(38, "SKINS_Xmas---16x16.png");
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // applicationMenu1
            // 
            this.applicationMenu1.ItemLinks.Add(this.barButtonItem6);
            this.applicationMenu1.ItemLinks.Add(this.barButtonItem2);
            this.applicationMenu1.ItemLinks.Add(this.barButtonItem3);
            this.applicationMenu1.ItemLinks.Add(this.barButtonItem4);
            this.applicationMenu1.Name = "applicationMenu1";
            this.applicationMenu1.Ribbon = this.ribbon;
            // 
            // popupMenu1
            // 
            this.popupMenu1.Name = "popupMenu1";
            this.popupMenu1.Ribbon = this.ribbon;
            // 
            // timer2
            // 
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // frmMainRibbon
            // 
            this.AllowMdiBar = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 553);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMainRibbon";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "frmMainRibbon";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMainRibbon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainDocumentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarEditItem txtModuleID;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.Docking2010.DocumentManager mainDocumentManager;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        //private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.Utils.ImageCollection imageCollection2;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarStaticItem barStaticItem4;
        private System.Windows.Forms.Timer timer2;
        private DevExpress.Utils.ImageCollection imageCollection3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}
namespace FIS.FReader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnReconnect = new DevExpress.XtraEditors.PictureEdit();
            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
            this.btnFolderBrowser = new DevExpress.XtraEditors.SimpleButton();
            this.txtFolderHome = new DevExpress.XtraEditors.TextEdit();
            this.txtLog = new DevExpress.XtraEditors.MemoEdit();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.txtPassWord = new DevExpress.XtraEditors.TextEdit();
            this.txtUserName = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lblStatus = new DevExpress.XtraLayout.SimpleLabelItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnReconnect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolderHome.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassWord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnReconnect);
            this.layoutControl1.Controls.Add(this.btnStart);
            this.layoutControl1.Controls.Add(this.btnFolderBrowser);
            this.layoutControl1.Controls.Add(this.txtFolderHome);
            this.layoutControl1.Controls.Add(this.txtLog);
            this.layoutControl1.Controls.Add(this.btnLogin);
            this.layoutControl1.Controls.Add(this.txtPassWord);
            this.layoutControl1.Controls.Add(this.txtUserName);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(419, 211, 432, 399);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(861, 486);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnReconnect
            // 
            this.btnReconnect.EditValue = ((object)(resources.GetObject("btnReconnect.EditValue")));
            this.btnReconnect.Location = new System.Drawing.Point(204, 442);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnReconnect.Properties.Appearance.Options.UseBackColor = true;
            this.btnReconnect.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.btnReconnect.Size = new System.Drawing.Size(25, 20);
            this.btnReconnect.StyleController = this.layoutControl1;
            this.btnReconnect.TabIndex = 11;
            this.btnReconnect.Click += new System.EventHandler(this.btnReconnect_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(749, 440);
            this.btnStart.MaximumSize = new System.Drawing.Size(88, 22);
            this.btnStart.MinimumSize = new System.Drawing.Size(88, 22);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(88, 22);
            this.btnStart.StyleController = this.layoutControl1;
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Đọc dữ liệu";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnFolderBrowser
            // 
            this.btnFolderBrowser.Location = new System.Drawing.Point(749, 42);
            this.btnFolderBrowser.MaximumSize = new System.Drawing.Size(88, 22);
            this.btnFolderBrowser.MinimumSize = new System.Drawing.Size(88, 22);
            this.btnFolderBrowser.Name = "btnFolderBrowser";
            this.btnFolderBrowser.Size = new System.Drawing.Size(88, 22);
            this.btnFolderBrowser.StyleController = this.layoutControl1;
            this.btnFolderBrowser.TabIndex = 9;
            this.btnFolderBrowser.Text = "Đường dẫn";
            this.btnFolderBrowser.Click += new System.EventHandler(this.btnFolderBrowser_Click);
            // 
            // txtFolderHome
            // 
            this.txtFolderHome.Location = new System.Drawing.Point(332, 42);
            this.txtFolderHome.Name = "txtFolderHome";
            this.txtFolderHome.Size = new System.Drawing.Size(413, 20);
            this.txtFolderHome.StyleController = this.layoutControl1;
            this.txtFolderHome.TabIndex = 8;
            // 
            // txtLog
            // 
            this.txtLog.EditValue = "";
            this.txtLog.Location = new System.Drawing.Point(257, 68);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(580, 368);
            this.txtLog.StyleController = this.layoutControl1;
            this.txtLog.TabIndex = 7;
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogin.Location = new System.Drawing.Point(24, 122);
            this.btnLogin.MaximumSize = new System.Drawing.Size(205, 22);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(205, 22);
            this.btnLogin.StyleController = this.layoutControl1;
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassWord
            // 
            this.txtPassWord.Location = new System.Drawing.Point(24, 98);
            this.txtPassWord.MaximumSize = new System.Drawing.Size(205, 20);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.Size = new System.Drawing.Size(205, 20);
            this.txtPassWord.StyleController = this.layoutControl1;
            this.txtPassWord.TabIndex = 5;
            this.txtPassWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassWord_KeyDown);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(24, 58);
            this.txtUserName.MaximumSize = new System.Drawing.Size(205, 20);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(205, 20);
            this.txtUserName.StyleController = this.layoutControl1;
            this.txtUserName.TabIndex = 4;
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyDown);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup3,
            this.layoutControlGroup2});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(861, 486);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.CustomizationFormText = "Đọc dữ liệu";
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.emptySpaceItem1});
            this.layoutControlGroup3.Location = new System.Drawing.Point(233, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(608, 466);
            this.layoutControlGroup3.Text = "Đọc dữ liệu";
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtLog;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(584, 372);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.txtFolderHome;
            this.layoutControlItem5.CustomizationFormText = "Đường dẫn";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(492, 26);
            this.layoutControlItem5.Text = "Đường dẫn";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(72, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnFolderBrowser;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(492, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(92, 26);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnStart;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
            this.layoutControlItem7.Location = new System.Drawing.Point(492, 398);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(92, 26);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 398);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(492, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "Tài khoản";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.emptySpaceItem2,
            this.lblStatus,
            this.layoutControlItem8});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(233, 466);
            this.layoutControlGroup2.Text = "Tài khoản";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtUserName;
            this.layoutControlItem1.CustomizationFormText = "Tên đăng nhập";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(209, 40);
            this.layoutControlItem1.Text = "Tên đăng nhập";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(72, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtPassWord;
            this.layoutControlItem2.CustomizationFormText = "Mật khẩu";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 40);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(209, 40);
            this.layoutControlItem2.Text = "Mật khẩu";
            this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(72, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnLogin;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 80);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(209, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 106);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(209, 294);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lblStatus
            // 
            this.lblStatus.AllowHotTrack = false;
            this.lblStatus.CustomizationFormText = "LabellblStatus";
            this.lblStatus.Location = new System.Drawing.Point(0, 400);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(180, 24);
            this.lblStatus.TextSize = new System.Drawing.Size(72, 13);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnReconnect;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem8";
            this.layoutControlItem8.Location = new System.Drawing.Point(180, 400);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(29, 24);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 486);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frmMain";
            this.Text = "F-Reader";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnReconnect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolderHome.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassWord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.MemoEdit txtLog;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.TextEdit txtPassWord;
        private DevExpress.XtraEditors.TextEdit txtUserName;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btnFolderBrowser;
        private DevExpress.XtraEditors.TextEdit txtFolderHome;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private DevExpress.XtraEditors.SimpleButton btnStart;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.SimpleLabelItem lblStatus;
        private DevExpress.XtraEditors.PictureEdit btnReconnect;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;

    }
}


namespace FIS.AppClient.Controls
{
    public partial class ucIMWizard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wzMain = new DevExpress.XtraWizard.WizardControl();
            this.wzpWelcome = new DevExpress.XtraWizard.WelcomeWizardPage();
            this.wzpReadFile = new DevExpress.XtraWizard.WizardPage();
            this.lbStatus = new DevExpress.XtraEditors.LabelControl();
            this.pgbReadFile = new DevExpress.XtraEditors.ProgressBarControl();
            this.txtFileName = new DevExpress.XtraEditors.ButtonEdit();
            this.lbFileName = new DevExpress.XtraEditors.LabelControl();
            this.completionWizardPage1 = new DevExpress.XtraWizard.CompletionWizardPage();
            this.wzpSetting = new DevExpress.XtraWizard.WizardPage();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.txtFileNameImp = new DevExpress.XtraEditors.ButtonEdit();
            this.lblStatusText = new DevExpress.XtraEditors.LabelControl();
            this.btnImport = new DevExpress.XtraEditors.SimpleButton();
            this.mainLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wzMain)).BeginInit();
            this.wzMain.SuspendLayout();
            this.wzpReadFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgbReadFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName.Properties)).BeginInit();
            this.wzpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileNameImp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // wzMain
            // 
            this.wzMain.CancelText = "Hủy";
            this.wzMain.Controls.Add(this.wzpWelcome);
            this.wzMain.Controls.Add(this.wzpReadFile);
            this.wzMain.Controls.Add(this.completionWizardPage1);
            this.wzMain.Controls.Add(this.wzpSetting);
            this.wzMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wzMain.FinishText = "&Hoàn thành";
            this.wzMain.Location = new System.Drawing.Point(0, 0);
            this.wzMain.Name = "wzMain";
            this.wzMain.NextText = "&Tiếp tục >";
            this.wzMain.Pages.AddRange(new DevExpress.XtraWizard.BaseWizardPage[] {
            this.wzpWelcome,
            this.wzpReadFile,
            this.wzpSetting,
            this.completionWizardPage1});
            this.wzMain.PreviousText = "< &Về trước";
            this.wzMain.Size = new System.Drawing.Size(820, 546);
            this.wzMain.CancelClick += new System.ComponentModel.CancelEventHandler(this.wzMain_CancelClick);
            this.wzMain.FinishClick += new System.ComponentModel.CancelEventHandler(this.wzMain_CancelClick);
            this.wzMain.Click += new System.EventHandler(this.wzMain_Click);
            this.wzMain.Leave += new System.EventHandler(this.wzMain_Leave);
            // 
            // wzpWelcome
            // 
            this.wzpWelcome.Name = "wzpWelcome";
            this.wzpWelcome.ProceedText = "Để Import nhấn nút Tiếp tục";
            this.wzpWelcome.Size = new System.Drawing.Size(603, 413);
            this.wzpWelcome.PageCommit += new System.EventHandler(this.wzpWelcome_PageCommit);
            this.wzpWelcome.Click += new System.EventHandler(this.wzpWelcome_Click);
            // 
            // wzpReadFile
            // 
            this.wzpReadFile.Controls.Add(this.lbStatus);
            this.wzpReadFile.Controls.Add(this.pgbReadFile);
            this.wzpReadFile.Controls.Add(this.txtFileName);
            this.wzpReadFile.Controls.Add(this.lbFileName);
            this.wzpReadFile.DescriptionText = "";
            this.wzpReadFile.Name = "wzpReadFile";
            this.wzpReadFile.Size = new System.Drawing.Size(788, 401);
            this.wzpReadFile.Text = "IMPORT_SETTING";
            this.wzpReadFile.Visible = false;
            this.wzpReadFile.PageCommit += new System.EventHandler(this.wzpReadFile_PageCommit);
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbStatus.Location = new System.Drawing.Point(3, 361);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(82, 13);
            this.lbStatus.TabIndex = 3;
            this.lbStatus.Text = "Tiến trình đọc file";
            // 
            // pgbReadFile
            // 
            this.pgbReadFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbReadFile.Location = new System.Drawing.Point(3, 380);
            this.pgbReadFile.Name = "pgbReadFile";
            this.pgbReadFile.Size = new System.Drawing.Size(782, 18);
            this.pgbReadFile.TabIndex = 2;
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Location = new System.Drawing.Point(109, 3);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtFileName.Size = new System.Drawing.Size(676, 20);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtFileName_ButtonClick);
            // 
            // lbFileName
            // 
            this.lbFileName.Location = new System.Drawing.Point(3, 6);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(70, 13);
            this.lbFileName.TabIndex = 0;
            this.lbFileName.Text = "Tên file dữ liệu";
            // 
            // completionWizardPage1
            // 
            this.completionWizardPage1.FinishText = "Bạn đã hoàn tất việc nhập liệu vào hệ thống";
            this.completionWizardPage1.Name = "completionWizardPage1";
            this.completionWizardPage1.ProceedText = "Nhấn nút \"Hoàn thành\" để kết thúc";
            this.completionWizardPage1.Size = new System.Drawing.Size(603, 413);
            this.completionWizardPage1.Text = "Hoàn thành nhập liệu";
            // 
            // wzpSetting
            // 
            this.wzpSetting.Controls.Add(this.mainLayout);
            this.wzpSetting.DescriptionText = "";
            this.wzpSetting.Name = "wzpSetting";
            this.wzpSetting.Size = new System.Drawing.Size(788, 401);
            this.wzpSetting.Click += new System.EventHandler(this.wzpSetting_Click);
            // 
            // mainLayout
            // 
            this.mainLayout.AllowCustomizationMenu = false;
            this.mainLayout.Controls.Add(this.txtFileNameImp);
            this.mainLayout.Controls.Add(this.lblStatusText);
            this.mainLayout.Controls.Add(this.btnImport);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Margin = new System.Windows.Forms.Padding(0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(492, 344, 250, 350);
            this.mainLayout.OptionsCustomizationForm.ShowLoadButton = false;
            this.mainLayout.OptionsCustomizationForm.ShowPropertyGrid = true;
            this.mainLayout.OptionsCustomizationForm.ShowSaveButton = false;
            this.mainLayout.OptionsSerialization.RestoreAppearanceItemCaption = true;
            this.mainLayout.OptionsSerialization.RestoreAppearanceTabPage = true;
            this.mainLayout.OptionsSerialization.RestoreGroupPadding = true;
            this.mainLayout.OptionsSerialization.RestoreGroupSpacing = true;
            this.mainLayout.OptionsSerialization.RestoreLayoutGroupAppearanceGroup = true;
            this.mainLayout.OptionsSerialization.RestoreLayoutItemPadding = true;
            this.mainLayout.OptionsSerialization.RestoreLayoutItemSpacing = true;
            this.mainLayout.OptionsSerialization.RestoreRootGroupPadding = true;
            this.mainLayout.OptionsSerialization.RestoreRootGroupSpacing = true;
            this.mainLayout.OptionsSerialization.RestoreTabbedGroupPadding = true;
            this.mainLayout.OptionsSerialization.RestoreTabbedGroupSpacing = true;
            this.mainLayout.OptionsSerialization.RestoreTextToControlDistance = true;
            this.mainLayout.OptionsView.HighlightFocusedItem = true;
            this.mainLayout.Root = this.mainLayoutGroup;
            this.mainLayout.Size = new System.Drawing.Size(788, 401);
            this.mainLayout.TabIndex = 4;
            this.mainLayout.Text = "layoutControl1";
            // 
            // txtFileNameImp
            // 
            this.txtFileNameImp.Location = new System.Drawing.Point(77, 2);
            this.txtFileNameImp.Name = "txtFileNameImp";
            this.txtFileNameImp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtFileNameImp.Size = new System.Drawing.Size(709, 20);
            this.txtFileNameImp.StyleController = this.mainLayout;
            this.txtFileNameImp.TabIndex = 8;
            this.txtFileNameImp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtFileName_ButtonClick);
            // 
            // lblStatusText
            // 
            this.lblStatusText.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lblStatusText.Location = new System.Drawing.Point(2, 26);
            this.lblStatusText.Name = "lblStatusText";
            this.lblStatusText.Size = new System.Drawing.Size(53, 13);
            this.lblStatusText.StyleController = this.mainLayout;
            this.lblStatusText.TabIndex = 9;
            this.lblStatusText.Text = "StatusText";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Image = global::FIS.AppClient.Properties.Resources.IMPORT;
            this.btnImport.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btnImport.Location = new System.Drawing.Point(59, 26);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(727, 22);
            this.btnImport.StyleController = this.mainLayout;
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // mainLayoutGroup
            // 
            this.mainLayoutGroup.CustomizationFormText = "mainLayoutGroup";
            this.mainLayoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.mainLayoutGroup.GroupBordersVisible = false;
            this.mainLayoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem1,
            this.layoutControlItem3});
            this.mainLayoutGroup.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutGroup.Name = "Root";
            this.mainLayoutGroup.OptionsItemText.TextToControlDistance = 5;
            this.mainLayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.mainLayoutGroup.Size = new System.Drawing.Size(788, 401);
            this.mainLayoutGroup.Text = "Root";
            this.mainLayoutGroup.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtFileNameImp;
            this.layoutControlItem1.CustomizationFormText = "Tên file dữ liệu";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(788, 24);
            this.layoutControlItem1.Text = "Tên file dữ liệu";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(70, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnImport;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(57, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(731, 26);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 50);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(788, 351);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lblStatusText;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(57, 26);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // ucIMWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wzMain);
            this.Name = "ucIMWizard";
            this.Size = new System.Drawing.Size(820, 546);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wzMain)).EndInit();
            this.wzMain.ResumeLayout(false);
            this.wzpReadFile.ResumeLayout(false);
            this.wzpReadFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgbReadFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileName.Properties)).EndInit();
            this.wzpSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFileNameImp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraWizard.WizardControl wzMain;
        private DevExpress.XtraWizard.WelcomeWizardPage wzpWelcome;
        private DevExpress.XtraWizard.WizardPage wzpReadFile;
        private DevExpress.XtraWizard.CompletionWizardPage completionWizardPage1;
        private DevExpress.XtraEditors.LabelControl lbFileName;
        private DevExpress.XtraEditors.ButtonEdit txtFileName;
        private DevExpress.XtraEditors.ProgressBarControl pgbReadFile;
        private DevExpress.XtraEditors.LabelControl lbStatus;
        private DevExpress.XtraWizard.WizardPage wzpSetting;
        private DevExpress.XtraEditors.SimpleButton btnImport;
        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraLayout.LayoutControlGroup mainLayoutGroup;
        private DevExpress.XtraEditors.ButtonEdit txtFileNameImp;
        private DevExpress.XtraEditors.LabelControl lblStatusText;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}

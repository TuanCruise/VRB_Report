namespace FIS.AppClient.Controls
{
    partial class ucMaintain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMaintain));
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.mainLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.btnCommit = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.lnkFile = new DevExpress.XtraEditors.HyperLinkEdit();
            this.btnLoadReport = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnGetData = new DevExpress.XtraEditors.SimpleButton();
            this.lblDownFile = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lnkFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.AllowCustomizationMenu = false;
            this.mainLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainLayout.Appearance.ControlFocused.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.mainLayout.Appearance.ControlFocused.Options.UseBorderColor = true;
            this.mainLayout.Location = new System.Drawing.Point(4, 55);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.OptionsCustomizationForm.ShowLoadButton = false;
            this.mainLayout.OptionsCustomizationForm.ShowPropertyGrid = true;
            this.mainLayout.OptionsCustomizationForm.ShowSaveButton = false;
            this.mainLayout.OptionsFocus.AllowFocusReadonlyEditors = false;
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
            this.mainLayout.Size = new System.Drawing.Size(753, 366);
            this.mainLayout.TabIndex = 101;
            this.mainLayout.Text = "layoutControl1";
            // 
            // mainLayoutGroup
            // 
            this.mainLayoutGroup.CustomizationFormText = "layoutControlGroup1";
            this.mainLayoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.mainLayoutGroup.GroupBordersVisible = false;
            this.mainLayoutGroup.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutGroup.Name = "mainLayoutGroup";
            this.mainLayoutGroup.OptionsItemText.TextToControlDistance = 5;
            this.mainLayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.mainLayoutGroup.Size = new System.Drawing.Size(753, 366);
            this.mainLayoutGroup.Text = "mainLayoutGroup";
            this.mainLayoutGroup.TextVisible = false;
            // 
            // lbTitle
            // 
            this.lbTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.lbTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.lbTitle.Size = new System.Drawing.Size(760, 50);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "lbTitle";
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommit.Image = ((System.Drawing.Image)(resources.GetObject("btnCommit.Image")));
            this.btnCommit.Location = new System.Drawing.Point(574, 433);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(86, 23);
            this.btnCommit.TabIndex = 99;
            this.btnCommit.Text = "btnCommit";
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(666, 433);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(83, 23);
            this.btnClose.TabIndex = 100;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lnkFile
            // 
            this.lnkFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkFile.EditValue = "";
            this.lnkFile.Location = new System.Drawing.Point(3, 434);
            this.lnkFile.Name = "lnkFile";
            this.lnkFile.Properties.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.lnkFile.Properties.Appearance.Options.UseBackColor = true;
            this.lnkFile.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.lnkFile.Properties.Caption = "Xem file đính kèm";
            this.lnkFile.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lnkFile.Size = new System.Drawing.Size(93, 22);
            this.lnkFile.StyleController = this.mainLayout;
            this.lnkFile.TabIndex = 103;
            this.lnkFile.ToolTip = "Bấm chuột phải để tải file đính kèm";
            this.lnkFile.Visible = false;
            // 
            // btnLoadReport
            // 
            this.btnLoadReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadReport.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadReport.Image")));
            this.btnLoadReport.Location = new System.Drawing.Point(458, 433);
            this.btnLoadReport.Name = "btnLoadReport";
            this.btnLoadReport.Size = new System.Drawing.Size(110, 23);
            this.btnLoadReport.TabIndex = 104;
            this.btnLoadReport.Text = "btnLoadReport";
            this.btnLoadReport.Visible = false;
            this.btnLoadReport.Click += new System.EventHandler(this.btnLoadReport_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(366, 433);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 23);
            this.btnSave.TabIndex = 105;
            this.btnSave.Text = "btnSave";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetData
            // 
            this.btnGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetData.Image = ((System.Drawing.Image)(resources.GetObject("btnGetData.Image")));
            this.btnGetData.Location = new System.Drawing.Point(274, 433);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(86, 23);
            this.btnGetData.TabIndex = 106;
            this.btnGetData.Text = "btnGetData";
            this.btnGetData.Visible = false;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // lblDownFile
            // 
            this.lblDownFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDownFile.Location = new System.Drawing.Point(102, 438);
            this.lblDownFile.Name = "lblDownFile";
            this.lblDownFile.Size = new System.Drawing.Size(61, 13);
            this.lblDownFile.TabIndex = 107;
            this.lblDownFile.Text = "File đính kèm";
            this.lblDownFile.Visible = false;
            // 
            // ucMaintain
            // 
            this.AcceptButton = this.btnCommit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.lblDownFile);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoadReport);
            this.Controls.Add(this.lnkFile);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.btnCommit);
            this.Name = "ucMaintain";
            this.Size = new System.Drawing.Size(760, 466);
            this.ModuleClosing += new System.ComponentModel.CancelEventHandler(this.ucMaintain_ModuleClosing);
            this.Load += new System.EventHandler(this.ucMaintain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lnkFile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCommit;
        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraLayout.LayoutControlGroup mainLayoutGroup;
        public DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraEditors.HyperLinkEdit lnkFile;
        private DevExpress.XtraEditors.SimpleButton btnLoadReport;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnGetData;
        private DevExpress.XtraEditors.LabelControl lblDownFile;
    }
}

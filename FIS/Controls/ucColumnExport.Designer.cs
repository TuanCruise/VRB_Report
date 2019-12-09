namespace FIS.AppClient.Controls
{
    partial class ucColumnExport
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.rdgExportColumn = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.rdgExportTitle = new DevExpress.XtraEditors.CheckEdit();
            this.chkLstColumnExport = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.lstExecuteResult = new DevExpress.XtraEditors.ImageListBoxControl();
            this.tabBatchLog = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabBatchSetting = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.chkCheckAll = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdgExportColumn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgExportTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLstColumnExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstExecuteResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCheckAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutControl1.Controls.Add(this.rdgExportColumn);
            this.layoutControl1.Controls.Add(this.layoutControl2);
            this.layoutControl1.Controls.Add(this.rdgExportTitle);
            this.layoutControl1.Controls.Add(this.chkLstColumnExport);
            this.layoutControl1.Controls.Add(this.lstExecuteResult);
            this.layoutControl1.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabBatchLog,
            this.tabBatchSetting});
            this.layoutControl1.Location = new System.Drawing.Point(2, 50);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(546, 255);
            this.layoutControl1.TabIndex = 3;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // rdgExportColumn
            // 
            this.rdgExportColumn.Location = new System.Drawing.Point(147, 12);
            this.rdgExportColumn.Name = "rdgExportColumn";
            this.rdgExportColumn.Properties.Caption = "Xuất theo cột";
            this.rdgExportColumn.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.rdgExportColumn.Properties.RadioGroupIndex = 1;
            this.rdgExportColumn.Size = new System.Drawing.Size(132, 19);
            this.rdgExportColumn.StyleController = this.layoutControl1;
            this.rdgExportColumn.TabIndex = 8;
            this.rdgExportColumn.TabStop = false;
            this.rdgExportColumn.CheckedChanged += new System.EventHandler(this.rdgExportColumn_CheckedChanged);
            // 
            // layoutControl2
            // 
            this.layoutControl2.Location = new System.Drawing.Point(283, 12);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.Root;
            this.layoutControl2.Size = new System.Drawing.Size(251, 20);
            this.layoutControl2.TabIndex = 7;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // Root
            // 
            this.Root.CustomizationFormText = "Root";
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Location = new System.Drawing.Point(0, 0);
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(251, 20);
            this.Root.Text = "Root";
            this.Root.TextVisible = false;
            // 
            // rdgExportTitle
            // 
            this.rdgExportTitle.EditValue = true;
            this.rdgExportTitle.Location = new System.Drawing.Point(12, 12);
            this.rdgExportTitle.Name = "rdgExportTitle";
            this.rdgExportTitle.Properties.Caption = "Xuất đủ title";
            this.rdgExportTitle.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.rdgExportTitle.Properties.RadioGroupIndex = 1;
            this.rdgExportTitle.Size = new System.Drawing.Size(131, 19);
            this.rdgExportTitle.StyleController = this.layoutControl1;
            this.rdgExportTitle.TabIndex = 5;
            this.rdgExportTitle.CheckedChanged += new System.EventHandler(this.rdgExportTitle_CheckedChanged);
            // 
            // chkLstColumnExport
            // 
            this.chkLstColumnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLstColumnExport.CheckOnClick = true;
            this.chkLstColumnExport.Location = new System.Drawing.Point(12, 36);
            this.chkLstColumnExport.Name = "chkLstColumnExport";
            this.chkLstColumnExport.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.chkLstColumnExport.Size = new System.Drawing.Size(522, 207);
            this.chkLstColumnExport.StyleController = this.layoutControl1;
            this.chkLstColumnExport.TabIndex = 4;
            // 
            // lstExecuteResult
            // 
            this.lstExecuteResult.Location = new System.Drawing.Point(24, 44);
            this.lstExecuteResult.Name = "lstExecuteResult";
            this.lstExecuteResult.Size = new System.Drawing.Size(498, 187);
            this.lstExecuteResult.StyleController = this.layoutControl1;
            this.lstExecuteResult.TabIndex = 4;
            // 
            // tabBatchLog
            // 
            this.tabBatchLog.CustomizationFormText = "Kết quả Thực thi";
            this.tabBatchLog.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.tabBatchLog.Location = new System.Drawing.Point(0, 0);
            this.tabBatchLog.Name = "tabBatchLog";
            this.tabBatchLog.Size = new System.Drawing.Size(502, 191);
            this.tabBatchLog.Text = "Kết quả Thực thi";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lstExecuteResult;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(502, 191);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // tabBatchSetting
            // 
            this.tabBatchSetting.CustomizationFormText = "Thiết lập Batch";
            this.tabBatchSetting.Location = new System.Drawing.Point(0, 0);
            this.tabBatchSetting.Name = "tabBatchSetting";
            this.tabBatchSetting.Size = new System.Drawing.Size(502, 191);
            this.tabBatchSetting.Text = "Thiết lập Batch";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem4});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(546, 255);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.chkLstColumnExport;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(526, 211);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.rdgExportTitle;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(135, 24);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.layoutControl2;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(271, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(255, 24);
            this.layoutControlItem5.Text = "layoutControlItem5";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextToControlDistance = 0;
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.rdgExportColumn;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(135, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(136, 24);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::FIS.AppClient.Properties.Resources.SAVE;
            this.btnSave.Location = new System.Drawing.Point(463, 305);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.lbTitle.Size = new System.Drawing.Size(548, 50);
            this.lbTitle.TabIndex = 2;
            this.lbTitle.Text = "lbTitle";
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCheckAll.EditValue = true;
            this.chkCheckAll.Location = new System.Drawing.Point(14, 303);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Properties.Caption = "Tất cả";
            this.chkCheckAll.Size = new System.Drawing.Size(131, 19);
            this.chkCheckAll.StyleController = this.layoutControl1;
            this.chkCheckAll.TabIndex = 6;
            this.chkCheckAll.CheckedChanged += new System.EventHandler(this.chkCheckAll_CheckedChanged);
            // 
            // ucColumnExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkCheckAll);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.btnSave);
            this.Name = "ucColumnExport";
            this.Size = new System.Drawing.Size(548, 343);
            this.Load += new System.EventHandler(this.ucColumnExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rdgExportColumn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgExportTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLstColumnExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstExecuteResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCheckAll.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.CheckEdit rdgExportTitle;
        private DevExpress.XtraEditors.CheckedListBoxControl chkLstColumnExport;
        private DevExpress.XtraEditors.ImageListBoxControl lstExecuteResult;
        private DevExpress.XtraLayout.LayoutControlGroup tabBatchLog;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlGroup tabBatchSetting;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.CheckEdit rdgExportColumn;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit chkCheckAll;
    }
}

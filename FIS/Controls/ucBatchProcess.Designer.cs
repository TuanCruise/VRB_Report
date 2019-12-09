namespace FIS.AppClient.Controls
{
    partial class ucBatchProcess
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
            this.chkLstBatchName = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.lstExecuteResult = new DevExpress.XtraEditors.ImageListBoxControl();
            this.btnBatch = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
            this.tabBatchSetting = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabBatchLog = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.chkCheckAll = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLstBatchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstExecuteResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCheckAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkLstBatchName
            // 
            this.chkLstBatchName.CheckOnClick = true;
            this.chkLstBatchName.Location = new System.Drawing.Point(102, 44);
            this.chkLstBatchName.Name = "chkLstBatchName";
            this.chkLstBatchName.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.chkLstBatchName.Size = new System.Drawing.Size(420, 213);
            this.chkLstBatchName.StyleController = this.mainLayout;
            this.chkLstBatchName.TabIndex = 0;
            // 
            // mainLayout
            // 
            this.mainLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainLayout.Controls.Add(this.lstExecuteResult);
            this.mainLayout.Controls.Add(this.chkLstBatchName);
            this.mainLayout.Controls.Add(this.btnBatch);
            this.mainLayout.Location = new System.Drawing.Point(2, 50);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Root = this.layoutControlGroup1;
            this.mainLayout.Size = new System.Drawing.Size(546, 281);
            this.mainLayout.TabIndex = 3;
            this.mainLayout.Text = "layoutControl1";
            // 
            // lstExecuteResult
            // 
            this.lstExecuteResult.Location = new System.Drawing.Point(24, 44);
            this.lstExecuteResult.Name = "lstExecuteResult";
            this.lstExecuteResult.Size = new System.Drawing.Size(498, 213);
            this.lstExecuteResult.StyleController = this.mainLayout;
            this.lstExecuteResult.TabIndex = 4;
            this.lstExecuteResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstExecuteResult_MouseDoubleClick);
            // 
            // btnBatch
            // 
            this.btnBatch.Image = global::FIS.AppClient.Properties.Resources.STOREEXECUTE;
            this.btnBatch.Location = new System.Drawing.Point(24, 44);
            this.btnBatch.Name = "btnBatch";
            this.btnBatch.Size = new System.Drawing.Size(74, 22);
            this.btnBatch.StyleController = this.mainLayout;
            this.btnBatch.TabIndex = 1;
            this.btnBatch.Text = "btnBatch";
            this.btnBatch.Click += new System.EventHandler(this.btnBatch_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabbedControlGroup1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(546, 281);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // tabbedControlGroup1
            // 
            this.tabbedControlGroup1.CustomizationFormText = "tabbedControlGroup1";
            this.tabbedControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.tabbedControlGroup1.Name = "tabbedControlGroup1";
            this.tabbedControlGroup1.SelectedTabPage = this.tabBatchSetting;
            this.tabbedControlGroup1.SelectedTabPageIndex = 0;
            this.tabbedControlGroup1.Size = new System.Drawing.Size(526, 261);
            this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabBatchSetting,
            this.tabBatchLog});
            this.tabbedControlGroup1.Text = "tabbedControlGroup1";
            // 
            // tabBatchSetting
            // 
            this.tabBatchSetting.CustomizationFormText = "Thiết lập Batch";
            this.tabBatchSetting.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3});
            this.tabBatchSetting.Location = new System.Drawing.Point(0, 0);
            this.tabBatchSetting.Name = "tabBatchSetting";
            this.tabBatchSetting.Size = new System.Drawing.Size(502, 217);
            this.tabBatchSetting.Text = "Thiết lập Batch";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.chkLstBatchName;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(78, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(424, 217);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnBatch;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(78, 217);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // tabBatchLog
            // 
            this.tabBatchLog.CustomizationFormText = "Kết quả Thực thi";
            this.tabBatchLog.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.tabBatchLog.Location = new System.Drawing.Point(0, 0);
            this.tabBatchLog.Name = "tabBatchLog";
            this.tabBatchLog.Size = new System.Drawing.Size(502, 217);
            this.tabBatchLog.Text = "Kết quả Thực thi";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lstExecuteResult;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(502, 217);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
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
            this.lbTitle.Text = "Xử lý lô";
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkCheckAll.EditValue = true;
            this.chkCheckAll.Location = new System.Drawing.Point(0, 331);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Properties.Caption = "Tất cả";
            this.chkCheckAll.Size = new System.Drawing.Size(548, 19);
            this.chkCheckAll.TabIndex = 4;
            this.chkCheckAll.CheckedChanged += new System.EventHandler(this.chkCheckAll_CheckedChanged);
            // 
            // ucBatchProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkCheckAll);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.lbTitle);
            this.Name = "ucBatchProcess";
            this.Size = new System.Drawing.Size(548, 350);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLstBatchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstExecuteResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabBatchLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCheckAll.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckedListBoxControl chkLstBatchName;
        private DevExpress.XtraEditors.SimpleButton btnBatch;
        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup tabBatchLog;
        private DevExpress.XtraLayout.LayoutControlGroup tabBatchSetting;
        private DevExpress.XtraEditors.ImageListBoxControl lstExecuteResult;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.CheckEdit chkCheckAll;
    }
}

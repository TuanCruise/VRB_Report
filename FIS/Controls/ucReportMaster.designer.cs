namespace FIS.AppClient.Controls
{
    partial class ucReportMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucReportMaster));
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.reportLayout = new DevExpress.XtraLayout.LayoutControl();
            this.reportLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.btnRepair = new DevExpress.XtraEditors.SimpleButton();
            this.btnView = new DevExpress.XtraEditors.SimpleButton();
            this.btnReport = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportLayout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportLayoutGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.lbTitle.Appearance.Options.UseBackColor = true;
            this.lbTitle.Appearance.Options.UseFont = true;
            this.lbTitle.Appearance.Options.UseForeColor = true;
            this.lbTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.lbTitle.Size = new System.Drawing.Size(700, 50);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "lbTitle";
            // 
            // reportLayout
            // 
            this.reportLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportLayout.Location = new System.Drawing.Point(5, 56);
            this.reportLayout.Name = "reportLayout";
            this.reportLayout.Root = this.reportLayoutGroup;
            this.reportLayout.Size = new System.Drawing.Size(691, 284);
            this.reportLayout.TabIndex = 9;
            this.reportLayout.Text = "reportLayout";
            // 
            // reportLayoutGroup
            // 
            this.reportLayoutGroup.CustomizationFormText = "reportLayoutGroup";
            this.reportLayoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.reportLayoutGroup.GroupBordersVisible = false;
            this.reportLayoutGroup.Name = "reportLayoutGroup";
            this.reportLayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.reportLayoutGroup.Size = new System.Drawing.Size(691, 284);
            this.reportLayoutGroup.TextVisible = false;
            // 
            // btnRepair
            // 
            this.btnRepair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRepair.ImageOptions.Image = global::FIS.AppClient.Properties.Resources.report_edit;
            this.btnRepair.Location = new System.Drawing.Point(5, 351);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.Size = new System.Drawing.Size(91, 27);
            this.btnRepair.TabIndex = 10;
            this.btnRepair.Text = "btnRepair";
            this.btnRepair.Click += new System.EventHandler(this.btnRepair_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.ImageOptions.Image = global::FIS.AppClient.Properties.Resources.report_magnify;
            this.btnView.Location = new System.Drawing.Point(508, 351);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(91, 27);
            this.btnView.TabIndex = 8;
            this.btnView.Text = "btnView";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReport.ImageOptions.Image = global::FIS.AppClient.Properties.Resources.report_add;
            this.btnReport.Location = new System.Drawing.Point(605, 351);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(91, 27);
            this.btnReport.TabIndex = 7;
            this.btnReport.Text = "btnReport";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.ImageOptions.Image")));
            this.btnExport.Location = new System.Drawing.Point(411, 351);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(91, 27);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "btnExport";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ucReportMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnRepair);
            this.Controls.Add(this.reportLayout);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.lbTitle);
            this.Name = "ucReportMaster";
            this.Size = new System.Drawing.Size(700, 387);
            this.Load += new System.EventHandler(this.ucReportMaster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportLayout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportLayoutGroup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraEditors.SimpleButton btnView;
        private DevExpress.XtraEditors.SimpleButton btnReport;
        private DevExpress.XtraLayout.LayoutControl reportLayout;
        private DevExpress.XtraLayout.LayoutControlGroup reportLayoutGroup;
        private DevExpress.XtraEditors.SimpleButton btnRepair;
        private DevExpress.XtraEditors.SimpleButton btnExport;
    }
}

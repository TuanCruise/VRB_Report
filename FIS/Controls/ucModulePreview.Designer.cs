namespace FIS.AppClient.Controls
{
    partial class ucModulePreview
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
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.moduleIcon = new System.Windows.Forms.PictureBox();
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.previewBox = new DevExpress.XtraEditors.PanelControl();
            this.moduleSpy = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.moduleIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.previewBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AllowHtmlString = true;
            this.lbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbTitle.Location = new System.Drawing.Point(57, 3);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(327, 31);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Click += new System.EventHandler(this.ucModulePreview_Click);
            // 
            // moduleIcon
            // 
            this.moduleIcon.Location = new System.Drawing.Point(5, 6);
            this.moduleIcon.Name = "moduleIcon";
            this.moduleIcon.Size = new System.Drawing.Size(48, 48);
            this.moduleIcon.TabIndex = 0;
            this.moduleIcon.TabStop = false;
            this.moduleIcon.Click += new System.EventHandler(this.ucModulePreview_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(57, 36);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(327, 18);
            this.progressBar.TabIndex = 2;
            this.progressBar.Click += new System.EventHandler(this.ucModulePreview_Click);
            // 
            // previewBox
            // 
            this.previewBox.Controls.Add(this.lbTitle);
            this.previewBox.Controls.Add(this.progressBar);
            this.previewBox.Controls.Add(this.moduleIcon);
            this.previewBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewBox.Location = new System.Drawing.Point(2, 2);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(389, 61);
            this.previewBox.TabIndex = 3;
            this.previewBox.Click += new System.EventHandler(this.ucModulePreview_Click);
            // 
            // moduleSpy
            // 
            this.moduleSpy.DoWork += new System.ComponentModel.DoWorkEventHandler(this.moduleSpy_DoWork);
            // 
            // ucModulePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewBox);
            this.Name = "ucModulePreview";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(393, 65);
            this.Click += new System.EventHandler(this.ucModulePreview_Click);
            ((System.ComponentModel.ISupportInitialize)(this.moduleIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.previewBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox moduleIcon;
        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraEditors.ProgressBarControl progressBar;
        private DevExpress.XtraEditors.PanelControl previewBox;
        private System.ComponentModel.BackgroundWorker moduleSpy;
    }
}

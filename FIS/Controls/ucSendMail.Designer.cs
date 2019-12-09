namespace FIS.AppClient.Controls
{
    partial class ucSendMail
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
            this.mainLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.mainLayout = new DevExpress.XtraLayout.LayoutControl();
            this.lnkFile = new DevExpress.XtraEditors.HyperLinkEdit();
            this.btnSendMail = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lnkFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.lbTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new System.Windows.Forms.Padding(23, 0, 23, 0);
            this.lbTitle.Size = new System.Drawing.Size(532, 58);
            this.lbTitle.TabIndex = 5;
            this.lbTitle.Text = "lbTitle";
            // 
            // mainLayoutGroup
            // 
            this.mainLayoutGroup.CustomizationFormText = "sendMailLayoutGroup";
            this.mainLayoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.mainLayoutGroup.GroupBordersVisible = false;
            this.mainLayoutGroup.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutGroup.Name = "mainLayoutGroup";
            this.mainLayoutGroup.Size = new System.Drawing.Size(532, 391);
            this.mainLayoutGroup.Text = "mainLayoutGroup";
            this.mainLayoutGroup.TextVisible = false;
            // 
            // mainLayout
            // 
            this.mainLayout.AllowCustomizationMenu = false;
            this.mainLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainLayout.Location = new System.Drawing.Point(0, 58);
            this.mainLayout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(494, 160, 250, 350);
            this.mainLayout.Root = this.mainLayoutGroup;
            this.mainLayout.Size = new System.Drawing.Size(532, 391);
            this.mainLayout.TabIndex = 6;
            this.mainLayout.Text = "layoutControl1";
            // 
            // lnkFile
            // 
            this.lnkFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkFile.Location = new System.Drawing.Point(14, 460);
            this.lnkFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lnkFile.Name = "lnkFile";
            this.lnkFile.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lnkFile.Properties.Appearance.Options.UseBackColor = true;
            this.lnkFile.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lnkFile.Properties.Image = global::FIS.AppClient.Properties.Resources.ATTACH;
            this.lnkFile.Properties.ImageAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lnkFile.Size = new System.Drawing.Size(23, 20);
            this.lnkFile.TabIndex = 104;
            this.lnkFile.OpenLink += new DevExpress.XtraEditors.Controls.OpenLinkEventHandler(this.lnkFile_OpenLink);
            // 
            // btnSendMail
            // 
            this.btnSendMail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendMail.Appearance.Image = global::FIS.AppClient.Properties.Resources.MAIL;
            this.btnSendMail.Appearance.Options.UseImage = true;
            this.btnSendMail.Image = global::FIS.AppClient.Properties.Resources.SEND_MAIL;
            this.btnSendMail.Location = new System.Drawing.Point(430, 457);
            this.btnSendMail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(87, 28);
            this.btnSendMail.TabIndex = 7;
            this.btnSendMail.Text = "Gửi Mail";
            this.btnSendMail.Click += new System.EventHandler(this.btnSendMail_Click);
            // 
            // ucSendMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkFile);
            this.Controls.Add(this.btnSendMail);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.lbTitle);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ucSendMail";
            this.Size = new System.Drawing.Size(532, 501);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayoutGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lnkFile.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraEditors.SimpleButton btnSendMail;
        private DevExpress.XtraLayout.LayoutControlGroup mainLayoutGroup;
        private DevExpress.XtraLayout.LayoutControl mainLayout;
        private DevExpress.XtraEditors.HyperLinkEdit lnkFile;
    }
}

namespace FIS.AppClient.Controls
{
    partial class ucShowDataFlow
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
            this.reMain = new FIS.AppClient.Controls.ucRelationEditor();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.btnGenerate = new DevExpress.XtraEditors.SimpleButton();
            this.cboStoresName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnDesign = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.xtraScrollableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboStoresName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // reMain
            // 
            this.reMain.BeginPaintObjects = null;
            this.reMain.Cursor = System.Windows.Forms.Cursors.Cross;
            this.reMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.reMain.EndPaintObjects = null;
            this.reMain.Location = new System.Drawing.Point(0, 0);
            this.reMain.Name = "reMain";
            this.reMain.Size = new System.Drawing.Size(642, 118);
            this.reMain.TabIndex = 0;
            this.reMain.UpdateConnect += new FIS.AppClient.Controls.ucRelationEditor.UpdateConnectEventHandler(this.reMain_UpdateConnect);
            this.reMain.ObjectTitleClicked += new FIS.AppClient.Controls.ucRelationEditor.ItemClickedEventHandler(this.reMain_ObjectTitleClicked);
            this.reMain.ItemClicked += new FIS.AppClient.Controls.ucRelationEditor.ItemClickedEventHandler(this.reMain_ItemClicked);
            this.reMain.ConnectChange += new FIS.AppClient.Controls.ucRelationEditor.UpdateConnectEventHandler(this.reMain_ConnectChange);
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraScrollableControl1.Controls.Add(this.reMain);
            this.xtraScrollableControl1.Location = new System.Drawing.Point(3, 49);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(642, 167);
            this.xtraScrollableControl1.TabIndex = 3;
            // 
            // lbTitle
            // 
            this.lbTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold);
            this.lbTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.lbTitle.Size = new System.Drawing.Size(645, 50);
            this.lbTitle.TabIndex = 4;
            this.lbTitle.Text = "labelControl1";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Image = global::FIS.AppClient.Properties.Resources.Database;
            this.btnGenerate.Location = new System.Drawing.Point(501, 222);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(66, 23);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "&Compile";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cboStoresName
            // 
            this.cboStoresName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboStoresName.Location = new System.Drawing.Point(12, 225);
            this.cboStoresName.Name = "cboStoresName";
            this.cboStoresName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStoresName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboStoresName.Size = new System.Drawing.Size(221, 20);
            this.cboStoresName.TabIndex = 6;
            // 
            // btnDesign
            // 
            this.btnDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDesign.Image = global::FIS.AppClient.Properties.Resources.RUN_SQL;
            this.btnDesign.Location = new System.Drawing.Point(239, 222);
            this.btnDesign.Name = "btnDesign";
            this.btnDesign.Size = new System.Drawing.Size(91, 23);
            this.btnDesign.TabIndex = 7;
            this.btnDesign.Text = "Design SQL";
            this.btnDesign.Click += new System.EventHandler(this.btnDesign_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Image = global::FIS.AppClient.Properties.Resources.HOME;
            this.btnClose.Location = new System.Drawing.Point(573, 222);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(65, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ucShowDataFlow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDesign);
            this.Controls.Add(this.cboStoresName);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.xtraScrollableControl1);
            this.Name = "ucShowDataFlow";
            this.Size = new System.Drawing.Size(645, 256);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.xtraScrollableControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboStoresName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ucRelationEditor reMain;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.XtraEditors.SimpleButton btnGenerate;
        private DevExpress.XtraEditors.ComboBoxEdit cboStoresName;
        private DevExpress.XtraEditors.SimpleButton btnDesign;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}

namespace FIS.AppClient.Controls
{
    partial class ucSQLModel
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
            this.reMain = new FIS.AppClient.Controls.ucRelationEditor();
            this.txtStoreSource = new DevExpress.XtraEditors.MemoEdit();
            this.cboTablesName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboScriptType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnAppend = new DevExpress.XtraEditors.SimpleButton();
            this.btnCompile = new DevExpress.XtraEditors.SimpleButton();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.btnAutoMatch = new DevExpress.XtraEditors.SimpleButton();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.txtFormatName = new DevExpress.XtraEditors.TextEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStoreSource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTablesName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboScriptType.Properties)).BeginInit();
            this.xtraScrollableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFormatName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold);
            this.lbTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.lbTitle.Size = new System.Drawing.Size(931, 50);
            this.lbTitle.TabIndex = 5;
            this.lbTitle.Text = "labelControl1";
            // 
            // reMain
            // 
            this.reMain.Appearance.BorderColor = System.Drawing.Color.LightGray;
            this.reMain.Appearance.Options.UseBorderColor = true;
            this.reMain.BeginPaintObjects = null;
            this.reMain.Cursor = System.Windows.Forms.Cursors.Cross;
            this.reMain.EndPaintObjects = null;
            this.reMain.Location = new System.Drawing.Point(0, 0);
            this.reMain.Name = "reMain";
            this.reMain.Size = new System.Drawing.Size(525, 220);
            this.reMain.TabIndex = 6;
            this.reMain.UpdateConnect += new FIS.AppClient.Controls.ucRelationEditor.UpdateConnectEventHandler(this.reMain_UpdateConnect);
            this.reMain.ItemClicked += new FIS.AppClient.Controls.ucRelationEditor.ItemClickedEventHandler(this.reMain_ItemClicked);
            this.reMain.ConnectChange += new FIS.AppClient.Controls.ucRelationEditor.UpdateConnectEventHandler(this.reMain_ConnectChange);
            // 
            // txtStoreSource
            // 
            this.txtStoreSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStoreSource.Location = new System.Drawing.Point(534, 82);
            this.txtStoreSource.Name = "txtStoreSource";
            this.txtStoreSource.Properties.AcceptsTab = true;
            this.txtStoreSource.Properties.Appearance.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStoreSource.Properties.Appearance.Options.UseFont = true;
            this.txtStoreSource.Properties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStoreSource.Properties.WordWrap = false;
            this.txtStoreSource.Size = new System.Drawing.Size(394, 282);
            this.txtStoreSource.TabIndex = 7;
            // 
            // cboTablesName
            // 
            this.cboTablesName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTablesName.Location = new System.Drawing.Point(33, 56);
            this.cboTablesName.Name = "cboTablesName";
            this.cboTablesName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboTablesName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboTablesName.Size = new System.Drawing.Size(419, 20);
            this.cboTablesName.TabIndex = 8;
            this.cboTablesName.SelectedValueChanged += new System.EventHandler(this.cboTablesName_SelectedValueChanged);
            // 
            // cboScriptType
            // 
            this.cboScriptType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboScriptType.EditValue = "SELECT * Query";
            this.cboScriptType.Location = new System.Drawing.Point(534, 56);
            this.cboScriptType.Name = "cboScriptType";
            this.cboScriptType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboScriptType.Properties.Items.AddRange(new object[] {
            "SELECT * Query",
            "SELECT Query",
            "INSERT Query",
            "DELETE Query",
            "UPDATE Query",
            "REPLACE Query"});
            this.cboScriptType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboScriptType.Size = new System.Drawing.Size(275, 20);
            this.cboScriptType.TabIndex = 9;
            // 
            // btnAppend
            // 
            this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAppend.Image = global::FIS.AppClient.Properties.Resources.PAGE;
            this.btnAppend.Location = new System.Drawing.Point(843, 56);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(85, 20);
            this.btnAppend.TabIndex = 10;
            this.btnAppend.Text = "Append";
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompile.Image = global::FIS.AppClient.Properties.Resources.Database;
            this.btnCompile.Location = new System.Drawing.Point(781, 370);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(70, 23);
            this.btnCompile.TabIndex = 11;
            this.btnCompile.Text = "&Compile";
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraScrollableControl1.Controls.Add(this.reMain);
            this.xtraScrollableControl1.Location = new System.Drawing.Point(3, 84);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(525, 280);
            this.xtraScrollableControl1.TabIndex = 12;
            // 
            // btnAutoMatch
            // 
            this.btnAutoMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoMatch.Image = global::FIS.AppClient.Properties.Resources.STATISTICS;
            this.btnAutoMatch.Location = new System.Drawing.Point(458, 56);
            this.btnAutoMatch.Name = "btnAutoMatch";
            this.btnAutoMatch.Size = new System.Drawing.Size(70, 20);
            this.btnAutoMatch.TabIndex = 13;
            this.btnAutoMatch.Text = "Match";
            this.btnAutoMatch.Click += new System.EventHandler(this.btnAutoMatch_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = global::FIS.AppClient.Properties.Resources.Refresh;
            this.btnRefresh.Location = new System.Drawing.Point(815, 56);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(22, 20);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "simpleButton1";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtFormatName
            // 
            this.txtFormatName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormatName.EditValue = "{0}";
            this.txtFormatName.Location = new System.Drawing.Point(534, 372);
            this.txtFormatName.Name = "txtFormatName";
            this.txtFormatName.Size = new System.Drawing.Size(241, 20);
            this.txtFormatName.TabIndex = 15;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Image = global::FIS.AppClient.Properties.Resources.HOME;
            this.btnClose.Location = new System.Drawing.Point(857, 370);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(65, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ucSQLModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtFormatName);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnAutoMatch);
            this.Controls.Add(this.xtraScrollableControl1);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.btnAppend);
            this.Controls.Add(this.cboScriptType);
            this.Controls.Add(this.cboTablesName);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.txtStoreSource);
            this.Name = "ucSQLModel";
            this.Size = new System.Drawing.Size(931, 403);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStoreSource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTablesName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboScriptType.Properties)).EndInit();
            this.xtraScrollableControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFormatName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbTitle;
        private ucRelationEditor reMain;
        private DevExpress.XtraEditors.MemoEdit txtStoreSource;
        private DevExpress.XtraEditors.ComboBoxEdit cboTablesName;
        private DevExpress.XtraEditors.ComboBoxEdit cboScriptType;
        private DevExpress.XtraEditors.SimpleButton btnAppend;
        private DevExpress.XtraEditors.SimpleButton btnCompile;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private DevExpress.XtraEditors.SimpleButton btnAutoMatch;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.TextEdit txtFormatName;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}

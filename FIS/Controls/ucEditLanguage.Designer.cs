namespace FIS.AppClient.Controls
{
    partial class ucEditLanguage
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
            this.mainGrid = new DevExpress.XtraGrid.GridControl();
            this.mainView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colLangID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLangName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLangValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.chkIcon = new DevExpress.XtraEditors.CheckEdit();
            this.chkTip = new DevExpress.XtraEditors.CheckEdit();
            this.chkHotkey = new DevExpress.XtraEditors.CheckEdit();
            this.chkModType = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIcon.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHotkey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkModType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // mainGrid
            // 
            this.mainGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainGrid.Location = new System.Drawing.Point(3, 3);
            this.mainGrid.MainView = this.mainView;
            this.mainGrid.Name = "mainGrid";
            this.mainGrid.Size = new System.Drawing.Size(573, 362);
            this.mainGrid.TabIndex = 0;
            this.mainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainView});
            // 
            // mainView
            // 
            this.mainView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colLangID,
            this.colLangName,
            this.colLangValue});
            this.mainView.GridControl = this.mainGrid;
            this.mainView.Name = "mainView";
            this.mainView.OptionsView.ShowAutoFilterRow = true;
            this.mainView.OptionsView.ShowGroupPanel = false;
            // 
            // colLangID
            // 
            this.colLangID.Caption = "LangID";
            this.colLangID.FieldName = "LANGID";
            this.colLangID.Name = "colLangID";
            this.colLangID.OptionsColumn.AllowEdit = false;
            this.colLangID.OptionsColumn.AllowFocus = false;
            this.colLangID.Visible = true;
            this.colLangID.VisibleIndex = 0;
            this.colLangID.Width = 49;
            // 
            // colLangName
            // 
            this.colLangName.Caption = "Lang Name";
            this.colLangName.FieldName = "LANGNAME";
            this.colLangName.Name = "colLangName";
            this.colLangName.OptionsColumn.AllowEdit = false;
            this.colLangName.OptionsColumn.AllowFocus = false;
            this.colLangName.OptionsColumn.FixedWidth = true;
            this.colLangName.Visible = true;
            this.colLangName.VisibleIndex = 1;
            this.colLangName.Width = 250;
            // 
            // colLangValue
            // 
            this.colLangValue.Caption = "Lang Value";
            this.colLangValue.FieldName = "LANGVALUE";
            this.colLangValue.Name = "colLangValue";
            this.colLangValue.Visible = true;
            this.colLangValue.VisibleIndex = 2;
            this.colLangValue.Width = 253;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::FIS.AppClient.Properties.Resources.SAVE;
            this.btnSave.Location = new System.Drawing.Point(503, 371);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(73, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = global::FIS.AppClient.Properties.Resources.Refresh;
            this.btnRefresh.Location = new System.Drawing.Point(422, 371);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkIcon
            // 
            this.chkIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIcon.Location = new System.Drawing.Point(3, 371);
            this.chkIcon.Name = "chkIcon";
            this.chkIcon.Properties.Caption = "Icon";
            this.chkIcon.Size = new System.Drawing.Size(75, 19);
            this.chkIcon.TabIndex = 3;
            // 
            // chkTip
            // 
            this.chkTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkTip.Location = new System.Drawing.Point(84, 371);
            this.chkTip.Name = "chkTip";
            this.chkTip.Properties.Caption = "Tip";
            this.chkTip.Size = new System.Drawing.Size(75, 19);
            this.chkTip.TabIndex = 3;
            // 
            // chkHotkey
            // 
            this.chkHotkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHotkey.Location = new System.Drawing.Point(165, 371);
            this.chkHotkey.Name = "chkHotkey";
            this.chkHotkey.Properties.Caption = "HotKey";
            this.chkHotkey.Size = new System.Drawing.Size(75, 19);
            this.chkHotkey.TabIndex = 3;
            // 
            // chkModType
            // 
            this.chkModType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkModType.Location = new System.Drawing.Point(246, 371);
            this.chkModType.Name = "chkModType";
            this.chkModType.Properties.Caption = "Modtype Language";
            this.chkModType.Size = new System.Drawing.Size(137, 19);
            this.chkModType.TabIndex = 4;
            // 
            // ucEditLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkModType);
            this.Controls.Add(this.chkHotkey);
            this.Controls.Add(this.chkTip);
            this.Controls.Add(this.chkIcon);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.mainGrid);
            this.Name = "ucEditLanguage";
            this.Size = new System.Drawing.Size(579, 397);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIcon.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHotkey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkModType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl mainGrid;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraGrid.Columns.GridColumn colLangID;
        private DevExpress.XtraGrid.Columns.GridColumn colLangName;
        private DevExpress.XtraGrid.Columns.GridColumn colLangValue;
        private DevExpress.XtraEditors.CheckEdit chkIcon;
        private DevExpress.XtraEditors.CheckEdit chkTip;
        private DevExpress.XtraEditors.CheckEdit chkHotkey;
        private DevExpress.XtraEditors.CheckEdit chkModType;
        public DevExpress.XtraGrid.Views.Grid.GridView mainView;
    }
}

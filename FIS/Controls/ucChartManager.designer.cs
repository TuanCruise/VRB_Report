namespace FIS.AppClient.Controls
{
    partial class ucChartManager
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            this.lbTitle = new DevExpress.XtraEditors.LabelControl();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.chartLayout = new DevExpress.XtraLayout.LayoutControl();
            this.btnCommit = new DevExpress.XtraEditors.SimpleButton();
            this.chartMain = new DevExpress.XtraCharts.ChartControl();
            this.chkLabel = new DevExpress.XtraEditors.CheckEdit();
            this.chkDate = new DevExpress.XtraEditors.CheckEdit();
            this.btnCandleStick = new DevExpress.XtraEditors.PictureEdit();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnStock = new DevExpress.XtraEditors.PictureEdit();
            this.chartLayoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLayout)).BeginInit();
            this.chartLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLabel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCandleStick.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStock.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLayoutGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold);
            this.lbTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.lbTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lbTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.lbTitle.Size = new System.Drawing.Size(831, 50);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "lbTitle";
            // 
            // chartLayout
            // 
            this.chartLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chartLayout.Controls.Add(this.btnCommit);
            this.chartLayout.Controls.Add(this.chartMain);
            this.chartLayout.Controls.Add(this.chkLabel);
            this.chartLayout.Controls.Add(this.chkDate);
            this.chartLayout.Controls.Add(this.btnCandleStick);
            this.chartLayout.Controls.Add(this.btnExport);
            this.chartLayout.Controls.Add(this.btnStock);
            this.chartLayout.Location = new System.Drawing.Point(2, 58);
            this.chartLayout.Margin = new System.Windows.Forms.Padding(0);
            this.chartLayout.Name = "chartLayout";
            this.chartLayout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(534, 216, 250, 350);
            this.chartLayout.Root = this.chartLayoutGroup;
            this.chartLayout.Size = new System.Drawing.Size(832, 520);
            this.chartLayout.TabIndex = 1;
            this.chartLayout.Text = "layoutControl1";
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommit.Image = global::FIS.AppClient.Properties.Resources.Refresh;
            this.btnCommit.Location = new System.Drawing.Point(747, 4);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(81, 22);
            this.btnCommit.StyleController = this.chartLayout;
            this.btnCommit.TabIndex = 106;
            this.btnCommit.Text = "Thực hiện";
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // chartMain
            // 
            this.chartMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chartMain.Location = new System.Drawing.Point(4, 30);
            this.chartMain.Name = "chartMain";
            this.chartMain.PaletteName = "In A Fog";
            this.chartMain.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            sideBySideBarSeriesLabel1.LineVisible = true;
            this.chartMain.SeriesTemplate.Label = sideBySideBarSeriesLabel1;
            this.chartMain.Size = new System.Drawing.Size(824, 486);
            this.chartMain.TabIndex = 107;
            this.chartMain.ObjectHotTracked += new DevExpress.XtraCharts.HotTrackEventHandler(this.chartMain_ObjectHotTracked);
            // 
            // chkLabel
            // 
            this.chkLabel.Location = new System.Drawing.Point(263, 4);
            this.chkLabel.Name = "chkLabel";
            this.chkLabel.Properties.Caption = "Hiển thị giá trị";
            this.chkLabel.Size = new System.Drawing.Size(108, 19);
            this.chkLabel.StyleController = this.chartLayout;
            this.chkLabel.TabIndex = 108;
            // 
            // chkDate
            // 
            this.chkDate.EditValue = true;
            this.chkDate.Location = new System.Drawing.Point(4, 4);
            this.chkDate.Name = "chkDate";
            this.chkDate.Properties.Caption = "Hiển thị thời gian";
            this.chkDate.Size = new System.Drawing.Size(255, 19);
            this.chkDate.StyleController = this.chartLayout;
            this.chkDate.TabIndex = 109;
            // 
            // btnCandleStick
            // 
            this.btnCandleStick.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCandleStick.EditValue = global::FIS.AppClient.Properties.Resources.candle_u;
            this.btnCandleStick.Location = new System.Drawing.Point(636, 4);
            this.btnCandleStick.Name = "btnCandleStick";
            this.btnCandleStick.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnCandleStick.Properties.Appearance.Options.UseBackColor = true;
            this.btnCandleStick.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnCandleStick.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.btnCandleStick.Size = new System.Drawing.Size(32, 22);
            this.btnCandleStick.StyleController = this.chartLayout;
            this.btnCandleStick.TabIndex = 111;
            this.btnCandleStick.Visible = false;
            this.btnCandleStick.Click += new System.EventHandler(this.btnCandleStick_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = global::FIS.AppClient.Properties.Resources.EXPORT;
            this.btnExport.Location = new System.Drawing.Point(672, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(71, 22);
            this.btnExport.StyleController = this.chartLayout;
            this.btnExport.TabIndex = 110;
            this.btnExport.Text = "Kết xuất";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnStock
            // 
            this.btnStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStock.EditValue = global::FIS.AppClient.Properties.Resources.bar_u;
            this.btnStock.Location = new System.Drawing.Point(596, 4);
            this.btnStock.Name = "btnStock";
            this.btnStock.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnStock.Properties.Appearance.Options.UseBackColor = true;
            this.btnStock.Size = new System.Drawing.Size(36, 22);
            this.btnStock.StyleController = this.chartLayout;
            this.btnStock.TabIndex = 112;
            this.btnStock.Visible = false;
            this.btnStock.Click += new System.EventHandler(this.btnStock_Click);
            // 
            // chartLayoutGroup
            // 
            this.chartLayoutGroup.CustomizationFormText = "Root";
            this.chartLayoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.chartLayoutGroup.GroupBordersVisible = false;
            this.chartLayoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem5,
            this.emptySpaceItem1,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.chartLayoutGroup.Location = new System.Drawing.Point(0, 0);
            this.chartLayoutGroup.Name = "Root";
            this.chartLayoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.chartLayoutGroup.Size = new System.Drawing.Size(832, 520);
            this.chartLayoutGroup.Text = "Root";
            this.chartLayoutGroup.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnCommit;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(743, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(85, 26);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(85, 26);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(85, 26);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.chartMain;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(828, 490);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.chkDate;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(259, 26);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.chkLabel;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(259, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(112, 26);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnExport;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(668, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(75, 26);
            this.layoutControlItem5.Text = "layoutControlItem5";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextToControlDistance = 0;
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(371, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(221, 26);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnCandleStick;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(632, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(36, 26);
            this.layoutControlItem6.Text = "layoutControlItem6";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextToControlDistance = 0;
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnStock;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem7";
            this.layoutControlItem7.Location = new System.Drawing.Point(592, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(40, 26);
            this.layoutControlItem7.Text = "layoutControlItem7";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextToControlDistance = 0;
            this.layoutControlItem7.TextVisible = false;
            // 
            // ucChartManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartLayout);
            this.Controls.Add(this.lbTitle);
            this.Name = "ucChartManager";
            this.Size = new System.Drawing.Size(831, 578);
            this.Load += new System.EventHandler(this.ucChartManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLayout)).EndInit();
            this.chartLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLabel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCandleStick.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStock.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLayoutGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbTitle;
        private DevExpress.Utils.ToolTipController toolTipController1;
        private DevExpress.XtraLayout.LayoutControl chartLayout;
        private DevExpress.XtraCharts.ChartControl chartMain;
        private DevExpress.XtraLayout.LayoutControlGroup chartLayoutGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btnCommit;
        private DevExpress.XtraEditors.CheckEdit chkLabel;
        private DevExpress.XtraEditors.CheckEdit chkDate;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.PictureEdit btnStock;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.PictureEdit btnCandleStick;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}

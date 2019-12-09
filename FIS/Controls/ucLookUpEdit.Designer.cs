namespace FIS.AppClient.Controls
{
    partial class ucLookUpEdit
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
            this.listBoxItems = new DevExpress.XtraEditors.ImageListBoxControl();
            this.btnLookUp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLookUp)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxItems
            // 
            this.listBoxItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxItems.Location = new System.Drawing.Point(0, 0);
            this.listBoxItems.Name = "listBoxItems";
            this.listBoxItems.Size = new System.Drawing.Size(511, 150);
            this.listBoxItems.TabIndex = 0;
            // 
            // btnLookUp
            // 
            this.btnLookUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLookUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLookUp.Image = global::FIS.AppClient.Properties.Resources.LookUp;
            this.btnLookUp.Location = new System.Drawing.Point(517, 130);
            this.btnLookUp.Name = "btnLookUp";
            this.btnLookUp.Size = new System.Drawing.Size(20, 20);
            this.btnLookUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnLookUp.TabIndex = 1;
            this.btnLookUp.TabStop = false;
            // 
            // ucLookUpEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLookUp);
            this.Controls.Add(this.listBoxItems);
            this.Name = "ucLookUpEdit";
            this.Size = new System.Drawing.Size(540, 150);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLookUp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ImageListBoxControl listBoxItems;
        private System.Windows.Forms.PictureBox btnLookUp;
    }
}

namespace FIS.AppClient
{
    partial class frmAddIn
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wbNDTNN = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wbNDTNN
            // 
            this.wbNDTNN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbNDTNN.Location = new System.Drawing.Point(0, 0);
            this.wbNDTNN.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbNDTNN.Name = "wbNDTNN";
            this.wbNDTNN.Size = new System.Drawing.Size(792, 570);
            this.wbNDTNN.TabIndex = 0;
            // 
            // frmAddIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 570);
            this.Controls.Add(this.wbNDTNN);
            this.Name = "frmAddIn";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gửi báo cáo NĐT NN";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbNDTNN;
    }
}
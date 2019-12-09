using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FIS.AppClient.Utils;
using FIS.Common;

namespace FIS.AppClient
{
	/// <summary>
    /// Summary description for frmAlert.
	/// </summary>
    public partial class frmAlert : XtraForm
    {
		private System.ComponentModel.IContainer components;
		int X=0;
        int Y = 0;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel lnkMessage;

        public frmAlert()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code 
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlert));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lnkMessage = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.No;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::FIS.AppClient.Properties.Resources.mail_mark_unread_new;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(168, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lnkMessage
            // 
            this.lnkMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkMessage.Location = new System.Drawing.Point(5, 69);
            this.lnkMessage.Name = "lnkMessage";
            this.lnkMessage.Size = new System.Drawing.Size(156, 23);
            this.lnkMessage.TabIndex = 2;
            this.lnkMessage.TabStop = true;
            this.lnkMessage.Text = "Bạn có thông báo mới";
            this.lnkMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkMessage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMessage_LinkClicked);
            // 
            // frmAlert
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(168, 93);
            this.Controls.Add(this.lnkMessage);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAlert";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Thông báo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion		
		
		private void Form1_Load(object sender, System.EventArgs e)
		{
			
            //X=Screen.GetWorkingArea(this).Width; 
            //Y=Screen.GetWorkingArea(this).Height; 
            //this.Location=new Point(X-this.Width,Y- this.Height);            			
		}
						

        private void lnkMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            MainProcess.ExecuteModule("03271", "MMN");
        }

        protected override void OnLoad(EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            base.OnLoad(e);
        }
	}
}

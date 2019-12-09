using FIS.AppClient.Utils;
namespace FIS.AppClient.Controls
{
    partial class ucAlertMaster
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
            MainProcess.CancelRegisterButton(StatusLed);
            base.DestroyHandle();
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.alertCtrlMain = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.SuspendLayout();
            // 
            // alertCtrlMain
            // 
            this.alertCtrlMain.AlertClick += new DevExpress.XtraBars.Alerter.AlertClickEventHandler(this.alertCtrlMain_AlertClick);
            // 
            // ucAlertMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ucAlertMaster";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Alerter.AlertControl alertCtrlMain;
    }
}

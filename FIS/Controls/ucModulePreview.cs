using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using FIS.Utils;
using FIS.AppClient.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucModulePreview : XtraUserControl
    {
        public ucModule Module { get; set; }
        public frmModuleBox ModuleForm { get; set; }

        public ucModulePreview()
        {
            InitializeComponent();
        }

        private void ucModulePreview_Click(object sender, EventArgs e)
        {
            if (ModuleForm != null && !ModuleForm.IsDisposed)
            {
                try
                {
                    ModuleForm.ShowInTaskbar = false;
                    ModuleForm.Show(MainProcess.GetMainForm());
                    ModuleForm.WindowState = FormWindowState.Normal;
                }
                catch
                {                    
                }
                finally
                {
                    ModuleForm.ucModule.ucPreview = null;
                }
            }

            MainProcess.RemoveModulePreview(this);
        }

        public delegate void RefreshPreviewInvoker();

        public void RefreshPreview()
        {
            if(InvokeRequired)
            {
                Invoke(new RefreshPreviewInvoker(RefreshPreview));
                return;
            }

            if (!InvokeRequired)
            {
                if (ModuleForm != null)
                {
                    if (Module.CurrentThread != null)
                    {
                        lbTitle.Text = string.Format("<b>{0}</b>\r\n{1}", ModuleForm.Text, Module.CurrentThread.JobName);
                        progressBar.EditValue = Module.CurrentThread.PercentComplete;
                    }
                    else
                    {
                        if (Module.ParentForm != null)
                        {
                            lbTitle.Text = string.Format("<b>{0}</b>", Module.ParentForm.Text);
                            progressBar.EditValue = 100;
                        }
                    }
                }
            }
        }

        private void moduleSpy_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (!Disposing)
                {
                    Invoke(new RefreshPreviewInvoker(RefreshPreview));
                    Thread.Sleep(500);
                }
            }
            catch
            {
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshPreview();
            
            var imageName = LangUtils.TranslateModuleItem(LangType.MODULE_ICON, Module.ModuleInfo);
            moduleIcon.Image = ThemeUtils.Image48.Images[imageName];
            moduleSpy.RunWorkerAsync();
        }
    }
}

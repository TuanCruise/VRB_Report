using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FIS.Common;

namespace FIS.AppClient
{
    public partial class frmSplash : XtraForm
    {
        static frmSplash Instance;

        public frmSplash()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                lbVersion.Text = string.Format(lbVersion.Text, Application.ProductVersion);
                Image backgroundImage;
                backgroundImage = Image.FromFile("Theme\\Splash.jpg");
                Width = backgroundImage.Width + 4;
                Height = backgroundImage.Height + 4;
                BackgroundImage = backgroundImage;                
            }
            catch
            {
            }
        }

        static public void ShowSplashScreen()
        {
            new Thread(_ShowSplashScreen).Start();
        }

        [STAThread]
        static public void _ShowSplashScreen()
        {
            Instance = new frmSplash();
            Application.Run(Instance);
        }

        private delegate void CloseFormDelegate();
        private void _CloseForm()
        {
            if(InvokeRequired)
            {
                Invoke(new CloseFormDelegate(_CloseForm));
                return;
            }

            try
            {
                Instance.Close();
                Instance.Dispose();
            }
            catch
            {
            }
            finally
            {
                Instance = null;
            }
        }

        public static void CloseForm()
        {
            if(Instance != null)
                Instance._CloseForm();
        }

        private void _ChangeSplashStatus(string statusText)
        {
            if(InvokeRequired)
            {
                Invoke(new DelegateChangeStatus(_ChangeSplashStatus), statusText);
                return;
            }

            Instance.lbAction.Text = statusText;
        }

        private delegate void DelegateChangeStatus(string statusText);
        public static void ChangeSplashStatus(string statusText)
        {
            try
            {
                if (Instance != null)
                {
                    Instance._ChangeSplashStatus(statusText);
                }
            }
            catch
            {
            }
        }
    }
}
using FIS.Controllers;
using FIS.Utils;
using System;

namespace FIS.AppClient.Controls
{
    public partial class ucSystemLog : ucModule
    {
        private string m_SystemLog;
        public ucSystemLog()
        {
            InitializeComponent();
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            Execute();
        }

        void worker_DoUpdateGUI(object sender, EventArgs e)
        {
            webBrowser.DocumentText = m_SystemLog;
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Execute();
        }

        public override void LockUserAction()
        {
            base.LockUserAction();

            if (!InvokeRequired)
            {
                ShowWaitingBox();
                Enabled = false;
            }
        }

        public override void UnLockUserAction()
        {
            base.UnLockUserAction();

            if (!InvokeRequired)
            {
                HideWaitingBox();
                Enabled = true;
            }
        }

        private void btnClearLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ClearLog();
        }

        public void ClearLog()
        {
            var w = new WorkerThread(delegate (WorkerThread worker)
            {
                LockUserAction();
                try
                {
                    using (var client = new SAController())
                    {
                        m_SystemLog = client.GetSystemLog(true);
                        worker.ExecuteUpdateGUI();
                    }
                }
                catch
                {
                }
                finally
                {
                    UnLockUserAction();
                }
            }, this);
            w.DoUpdateGUI += worker_DoUpdateGUI;
            w.Start();
        }

        public override void Execute()
        {
            base.Execute();

            var w = new WorkerThread(delegate (WorkerThread worker)
            {
                LockUserAction();
                try
                {
                    using (var client = new SAController())
                    {
                        m_SystemLog = client.GetSystemLog(false);
                        worker.ExecuteUpdateGUI();
                    }
                }
                catch
                {
                }
                finally
                {
                    UnLockUserAction();
                }
            }, this);
            w.DoUpdateGUI += worker_DoUpdateGUI;
            w.Start();
        }
    }
}

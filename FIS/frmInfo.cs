using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using FIS.AppClient.Utils;
using FIS.Common;
using FIS.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;

namespace FIS.AppClient
{
    public partial class frmInfo : XtraForm
    {
        public frmInfo()
        {
            InitializeComponent();
            LookAndFeel.SkinName = UserLookAndFeel.Default.SkinName;
            LookAndFeel.UseDefaultLookAndFeel = false;
        }

        private delegate void ShowErrorInfoInvoker(string title, FaultException ex, Control owner);

        private delegate void ShowInfoInvoker(string title, string infoText, Control owner);
        public static void ShowInfo(string title, string infoText, Control owner)
        {
            if (owner == null || owner.IsDisposed || owner.Disposing)
            {
                var frmMain = MainProcess.GetMainForm();
                if (frmMain != null && !frmMain.IsDisposed && !frmMain.Disposing)
                {
                    ShowInfo(title, infoText, frmMain);
                }
                else
                {
                    EventLog.WriteEntry("IMSS.Client", infoText, EventLogEntryType.Information);
                }
                return;
            }

            if (owner.InvokeRequired)
            {
                owner.Invoke(new ShowInfoInvoker(ShowInfo), title, infoText, owner);
                return;
            }

            if (!owner.InvokeRequired)
            {
                var frmDialog = new frmInfo
                {
                    Text = title,
                    pictureBox1 = { Image = Properties.Resources.Info },
                    lbErrorInfo = { Text = infoText }
                };
                frmDialog.ShowDialog(owner);
            }
        }

        private delegate void ShowWarningInvoker(string title, string infoText, Control owner);
        public static void ShowWarning(string title, string warningText, Control owner)
        {
            if (owner == null || owner.IsDisposed || owner.Disposing)
            {
                var frmMain = MainProcess.GetMainForm();
                if (frmMain != null && !frmMain.IsDisposed && !frmMain.Disposing)
                {
                    ShowWarning(title, warningText, frmMain);
                }
                else
                {
                    EventLog.WriteEntry("IMSS.Client", warningText, EventLogEntryType.Warning);
                }
                return;
            }

            if (owner.InvokeRequired)
            {
                owner.Invoke(new ShowWarningInvoker(ShowWarning), title, warningText, owner);
                return;
            }

            if (!owner.InvokeRequired)
            {
                var frmDialog = new frmInfo
                {
                    Text = title,
                    pictureBox1 = { Image = Properties.Resources.Warning },
                    lbErrorInfo = { Text = warningText }
                };
                frmDialog.ShowDialog(owner);
            }
        }

        public static void ShowError(string title, FaultException ex, Control owner)
        {
            if (ex.Code.Name == ERR_SYSTEM.ERR_SYSTEM_MODULE_SINGLE_INSTANCE.ToString())
                return;

            if (App.Environment.ClientInfo.SessionKey != null)
            {
                if (int.Parse(ex.Code.Name) == ERR_SYSTEM.ERR_SYSTEM_SESSION_TERMINATED_BY_ADMIN ||
                    int.Parse(ex.Code.Name) == ERR_SYSTEM.ERR_SYSTEM_SESSION_NOT_EXISTS_OR_DUPLICATE ||
                    int.Parse(ex.Code.Name) == ERR_SYSTEM.ERR_SYSTEM_SESSION_TERMINATED_BY_SELF)
                {
                    MainProcess.LogoutFromSystem(false);

                    App.Environment.ClientInfo.UserName = null;
                    App.Environment.ClientInfo.SessionKey = null;

                    MainProcess.ShowLogin();
                    return;
                }
            }

            if (owner == null || owner.IsDisposed || owner.Disposing)
            {
                var frmMain = MainProcess.GetMainForm();
                if (frmMain != null && !frmMain.IsDisposed && !frmMain.Disposing)
                {
                    ShowError(title, ex, frmMain);
                }
                else
                {
                    EventLog.WriteEntry(
                        "IMSS.Client",
                        string.Format("{0}\r\n{1}", ex.ToMessage(), ex.Reason),
                        EventLogEntryType.Error);
                }
                return;
            }

            if (owner.InvokeRequired)
            {
                owner.Invoke(new ShowErrorInfoInvoker(ShowError), title, ex, owner);
                return;
            }

            if (!owner.InvokeRequired)
            {
                var frmDialog = new frmInfo
                {
                    Text = title,
                    lbErrorInfo =
                                            {
                                                Text = string.Format("<b>{0}</b>\r\n{1}", ex.ToMessage(), ex.Reason)
                                            }
                };
                if (ex.Code.Name == "101")
                {
                    try
                    {
                        File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", ex.Reason));
                    }
                    catch
                    {
                    }
                }

                frmDialog.ShowDialog(owner);
            }
        }

        public static void ShowError(string title, FaultException ex)
        {
            ShowError(title, ex, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
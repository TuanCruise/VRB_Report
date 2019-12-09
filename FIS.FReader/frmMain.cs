using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FIS.Entities;
using FIS.FReader.Entities;
using FIS.FReader.Process;


namespace FIS.FReader
{
    public partial class frmMain : XtraForm
    {
        public SAController ctrlSA;
        public txtPROCESS txtProcess;
        private Session session;
        private delegate void RefreshDelegate();
        public frmMain()
        {
            InitializeComponent();

        }


        private void frmMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
        }

        private void StartProcess<T>(ref T Process)
            where T : AbstractProcess, new()
        {
            if (Process == null || Process.ProcessState == ProcessState.Stopped)
            {
                try
                {
                    Process = new T();
                    Process.ProcessStateChange += new EventHandler(Process_ProcessStateChange);
                    Process.Start();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void StopProcess(AbstractProcess Process)
        {
            if (Process != null && Process.ProcessState != ProcessState.Stopped)
            {
                Process.Stop();
            }
        }

        void Process_ProcessStateChange(object sender, EventArgs e)
        {
            try
            {
                Invoke(new RefreshDelegate(Refresh));
            }
            catch
            {
            }
        }


        public override void Refresh()
        {
            base.Refresh();

            if (txtProcess != null)
            {
                lock (txtProcess)
                {
                    SuspendLayout();
                    //btnHNX.ImageIndex = (int)HNXProcess.ProcessState;
                    //btnHNX.Caption = string.Format("{0,-26}", HNXProcess.ProcessStatusText);
                    txtLog.Text = string.Join("\r\n", txtProcess.LogBuffer.ToArray());
                    //if (HNXProcess.ProcessState == ProcessState.Error)
                    //{
                    //    btnHNX.SuperTip = new SuperToolTip();
                    //    btnHNX.SuperTip.Items.Add(HNXProcess.LastError);
                    //}
                    ResumeLayout();
                }
            }





        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            CheckConnect();
           // StartProcess(ref txtProcess);
            //StartProcess(ref HOSEProcess);
            //StartProcess(ref UPCOMProcess);
            //StartProcess(ref EXCELProcess);
        }

        private void btnFolderBrowser_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFolderHome.Text = folderBrowserDialog1.SelectedPath.ToString();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            try
            {
                ctrlSA.CreateUserSession(out session, txtUserName.Text, txtPassWord.Text);
                FReadEntities.SetGlobalSession(session);
                txtUserName.Enabled = false;
                txtPassWord.Enabled = false;
                btnLogin.Enabled = false;
                btnStart.Enabled = true;
                btnFolderBrowser.Enabled = true;
                txtFolderHome.Enabled = true;
                txtLog.Enabled = true;
                txtLog.Properties.ReadOnly = true;
                txtLog.Text += "Đăng nhập thành công\r\n";
            }
            catch
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu","Đăng nhập");
            }
        }

        private void CheckConnect()
        {
            try
            {
                ctrlSA = new SAController(session);
                lblStatus.Text = "Đã kết nối với SERVER!";
                txtFolderHome.Text = Configs.HOMEDirectory.ToString();
                btnReconnect.Enabled = false;
                txtUserName.Enabled = true;
                txtPassWord.Enabled = true;
                btnLogin.Enabled = true;
                btnStart.Enabled = false;
                btnFolderBrowser.Enabled = false;
                txtFolderHome.Enabled = false;
                txtLog.Enabled = false;
            }
            catch
            {
                lblStatus.Text = "Chưa kết nối được với SERVER!";
                btnReconnect.Enabled = true;
                txtUserName.Enabled = false;
                txtPassWord.Enabled = false;
                btnLogin.Enabled = false;
                btnStart.Enabled = false;
                btnFolderBrowser.Enabled = false;
                txtFolderHome.Enabled = false;
                txtLog.Enabled = false;
            }
        }

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            CheckConnect();
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return))
                txtPassWord.Focus();
        }


        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return))
                Login();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtFolderHome.Text != "")
            {
                if (txtProcess == null || txtProcess.ProcessState == ProcessState.Stopped)
                {
                    FReadEntities.SetDirectory(txtFolderHome.Text);
                    txtFolderHome.Enabled = false;
                    btnFolderBrowser.Enabled = false;
                    btnStart.Text = "Dừng đọc";
                    StartProcess(ref txtProcess);   
                }
                else
                {
                    txtFolderHome.Enabled = true;
                    btnFolderBrowser.Enabled = true;
                    btnStart.Text = "Bắt đầu đọc";
                    StopProcess(txtProcess);  
                }

            } 

        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (txtProcess != null)
            {
                StopProcess(txtProcess);
            }
        }
    }
}

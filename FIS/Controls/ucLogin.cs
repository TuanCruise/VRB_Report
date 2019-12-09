using System;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using FIS.AppClient.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucLogin : ucModule
    {
        public ucLogin()
        {
            InitializeComponent();
            //if (Program.StrUserName != "" && Program.StrPassWord != "")
            //{
            //    ExecuteLogin(Program.StrUserName,Program.StrPassWord);
            //}
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();
            //edit by TrungTT - 14.12.2011 - Do not save user/pass - Requied by HNX
            /*var clientEnvironment = (App.Environment as ClientEnvironment);
            if(clientEnvironment != null)
            {
                var appRegistry = clientEnvironment.MaximusRegistry;
                txtUsername.EditValue = appRegistry.GetValue("UserName", string.Empty);
                txtPassword.EditValue = appRegistry.GetValue("Password", string.Empty);
                chkSaveLogin.Checked = (string)appRegistry.GetValue("SaveLogin", "N") == "Y";
                if(chkSaveLogin.Checked)
                {
                    btnLogin.Focus();
                    ActiveControl = btnLogin;
                }
            }*/
            //end TrungTT
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            var parentForm = Parent as XtraForm;

            if (parentForm != null)
            {
                parentForm.FormBorderStyle = FormBorderStyle.None;
                BackgroundImage = Image.FromFile("Theme\\Login.png");
                parentForm.ClientSize = BackgroundImage.Size;
                btnLogin.BackgroundImage = Image.FromFile("Theme\\btnLoginSubmit.png");
                btnClose.BackgroundImage = Image.FromFile("Theme\\btnLoginClose.png");
            }
        }

        public override void ShowModule(IWin32Window owner)
        {
            // terminal last session
            try
            {
                var clientInfos = CachedUtils.GetCacheOf<ClientInfo>("LastSession");
                if (clientInfos.Count > 0)
                {
                    App.Environment.ClientInfo.SessionKey = clientInfos[0].SessionKey;
                    using (var ctrlSA = new SAController())
                    {
                        ctrlSA.TerminalCurrentSession();
                    }
                }
            }
            catch
            {
            }
            // login
            var frmOwner = (XtraForm)Parent;
            InitializeModuleData();

            App.Environment.ClientInfo.SessionKey = null;
            frmOwner.ShowDialog(owner);

            if (App.Environment.ClientInfo.SessionKey == null)
            {
                MainProcess.Close();
            }            
        }
        private void ExecuteLogin(string strUser,string strPass)
        {
            try
            {
                using (var ctrlSA = new SAController())
                {
                    Session session;
                    ctrlSA.CreateUserSession(out session, strUser, strPass);
                    MainProcess.LoginToSystem(session);
                }
                CloseModule();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctrlSA = new SAController())
                {
                    Session session;                    
                    ctrlSA.CreateUserSession(out session, txtUsername.Text, txtPassword.Text);                                                           
                    MainProcess.LoginToSystem(session);
                    if (session.ChkLog == 1)
                    {
                        frmInfo.ShowWarning("Error System", "Tài khoản của bạn đăng nhập lần đầu hoặc mật khẩu của bạn đã hết hạn. \n Bạn nên thay đổi mật khẩu để đảm bảo an toàn bảo mật khi sử dụng hệ thống !", this);                    
                        MainProcess.ExecuteModule("02913", "MAD");
                    }                    
                }
                CloseModule();
            }
            catch (Exception ex)
            {
                ActiveControl = txtUsername;
                txtPassword.Text = "";
                //txtUsername.Focus();
                txtPassword.Focus();
                ShowError(ex);
            }
            //--End trungTT
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseModule();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    using (var ctrlSA = new SAController())
                    {
                        Session session;                        
                        ctrlSA.CreateUserSession(out session, txtUsername.Text, txtPassword.Text);
                        

                        MainProcess.LoginToSystem(session);
                        if (session.ChkLog == 1)
                        {                            
                            frmInfo.ShowWarning("Error System", "Tài khoản của bạn đăng nhập lần đầu hoặc mật khẩu của bạn đã hết hạn. \n Bạn nên thay đổi mật khẩu để đảm bảo an toàn bảo mật khi sử dụng hệ thống !", this);
                            MainProcess.ExecuteModule("02913", "MAD");
                        }    
                    }
                    CloseModule();
                }
                catch (Exception ex)
                {
                    ActiveControl = txtUsername;
                    txtPassword.Text = "";
                    //txtUsername.Focus();
                    txtPassword.Focus();
                    ShowError(ex);
                }
            }
        }

                
    }
}

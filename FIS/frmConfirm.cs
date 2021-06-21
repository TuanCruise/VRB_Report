using FIS.AppClient.Controls;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Windows.Forms;

namespace FIS.AppClient
{
    public partial class frmConfirm : DevExpress.XtraEditors.XtraForm
    {
        public ModuleInfo ModuleInfo { get; set; }
        public bool ConfirmResult { get; set; }

        public frmConfirm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (ModuleInfo != null)
            {
                Text = LangUtils.TranslateModuleItem(LangType.MODULE_TITLE, ModuleInfo);
                lbWarning.Text = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "Warning");
                btnConfirm.Text = LangUtils.TranslateModuleItem(LangType.BUTTON_CAPTION, ModuleInfo, btnConfirm.Name);
                btnClose.Text = LangUtils.TranslateModuleItem(LangType.BUTTON_CAPTION, ModuleInfo, btnClose.Name);

            }
            else
            {
                btnConfirm.Text = LangUtils.TranslateBasic("&Accept", "APP$btnAccept.Caption");
                btnClose.Text = LangUtils.TranslateBasic("&Cancel", "APP$btnClose.Caption");
            }
        }

        private void frmConfirm_Load(object sender, EventArgs e)
        {
            ActiveControl = btnConfirm;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ConfirmResult = false;
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ConfirmResult = true;
            Close();
        }

        public static bool ShowConfirm(string title, string text, IWin32Window owner)
        {
            var frmConfirm = new frmConfirm { Text = title, lbWarning = { Text = text } };
            frmConfirm.ShowDialog(owner);
            return frmConfirm.ConfirmResult;
        }

        public delegate bool ShowConfirmInvoker(string title, string text, ucModule owner);
        public static bool ShowConfirm(string title, string text, ucModule owner)
        {
            if (owner.InvokeRequired)
            {
                return (bool)owner.Invoke(new ShowConfirmInvoker(ShowConfirm), title, text, owner);
            }

            return ShowConfirm(title, text, (IWin32Window)owner);
        }
    }
}
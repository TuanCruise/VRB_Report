using System;
using System.IO;
using System.ServiceModel;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using FIS.AppClient.Interface;
using FIS.Common;
using FIS.Controllers;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucGeneratePackage : ucModule,
        ICommonFieldSupportedModule
    {
        public ucGeneratePackage()
        {
            InitializeComponent();
        }

        protected override void InitializeGUI(DevExpress.Skins.Skin skin)
        {
            base.InitializeGUI(skin);
            lbTitle.Text = Language.Title;
        }

        protected override void BuildButtons()
        {
            base.BuildButtons();
            if (ModuleInfo.UIType == CODES.DEFMOD.UITYPE.POPUP)
            {
                var frmOwner = (XtraForm) Parent;
                frmOwner.CancelButton = btnClose;
                frmOwner.AcceptButton = btnSave;

#if DEBUG
                SetupSaveLayout(mainLayout);
#endif
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseModule();
        }

        public bool ValidateRequire
        {
            get { return true; }
        }

        public LayoutControl CommonLayout
        {
            get { return mainLayout; }
        }

        public string CommonLayoutStoredData
        {
            get { return Language.Layout; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(ValidateModule())
            {
                new WorkerThread(
                    delegate
                        {
                            try
                            {
                                LockUserAction();

                                using (var client = new SAController())
                                {
                                    string generatedPackage;
                                    client.ExecuteGenerateModulePackage((string)this["C01"], out generatedPackage);

                                    File.WriteAllText((string)this["C02"], generatedPackage, Encoding.UTF8);
                                }

                                CloseModule();
                            }
                            catch (Exception ex)
                            {
                                ShowError(ex);
                                UnLockUserAction();
                            }
                        }
                    ,this).Start();
            }
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
    }
}

using DevExpress.XtraEditors;
using FIS.AppClient.Interface;
using FIS.Controllers;
using FIS.Utils;
using System;
using System.Data;
using System.IO;

namespace FIS.AppClient.Controls
{
    public partial class ucInstallPackage : ucModule,
        IParameterFieldSupportedModule
    {
        public ucInstallPackage()
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
                var frmOwner = (XtraForm)Parent;
                frmOwner.CancelButton = btnClose;
                frmOwner.AcceptButton = btnInstall;

#if DEBUG
                SetupSaveLayout(mainLayout);
#endif
            }
        }

        protected override void InitializeModuleData()
        {
            base.InitializeModuleData();

            var ds = new DataSet();
            ds.ReadXml((string)this["P01"]);

            var modules = ds.Tables["DEFMOD"].Rows;
            lstModule.ImageList = ThemeUtils.Image16;

            for (var i = 0; i < modules.Count; i++)
            {
                var codeName = CodeUtils.GetCodeName("DEFMOD", "SUBMOD", (string)modules[i]["SUBMOD"]);
                lstModule.Items.Add(modules[i]["MODNAME"], ThemeUtils.GetImage16x16Index(codeName));
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            try
            {
                btnInstall.Enabled = false;
                using (var ctrlSA = new SAController())
                {
                    ctrlSA.ExecuteInstallModule(File.ReadAllText((string)this["P01"]));
                }
                XtraMessageBox.Show("Cài đặt chức năng mới thành công vào hệ thống !", "Install Package", System.Windows.Forms.MessageBoxButtons.OK);
                CloseModule();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                btnInstall.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseModule();
        }
    }
}

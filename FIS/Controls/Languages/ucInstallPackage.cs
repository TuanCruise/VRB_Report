using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucInstallPackage
    {
        class InstallPackageLanguage : ModuleLanguage
        {
            public string Layout { get; set; }

            public InstallPackageLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new InstallPackageLanguage Language
        {
            get
            {
                return (InstallPackageLanguage)base.Language;
            }
        }
        
        public override void InitializeLanguage()
        {
            base.Language = new InstallPackageLanguage(ModuleInfo);

            Language.Layout = Language.GetLayout(null);
            base.InitializeLanguage();
        }
    }
}

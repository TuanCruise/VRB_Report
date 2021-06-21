using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucGeneratePackage
    {
        class GeneratePackageLanguage : ModuleLanguage
        {
            public string Layout { get; set; }

            public GeneratePackageLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new GeneratePackageLanguage Language
        {
            get
            {
                return (GeneratePackageLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new GeneratePackageLanguage(ModuleInfo);

            Language.Layout = Language.GetLayout(null);
            base.InitializeLanguage();
        }
    }
}

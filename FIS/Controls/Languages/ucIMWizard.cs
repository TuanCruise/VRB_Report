using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucIMWizard
    {
        class IMWizardLanguage : ModuleLanguage
        {
            public string Layout { get; set; }
            public string Info { get; set; }
            public string ReadingStatus { get; set; }
            public string ExportingStatus { get; set; }
            public IMWizardLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new IMWizardLanguage Language
        {
            get
            {
                return (IMWizardLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new IMWizardLanguage(ModuleInfo)
            {
                Info = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "Info")
            };

            Language.Layout = Language.GetLayout(null);
            Language.ReadingStatus = Language.GetSpecialStatus("Reading");
            Language.ExportingStatus = Language.GetSpecialStatus("Exporting");

            base.InitializeLanguage();
        }

    }
}

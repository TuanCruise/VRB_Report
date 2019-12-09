using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucEditLanguage
    {
        class EditLanguageLanguage : ModuleLanguage
        {
            public EditLanguageLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new EditLanguageLanguage Language
        {
            get
            {
                return (EditLanguageLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new EditLanguageLanguage(ModuleInfo);
            base.InitializeLanguage();
        }
    }
}

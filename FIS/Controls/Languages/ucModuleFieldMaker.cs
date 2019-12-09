using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucModuleFieldMaker
    {
        class ModuleFieldMakerLanguage : ModuleLanguage
        {
            public string Layout { get; set; }

            public ModuleFieldMakerLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new ModuleFieldMakerLanguage Language
        {
            get
            {
                return (ModuleFieldMakerLanguage)base.Language;
            }
        }
        
        public override void InitializeLanguage()
        {
            base.Language = new ModuleFieldMakerLanguage(ModuleInfo);
            Language.Layout = Language.GetLayout(null);
            base.InitializeLanguage();
        }
    }
}

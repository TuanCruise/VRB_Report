using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucStoreExecute
    {
        class StoreExecuteLanguage : ModuleLanguage
        {
            public string Warning { get; set; }
            public StoreExecuteLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new StoreExecuteLanguage Language
        {
            get
            {
                return (StoreExecuteLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new StoreExecuteLanguage(ModuleInfo)
            {
                Warning = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "Warning")
            };
            base.InitializeLanguage();
        }
    }
}

using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucAlertMaster
    {
        class AlertLanguage : ModuleLanguage
        {
            public string Content { get; set; }
            public string Icon { get; set; }
            public string Status { get; set; }
            public string Error { get; set; }

            public AlertLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new AlertLanguage Language
        {
            get
            {
                return (AlertLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new AlertLanguage(ModuleInfo)
            {
                Status = LangUtils.TranslateModuleItem(LangType.MODULE_STATUS, ModuleInfo),
                Content = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "Info"),
                Error = LangUtils.TranslateModuleItem(LangType.MODULE_ICON, ModuleInfo, "Error"),
                Icon = LangUtils.TranslateModuleItem(LangType.MODULE_ICON, ModuleInfo)
            };
            base.InitializeLanguage();
        }
    }
}

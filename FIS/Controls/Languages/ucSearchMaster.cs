using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucSearchMaster
    {
        class SearchLanguage : ModuleLanguage
        {
            public string Layout { get; set; }
            public string StatisticResultFormat { get; set; }
            public string StatisticFullResultFormat { get; set; }
            public string PageInfo { get; set; }
            public string MorePageInfo { get; set; }
            public string MoreRowsCaption { get; set; }

            public SearchLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new SearchLanguage Language
        {
            get
            {
                return (SearchLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new SearchLanguage(ModuleInfo)
            {
                StatisticResultFormat = LangUtils.TranslateModuleItem(LangType.MODULE_STATUS, ModuleInfo, "Result"),
                StatisticFullResultFormat = LangUtils.TranslateModuleItem(LangType.MODULE_STATUS, ModuleInfo, "FullResult"),
                PageInfo = LangUtils.TranslateModuleItem(LangType.PAGE_INFO, ModuleInfo),
                MorePageInfo = LangUtils.TranslateModuleItem(LangType.PAGE_INFO, ModuleInfo, "More")
            };

            Language.FormatButton(btnSearch);
            Language.FormatButton(btnExport);
            Language.FormatButton(btnEdit);
            Language.MoreRowsCaption = Language.GetButtonCaption("MoreRows");
            Language.Layout = Language.GetLayout(null);

            base.InitializeLanguage();
        }
    }
}

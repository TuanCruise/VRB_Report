using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucStatisticsMaster
    {
        class StatisticsMasterLanguage : ModuleLanguage
        {
            public string Layout { get; set; }
            public string GridLayout { get; set; }
            public string PageInfo { get; set; }
            public string MorePageInfo { get; set; }
            public string MoreRowsCaption { get; set; }
            public string AscendingCaption { get; set; }
            public string DescendingCaption { get; set; }
            public string NoSortCaption { get; set; }

            public StatisticsMasterLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new StatisticsMasterLanguage Language
        {
            get
            {
                return (StatisticsMasterLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new StatisticsMasterLanguage(ModuleInfo)
            {
                PageInfo = LangUtils.TranslateModuleItem(LangType.PAGE_INFO, ModuleInfo),
                MorePageInfo = LangUtils.TranslateModuleItem(LangType.PAGE_INFO, ModuleInfo, "More")
            };

            Language.Layout = Language.GetLayout(null);
            Language.GridLayout = LangUtils.TranslateModuleItem(LangType.MODULE_LAYOUT, ModuleInfo, "Grid");
            Language.MoreRowsCaption = Language.GetButtonCaption("MoreRows");
            Language.AscendingCaption = Language.GetButtonCaption("Ascending");
            Language.DescendingCaption = Language.GetButtonCaption("Descending");
            Language.NoSortCaption = Language.GetButtonCaption("NoSort");
            btnExecute.Text = Language.GetButtonCaption("btnExecute");
            btnExport.Text = Language.GetButtonCaption("btnExport");
            //btnMail.Text = Language.GetButtonCaption("btnMail");

            base.InitializeLanguage();
        }
    }
}

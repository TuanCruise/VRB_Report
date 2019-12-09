using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucChartManager
    {
        class ChartManagerLanguage : ModuleLanguage
        {
            public string Layout { get; set; }
            public ChartManagerLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new ChartManagerLanguage Language
        {
            get
            {
                return (ChartManagerLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new ChartManagerLanguage(ModuleInfo);
            Language.Layout = Language.GetLayout(null);
            base.InitializeLanguage();
        }
    }
}

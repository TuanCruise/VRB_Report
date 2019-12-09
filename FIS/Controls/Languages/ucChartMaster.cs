using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucChartMaster
    {
        class ChartMasterLanguage : ModuleLanguage
        {
            public string Layout { get; set; }

            public ChartMasterLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new ChartMasterLanguage Language
        {
            get
            {
                return (ChartMasterLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new ChartMasterLanguage(ModuleInfo);

            Language.Layout = Language.GetLayout(null);
            Language.FormatButton(btnExecute);
            
            base.InitializeLanguage();
        }
    }
}

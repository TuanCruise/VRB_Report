using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucReportMaster
    {
        class ReportMasterLanguage : ModuleLanguage
        {
            public string Layout { get; set; }
            public ReportMasterLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new ReportMasterLanguage Language
        {
            get
            {
                return (ReportMasterLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new ReportMasterLanguage(ModuleInfo);

            Language.Layout = Language.GetLayout(null);
            Language.FormatButton(btnReport, "BTN_REPORT");
            Language.FormatButton(btnRepair, "BTN_REPAIR");
            Language.FormatButton(btnView, "BTN_VIEW");
            Language.FormatButton(btnExport, "BTN_EXPORT");
            base.InitializeLanguage();
        }

    }
}

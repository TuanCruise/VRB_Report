using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucSearchExport
    {

        class SearchExportLanguage : ModuleLanguage
        {
            public string SheetName { get; set; }
            public string SaveToSheetStatus { get; set; }
            public string ApplyFormatToSheetStatus { get; set; }
            public string SaveToFileStatus { get; set; }
            public string CompletedStatus { get; set; }
            public string ExportLayout { get; set; }
            public string ErrorStatus { get; set; }
            public string BufferRowStatus { get; set; }

            public SearchExportLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new SearchExportLanguage Language
        {
            get
            {
                return (SearchExportLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new SearchExportLanguage(ModuleInfo)
            {
                SheetName = LangUtils.TranslateModuleItem(LangType.LABEL_FIELD, ModuleInfo, "SheetName"),
                ApplyFormatToSheetStatus = LangUtils.TranslateModuleItem(LangType.MODULE_STATUS, ModuleInfo, "ApplyFormatToSheet"),
            };
            Language.SaveToSheetStatus = Language.GetSpecialStatus("SaveToSheet");
            Language.SaveToFileStatus = Language.GetSpecialStatus("SaveToFile");
            Language.CompletedStatus = Language.GetSpecialStatus("Completed");
            Language.ErrorStatus = Language.GetSpecialStatus("Error");
            Language.BufferRowStatus = Language.GetSpecialStatus("Buffering");
            Language.FormatButton(btnExport);
            Language.ExportLayout = Language.GetLayout("Export");
            base.InitializeLanguage();
        }
    }
}

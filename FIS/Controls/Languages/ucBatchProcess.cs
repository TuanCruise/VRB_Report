using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucBatchProcess
    {
        class BatchProcessLanguage : ModuleLanguage
        {
            public BatchProcessLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new BatchProcessLanguage Language
        {
            get
            {
                return (BatchProcessLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new BatchProcessLanguage(ModuleInfo);
            Language.FormatButton(btnBatch);
            base.InitializeLanguage();
        }
    }
}

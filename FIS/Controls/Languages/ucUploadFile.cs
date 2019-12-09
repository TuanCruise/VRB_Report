using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucUploadFile 
    {

        class UploadFileLanguage : ModuleLanguage
        {

            public UploadFileLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }
        private new UploadFileLanguage Language
        {
            get
            {
                return (UploadFileLanguage)base.Language;
            }
        }


        public override void InitializeLanguage()
        {
         base.InitializeLanguage();
        }
    }
}

using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucLogin
    {
        class LoginLanguage : ModuleLanguage
        {
            public LoginLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        public override void InitializeLanguage()
        {
            Language = new LoginLanguage(ModuleInfo);
            base.InitializeLanguage();
        }
    }
}

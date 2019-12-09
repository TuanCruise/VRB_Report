using FIS.AppClient.Interface;
using FIS.Utils;

namespace FIS.AppClient
{
    public partial class frmMain
    {
        internal class frmMainLanguage : IMainLanguage
        {
            public string ApplicationTitle { get; set; }
            public string ExitTitle { get; set; }
            public string ExitConfirm { get; set; }
            public string LogoutTitle { get; set; }
            public string LogoutConfirm { get; set; }
        }

        internal frmMainLanguage Language { get; set; }

        public void InitializeLanguage()
        {
            Language = new frmMainLanguage
            {
                ApplicationTitle = LangUtils.TranslateBasic("APP.Title", "APP.Title"),
                ExitTitle = LangUtils.TranslateBasic("APP$EXIT.Title", "APP$EXIT.Title"),
                ExitConfirm = LangUtils.TranslateBasic("APP$EXIT.Confirm", "APP$EXIT.Confirm"),
                LogoutTitle = LangUtils.TranslateBasic("APP$EXIT.Title", "APP$EXIT.Title"),
                LogoutConfirm = LangUtils.TranslateBasic("APP$EXIT.Confirm", "APP$EXIT.Confirm")
            };
        }
    }
}

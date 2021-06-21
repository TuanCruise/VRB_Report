using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucMaintain
    {
        class MaintainLanguage : ModuleLanguage
        {
            public string AddTitle { get; set; }
            public string EditTitle { get; set; }
            public string ViewTitle { get; set; }
            public string AddLayout { get; set; }
            public string EditLayout { get; set; }
            public string ViewLayout { get; set; }
            public string SuccessStatus { get; set; }

            public MaintainLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new MaintainLanguage Language
        {
            get
            {
                return (MaintainLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new MaintainLanguage(ModuleInfo)
            {
                AddTitle = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "Add"),
                EditTitle = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "Edit"),
                ViewTitle = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "View"),
            };

            Language.SuccessStatus = Language.GetSpecialStatus("Success");
            Language.FormatButton(btnCommit, "BTN_COMMIT");
            Language.FormatButton(btnClose, "BTN_CLOSE");
            Language.AddLayout = Language.GetLayout("Add");
            Language.EditLayout = Language.GetLayout("Edit");
            Language.ViewLayout = Language.GetLayout("View");
            //TUDQ them
            try
            {
                MaintainModuleInfo moduleinfo = (MaintainModuleInfo)ModuleInfo;
                if (moduleinfo.TRANSACTION_MODE == "Y")
                {
                    btnCommit.Visible = true;
                    btnClose.Visible = true;
                    btnCommit.Text = "Duyệt";
                    btnClose.Text = "Từ chối";
                }
            }
            catch
            {

            }
            //END

            base.InitializeLanguage();
        }
    }
}

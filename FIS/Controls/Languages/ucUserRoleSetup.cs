using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucUserRoleSetup
    {
        class UserRoleSetupLanguage : ModuleLanguage
        {
            public int RoleYesImageIndex { get; set; }
            public int RoleNoImageIndex { get; set; }
            public int HightRoleYesImageIndex { get; set; }
            public int HighRoleNoImageIndex { get; set; }
            public int FolderOpenImageIndex { get; set; }
            public int FolderCloseImageIndex { get; set; }
            public string Info { get; set; }

            public UserRoleSetupLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new UserRoleSetupLanguage Language
        {
            get
            {
                return (UserRoleSetupLanguage)base.Language;
            }
        }
        
        public override void InitializeLanguage()
        {
            base.Language = new UserRoleSetupLanguage(ModuleInfo)
            {
                RoleYesImageIndex = ThemeUtils.GetImage16x16Index("ROLE[YES]"),
                RoleNoImageIndex = ThemeUtils.GetImage16x16Index("ROLE[NO]"),
                HightRoleYesImageIndex = ThemeUtils.GetImage16x16Index("HIGHROLE[YES]"),
                HighRoleNoImageIndex = ThemeUtils.GetImage16x16Index("HIGHROLE[NO]"),
                FolderOpenImageIndex = ThemeUtils.GetImage16x16Index("FOLDER[OPEN]"),
                FolderCloseImageIndex = ThemeUtils.GetImage16x16Index("FOLDER[CLOSE]"),
                Info = LangUtils.TranslateModuleItem(LangType.MODULE_TEXT, ModuleInfo, "Info")
            };
            base.InitializeLanguage();
        }
    }
}

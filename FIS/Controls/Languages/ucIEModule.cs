using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucIEModule
    {
        class IEModuleLanguage : ModuleLanguage
        {
            public int ModuleImageIndex { get; set; }
            public int ModuleTypeImageIndex { get; set; }

            public IEModuleLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new IEModuleLanguage Language
        {
            get
            {
                return (IEModuleLanguage)base.Language;
            }
        }
        
        public override void InitializeLanguage()
        {
            base.Language = new IEModuleLanguage(ModuleInfo)
            {
                ModuleImageIndex = ThemeUtils.GetImage24x24Index("MODULE"),
                ModuleTypeImageIndex = ThemeUtils.GetImage24x24Index("MODULE_TYPE")
            };
            base.InitializeLanguage();
        }
    }
}

using FIS.Entities;

namespace FIS.AppClient.Controls
{
    public partial class ucTreeView
    {
        class TreeViewLanguage : ModuleLanguage
        {
            public string NAME { get; set; }
            public string VALUE { get; set; }
            public TreeViewLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new TreeViewLanguage Language
        {
            get
            {
                return (TreeViewLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new TreeViewLanguage(ModuleInfo)
            {

            };
            base.InitializeLanguage();
        }
    }
}

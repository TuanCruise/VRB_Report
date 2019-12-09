using FIS.Entities;
using FIS.AppClient.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucExpression
    {
        class ExpressionLanguage : ModuleLanguage
        {
            public ExpressionLanguage(ModuleInfo moduleInfo)
                : base(moduleInfo)
            {
            }
        }

        private new ExpressionLanguage Language
        {
            get
            {
                return (ExpressionLanguage)base.Language;
            }
        }

        public override void InitializeLanguage()
        {
            base.Language = new ExpressionLanguage(ModuleInfo)
            {
            };
            base.InitializeLanguage();
        }
    }
}

using System.ComponentModel;
using System.Configuration.Install;

namespace FIS.Service
{
    [RunInstaller(true)]
    public partial class FISServiceInstaller : Installer
    {
        public FISServiceInstaller()
        {
            InitializeComponent();
        }
    }
}

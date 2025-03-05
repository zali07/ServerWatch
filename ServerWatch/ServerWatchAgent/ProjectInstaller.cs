using System.ComponentModel;
using System.ServiceProcess;

namespace ServerWatchAgent
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;

            serviceInstaller1.ServiceName = "ServerWatchAgent";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
        }
    }
}

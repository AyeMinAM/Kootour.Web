using System.ComponentModel;
using System.ServiceProcess;

namespace MVCSite.EmailService
{
    [RunInstaller(true)]
    public sealed class EmailServiceInstaller : ServiceInstaller
    {
        public EmailServiceInstaller()
        {
            const string name = "MVCSite Email Sender";
            this.Description = name;
            this.DisplayName = name;
            this.ServiceName = name;
            this.StartType = ServiceStartMode.Automatic;
        }
    }

    [RunInstaller(true)]
    public sealed class MyServiceInstallerProcess : ServiceProcessInstaller
    {
        public MyServiceInstallerProcess()
        {
            this.Account = ServiceAccount.NetworkService;
        }
    }
}
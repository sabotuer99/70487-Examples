using System.ServiceModel;
using System.ServiceProcess;

namespace WindowsServiceHosted
{
    public class EchoWindowsService : ServiceBase
    {
        public ServiceHost serviceHost = null;
        public EchoWindowsService()
        {
            ServiceName = "EchoService";
        }

        public static void Main()
        {
            Run(new EchoWindowsService());
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            serviceHost = new ServiceHost(typeof(EchoService));
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}

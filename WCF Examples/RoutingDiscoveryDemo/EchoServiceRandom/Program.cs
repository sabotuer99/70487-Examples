using RoutingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;

namespace PrintService15560
{
    class Program
    {
        static void Main(string[] args)
        {

            string uriString = "http://localhost:"+ new Random().Next(2000, 64000) +"/echo";
            Uri baseAddress = new Uri(uriString);

            using (ServiceHost host = new ServiceHost(typeof(EchoService), baseAddress))
            {

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = new Uri(host.BaseAddresses[0] + "/mex");
                host.Description.Behaviors.Add(smb);

                host.AddServiceEndpoint(typeof(IEchoService),
                                        new BasicHttpBinding(),
                                        "");

                //Make host discoverable with built in behavior, and add endpoint
                host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
                host.AddServiceEndpoint(new UdpDiscoveryEndpoint());

                host.Open();
                PrintServiceDescription(host);
                Console.ReadKey();
            }
        }

        private static void PrintServiceDescription(ServiceHost host)
        {
            Console.WriteLine("{0} is up and running with the following endpoints:",
                host.Description.ServiceType.Name);
            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                Console.WriteLine("Address: {0}  ({1})",
                    endpoint.Address.ToString(), endpoint.Binding.Name);
            }
        }
    }
}

using ServiceShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ServiceVersion3
{
    class Program
    {
        static void Main(string[] args)
        {
                using (ServiceHost host = new ServiceHost(typeof(Service),
                                                            new Uri("http://localhost:10000")))
                {
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    //smb.HttpGetUrl = new Uri(host.BaseAddresses[0] + "/mex");
                    host.Description.Behaviors.Add(smb);

                    host.AddServiceEndpoint(typeof(IService3),
                                            new BasicHttpBinding(),
                                            "");

                    host.AddServiceEndpoint(typeof(IMetadataExchange),
                                            MetadataExchangeBindings.CreateMexHttpBinding(),
                                            "/mex");

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

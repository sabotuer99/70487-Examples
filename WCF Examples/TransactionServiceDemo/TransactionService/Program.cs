using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using TransactionShared;

namespace TransactionService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(EchoService), 
                              new Uri("http://localhost:60999")))
            {
                //metadata exchange
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                host.Description.Behaviors.Add(smb);

                host.AddServiceEndpoint(typeof(IMetadataExchange),
                                        MetadataExchangeBindings.CreateMexHttpBinding(),
                                        "mex");

                //Service endpoint
                var binding = new WSHttpBinding();
                binding.TransactionFlow = true;
                host.AddServiceEndpoint(typeof(IEchoService),
                                        binding,
                                        "echo");

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

            var serviceBehavior = host.Description.Behaviors.OfType<ServiceBehaviorAttribute>().FirstOrDefault();
            if(serviceBehavior != null)
            {
                Console.WriteLine("Instance Mode: {0}", serviceBehavior.InstanceContextMode);
                Console.WriteLine("Concurrency Mode: {0}", serviceBehavior.ConcurrencyMode);
            }
            
            Console.WriteLine("\n\n");
        }
        
    }
}

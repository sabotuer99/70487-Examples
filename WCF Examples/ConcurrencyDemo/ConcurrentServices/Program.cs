using ConcurrentServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentServices
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost pcm = new ServiceHost(typeof(PerCall_Multi_Service)),
                               pcs = new ServiceHost(typeof(PerCall_Single_Service)), 
                               psm = new ServiceHost(typeof(PerSession_Multi_Service)),
                               pss = new ServiceHost(typeof(PerSession_Single_Service)),
                               sm = new ServiceHost(typeof(Singleton_Multi_Service)),
                               ss = new ServiceHost(typeof(Singleton_Single_Service)))
            {

                ServiceHost[] services = { pcm, pcs, psm, pss, sm, ss };

                foreach(var service in services)
                {
                    service.Open();
                    PrintServiceDescription(service);
                }

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

            Console.WriteLine("Instance Mode: {0}", host.Description.Behaviors.OfType<ServiceBehaviorAttribute>().FirstOrDefault().InstanceContextMode);
            Console.WriteLine("Concurrency Mode: {0}", host.Description.Behaviors.OfType<ServiceBehaviorAttribute>().FirstOrDefault().ConcurrencyMode);
            Console.WriteLine("\n\n");
        }
    }
}

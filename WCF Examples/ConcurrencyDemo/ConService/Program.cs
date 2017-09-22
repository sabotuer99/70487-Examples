using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ConService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost single = new ServiceHost(typeof(SingleService)),
                               persession = new ServiceHost(typeof(SessionService)),
                               percall = new ServiceHost(typeof(CallService)))
            {


                single.Open();
                PrintServiceDescription(single);


                persession.Open();
                PrintServiceDescription(persession);


                percall.Open();
                PrintServiceDescription(percall);

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

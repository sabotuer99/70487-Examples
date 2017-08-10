using ChatShared;
using System.ServiceModel;
using System;
using System.ServiceModel.Description;

namespace ConsoleChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ChatManagerService), new Uri("http://localhost:8080/chatmgr")))
            {
                host.Open();
                PrintServiceDescription(host);
                Console.ReadKey();
            }
        }

        private static void PrintServiceDescription(ServiceHost host)
        {
            Console.WriteLine("{0} is up and running with the following endpoints:", 
                host.Description.ServiceType.Name);
            foreach(ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                Console.WriteLine("Address: {0}  ({1})",
                    endpoint.Address.ToString(), endpoint.Binding.Name);
            }
        }
    }
}

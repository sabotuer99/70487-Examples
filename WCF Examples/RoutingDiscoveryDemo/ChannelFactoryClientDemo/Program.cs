using RoutingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelFactoryClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //service uses Discovery... 
            var services = FindServices();
            var address = services.Endpoints[0].Address;
            var binding = new BasicHttpBinding();
            Console.WriteLine("Found service at: {0}", address);

            //create ChannelFactory Client to found echoservice
            ChannelFactory<IEchoService> factory = 
                new ChannelFactory<IEchoService>(binding, address);

            IEchoService instance = factory.CreateChannel();

            string message = null;
            do
            {
                Console.Write("Enter a message, or blank to exit: ");
                message = Console.ReadLine();
                Console.WriteLine(instance.Echo(message));

            } while (!string.IsNullOrEmpty(message));

            factory.Close();
        }

        static FindResponse FindServices()
        {
            // Create DiscoveryClient  
            FindResponse findResponse = null;
            while(true)
            {
                DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
                Console.WriteLine("Scanning for endpoints.");
                findResponse = discoveryClient.Find(new FindCriteria(typeof(IEchoService))
                {
                    Duration = TimeSpan.FromSeconds(2)
                });

                if(findResponse == null || findResponse.Endpoints == null || findResponse.Endpoints.Count == 0)
                {
                    Console.Write(".");
                    Thread.Sleep(5000);
                } else
                {
                    return findResponse;
                }
            }
        }
    }
}

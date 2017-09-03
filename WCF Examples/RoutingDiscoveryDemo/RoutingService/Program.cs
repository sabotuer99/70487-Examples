using RoutingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoutingService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(System.ServiceModel.Routing.RoutingService),
                new Uri("http://localhost:14552/router")))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.HttpGetUrl = new Uri(host.BaseAddresses[0] + "/mex");
                host.Description.Behaviors.Add(smb);

                var routingConfig = GetRoutingConfig(); 
                RoutingBehavior routingBehavior = new RoutingBehavior(routingConfig);
                host.Description.Behaviors.Add(routingBehavior);
                host.AddServiceEndpoint(typeof(IRequestReplyRouter), new BasicHttpBinding(), "");

                //start a task to update the routing config every 15 seconds
                var task = Task.Factory.StartNew(() => {
                    while (true)
                    {
                        UpdateEndpoints(host);
                        Thread.Sleep(15000);
                    }
                }, TaskCreationOptions.LongRunning);

                host.Open();
                PrintServiceDescription(host);
                Console.ReadKey();
            }
        }


        private static void UpdateEndpoints(ServiceHost host)
        {
            var rc = GetRoutingConfig();
            host.Extensions.Find<RoutingExtension>().ApplyConfiguration(rc);
        }

        private static RoutingConfiguration GetRoutingConfig()
        {
            var routingConfig = new RoutingConfiguration();
            var filter = new MatchAllMessageFilter();
            var endpoints = new List<ServiceEndpoint>();

            foreach(EndpointAddress address in FindServiceAddresses()){
                Console.WriteLine("Adding config for endpoint at address {0}", address.Uri);
                Binding binding = new BasicHttpBinding();
                binding.ReceiveTimeout = TimeSpan.FromMilliseconds(100);
                binding.SendTimeout = TimeSpan.FromMilliseconds(100);
                binding.CloseTimeout = TimeSpan.FromMilliseconds(100);
                binding.OpenTimeout = TimeSpan.FromMilliseconds(100);
                ContractDescription description = ContractDescription.GetContract(typeof(IEchoService));
                endpoints.Add(new ServiceEndpoint(description, binding, address));
            }

            routingConfig.FilterTable.Add(filter, endpoints, 2);

            return routingConfig;
        }

        static IEnumerable<EndpointAddress> FindServiceAddresses()
        {
            // Create DiscoveryClient  
            DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            Console.WriteLine("Scanning for endpoints...");
            FindResponse findResponse = discoveryClient.Find(new FindCriteria(typeof(IEchoService)) {
                Duration = TimeSpan.FromSeconds(2)
            });
            return findResponse.Endpoints.Select(x => x.Address);
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

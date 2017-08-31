using RoutingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using System.Text;
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

                var routingConfig = new RoutingConfiguration();
                var filter = new MatchAllMessageFilter();
                var endpoints = new List<ServiceEndpoint>();
                endpoints.Add(getEchoClient(15560));
                endpoints.Add(getEchoClient(33344));
                endpoints.Add(getEchoClient(22455));
                routingConfig.FilterTable.Add(filter, endpoints, 2);

                RoutingBehavior routingBehavior = new RoutingBehavior(routingConfig);


                host.Description.Behaviors.Add(routingBehavior);


                host.AddServiceEndpoint(typeof(IRequestReplyRouter), new BasicHttpBinding(), "");


                host.Open();
                PrintServiceDescription(host);
                Console.ReadKey();
            }
        }

        private static ServiceEndpoint getEchoClient(int port)
        {
            Binding binding = new BasicHttpBinding();
            binding.ReceiveTimeout = TimeSpan.FromMilliseconds(100);
            binding.SendTimeout    = TimeSpan.FromMilliseconds(100);
            binding.CloseTimeout   = TimeSpan.FromMilliseconds(100);
            binding.OpenTimeout    = TimeSpan.FromMilliseconds(100);
            EndpointAddress address = new EndpointAddress("http://localhost:" + port + "/print");
            ContractDescription description = ContractDescription.GetContract(typeof(IPrintService));
            return new ServiceEndpoint(description, binding, address);
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

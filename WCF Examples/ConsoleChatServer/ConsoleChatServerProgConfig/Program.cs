using ChatShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChatServerProgConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ChatManagerService), 
                                                      new Uri("http://localhost:8080/chatmgr")))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                //smb.HttpGetBinding = new WebHttpBinding();
                smb.HttpGetUrl = new Uri(host.BaseAddresses[0] + "/mex");

                ProfanityInterceptorBehavior pib = new ProfanityInterceptorBehavior();

                host.Description.Behaviors.Add(smb);
                //host.Description.Behaviors.Add(pib);

                //Manually create the service endpoint, and add to host collection
                Binding binding = new WSDualHttpBinding();
                EndpointAddress address = new EndpointAddress(host.BaseAddresses[0] + "/duplex");
                ContractDescription description = ContractDescription.GetContract(typeof(IChatManager));
                ServiceEndpoint endpoint = new ServiceEndpoint(description, binding, address);
                endpoint.EndpointBehaviors.Add(pib);
                host.AddServiceEndpoint(endpoint);

                //Add the metadata endpoint directly
                host.AddServiceEndpoint(typeof(IMetadataExchange), 
                                        MetadataExchangeBindings.CreateMexHttpBinding(), 
                                        "mex");

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

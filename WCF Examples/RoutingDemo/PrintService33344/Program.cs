using RoutingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace PrintService33344
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(PrintService),
                                                     new Uri("http://localhost:33344/print")))
            {
                host.AddServiceEndpoint(typeof(IPrintService),
                                        new BasicHttpBinding(),
                                        "");

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

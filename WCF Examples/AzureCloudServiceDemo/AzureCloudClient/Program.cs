using AzureCloudClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCloudClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var client = new ServiceReference1.Service1Client())
            {
                var one = client.GetData(99);
                CompositeType composite = new CompositeType();
                composite.BoolValue = true;
                composite.StringValue = "Hello Azure!";
                var two = client.GetDataUsingDataContract(composite);

                Console.WriteLine(one);
                Console.WriteLine(two.StringValue);

                Console.ReadLine();
            }
        }
    }
}

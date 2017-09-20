using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipProviderClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new EchoService.ServiceClient())
            {
                client.ClientCredentials.UserName.UserName = "wcfuser";
                client.ClientCredentials.UserName.Password = "blah";

                Console.WriteLine(client.Echo("A message from the client"));
                Console.ReadLine();
            }
        }
    }
}

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
                Console.WriteLine("Enter Username: ");
                var username = Console.ReadLine();
                Console.WriteLine("Enter Password: ");
                var password = Console.ReadLine();
                client.ClientCredentials.UserName.UserName = username;
                client.ClientCredentials.UserName.Password = password;

                Console.WriteLine(client.Echo("A message from the client"));
                Console.ReadLine();
            }
        }
    }
}

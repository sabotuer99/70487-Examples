using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new EchoService.ServiceClient())
            {

                Console.WriteLine(client.Echo("A message from the client"));
                Console.ReadLine();
            }
        }
    }
}

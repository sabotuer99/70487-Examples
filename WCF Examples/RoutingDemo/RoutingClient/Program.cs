using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var client = new PrintService.PrintServiceClient())
            {
                string msg = null;
                do
                {
                    Console.Write("Enter a message, or blank to exit: ");
                    msg = Console.ReadLine();
                    client.Print(msg);


                } while (msg != "");
                
            }
        }
    }
}

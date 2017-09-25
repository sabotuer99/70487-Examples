using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TransactionClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new EchoServiceReference.EchoServiceClient())
            {
                string msg = null;
                do
                {
                    Console.Write("Enter a message, or blank to exit: ");
                    msg = Console.ReadLine();
                    using (TransactionScope scope = new TransactionScope())
                    {
                        Console.WriteLine(client.Echo(msg));
                        scope.Complete();
                    }                                      
                } while (msg != "");

            }
        }
    }
}

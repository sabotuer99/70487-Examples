using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var callClient = new CallServiceReference.ConServiceClient())
            using(var sessionClient = new SessionServiceReference.ConServiceClient())
            using(var singleClient = new SingleServiceReference.ConServiceClient())
            {
                string command = null;
                do
                {
                    Console.WriteLine("Single: " + singleClient.Status());
                    Console.WriteLine("Session: " + sessionClient.Status());
                    Console.WriteLine("Call: " + callClient.Status() + "\n\n");

                    Console.WriteLine("Hit 'x' to go again, anything else to quit...");
                    command = Console.ReadKey().KeyChar.ToString();

                    Console.WriteLine("\n\n");

                } while (command == "x");
            }
        }
    }
}

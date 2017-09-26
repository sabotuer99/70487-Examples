using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var relay = new EchoServiceReference.EchoServiceClient("NetTcpRelayBinding_IEchoService"))
            using (var direct = new EchoServiceReference.EchoServiceClient("NetTcpBinding_IEchoService"))
            {
                string message = null;
                do
                {
                    Console.WriteLine("Enter a message, or [ENTER] to quit...");
                    message = Console.ReadLine();

                    if (message == "") continue;

                    Console.WriteLine("Calling directly...");

                    var sw = new Stopwatch();
                    sw.Start();
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine(direct.Echo(message));
                    }
                    sw.Stop();
                    Console.WriteLine("Took " + sw.ElapsedMilliseconds + "ms");

                    Console.WriteLine("\n\nCalling relay...");
                    sw.Reset();
                    sw.Start();
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine(relay.Echo(message));
                    }
                    sw.Stop();
                    Console.WriteLine("Took " + sw.ElapsedMilliseconds + "ms");


                } while (message != "");               
            }
        }
    }
}

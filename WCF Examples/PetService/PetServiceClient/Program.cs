using PetServiceClient.CatServiceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new CatServiceClient.CatServiceClient("TCPEndpoint");
            string color = "";
            string name = "";
            client.taskKitten(out color, out name);

            Console.WriteLine("Syncronous call from client to async server code returned : " + name + " | " + color);

            Console.WriteLine("Calling service asyncronously: ");

            Dictionary<int, Task<Kitten>> waitList = new Dictionary<int, Task<Kitten>>();
            Console.Write("Requesting async kittens: ");
            var start = DateTime.Now.Ticks;
            for (int i = 0; i < 15; i++)
            {
                Console.Write(" #" + i);
                var request = new taskKittenRequest();
                waitList.Add(i, client.taskKittenAsync(request));
            }
            Console.WriteLine();

            while(waitList.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Checking on new kittens");
                Console.ForegroundColor = ConsoleColor.White;
                var keys = waitList.Keys.ToList();
                foreach(int i in keys)
                {
                    
                    if (waitList[i].IsCompleted)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Yay, kitten #" + i + " is here!");
                        Kitten newKitty = waitList[i].Result;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(newKitty.color + " | " + newKitty.name);
                        waitList.Remove(i);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    
                }

                if(waitList.Count > 0)
                {
                    Console.WriteLine("Still waiting on some kitties, lets take a nap...");
                    Thread.Sleep(500);
                }
                
            }

            Console.WriteLine("Getting the async kitties took " + 
                TimeSpan.FromTicks(DateTime.Now.Ticks - start).TotalMilliseconds + "ms ");

            Console.Write("Requesting sync kittens: ");
            start = DateTime.Now.Ticks;
            for (int i = 0; i < 15; i++)
            {
                Console.Write(" #" + i);
                var request = new syncKittenRequest();
                waitList.Add(i, client.syncKittenAsync(request));
            }
            Console.WriteLine();

            while (waitList.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Checking on new kittens");
                Console.ForegroundColor = ConsoleColor.White;
                var keys = waitList.Keys.ToList();
                foreach (int i in keys)
                {

                    if (waitList[i].IsCompleted)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Yay, kitten #" + i + " is here!");
                        Kitten newKitty = waitList[i].Result;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(newKitty.color + " | " + newKitty.name);
                        waitList.Remove(i);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                }

                if (waitList.Count > 0)
                {
                    Console.WriteLine("Still waiting on some kitties, lets take a nap...");
                    Thread.Sleep(500);
                }

            }

            Console.WriteLine("Getting the sync kitties took " + 
                TimeSpan.FromTicks(DateTime.Now.Ticks - start).TotalMilliseconds + "ms ");


            Console.WriteLine("Press <ENTER> to exit.");
            Console.ReadLine();
        }


    }
}

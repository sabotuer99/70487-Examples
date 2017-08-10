using ChatShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ConsoleChatClient
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to ChatClient [Color Edition]!");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Please enter your name:");      

            ChatService localService = new ChatService();
            localService.Name = Console.ReadLine(); ;
            var dcf = new DuplexChannelFactory<IChatManager>(localService, "manager");

            IChatManager manager = dcf.CreateChannel();
            manager.RegisterClient(localService.Name);

            ConsoleColor color = ChooseColor();

            
            Console.WriteLine("Type CHANGECOLOR at any time to switch.");
            Console.WriteLine("Type EXIT at any time to leave.");
            Console.WriteLine("Chat away, chump:");
            string command = Console.ReadLine(); 
            while(command != "EXIT")
            {
                if(command == "CHANGECOLOR")
                {
                    color = ChooseColor();
                } else
                {
                    var message = new ChatMessage(localService.Name, DateTime.Now, command, color);
                    manager.BroadcastMessage(message);
                }

                command = Console.ReadLine(); 
            }

            manager.UnRegisterClient(localService.Name);
        }

        private static ConsoleColor ChooseColor()
        {
            var colors = Enum.GetNames(typeof(ConsoleColor));
            Console.WriteLine("Pick your color: ");
            foreach (var c in colors.Where(c => c != "Black"))
            {
                Console.WriteLine(c);
            }

            ConsoleColor color = ConsoleColor.Black;
            while (color == ConsoleColor.Black)
            {
                try
                {
                    color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Console.ReadLine());
                    if (!colors.Contains(color.ToString()) || color == ConsoleColor.Black)
                    {
                        throw new Exception();
                    }

                }
                catch
                {
                    Console.WriteLine("INVALID SELECTION STUPID!");
                    color = ConsoleColor.Black;
                }

            }

            Console.ForegroundColor = color;
            Console.WriteLine("Your chats will all be {0}", color.ToString());
            return color;
        }
    }
}

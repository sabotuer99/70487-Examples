using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;


/// <summary>
/// Code is take almost verbatim from Aaron Skonnard's "WCF Design Concepts" PluralSight course, the Duplex Contracts Demo.
/// </summary>
namespace ChatShared
{
    [DataContract(Namespace = "http://failedturing.com/chatdemo")]
    public class ChatMessage
    {
        public ChatMessage() { }
        public ChatMessage(string name, DateTime timeStamp, string message, ConsoleColor color)
        {
            Name = name;
            TimeStamp = timeStamp;
            Message = message;
            Color = color;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
        [DataMember]
        public ConsoleColor Color { get; set; }

    }

    [ServiceContract(Namespace = "http://failedturing.com/chatdemo")]
    public interface IChat
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(ChatMessage message);

        [OperationContract(IsOneWay = true)]
        void ChangeName(string name);
    }

    [ServiceContract(Namespace = "http://failedturing.com/chatdemo",
        CallbackContract = typeof(IChat))]
    public interface IChatManager
    {
        [OperationContract(IsOneWay = true)]
        void RegisterClient(string name);

        [OperationContract(IsOneWay = true)]
        void UnRegisterClient(string name);

        [OperationContract(IsOneWay = true)]
        void BroadcastMessage(ChatMessage message);

    }

    public class ChatService : IChat
    {
        public string Name { get; set; }

        public void ChangeName(string name)
        {
            Name = name;

        }

        public void SendMessage(ChatMessage message)
        {
            var before = Console.ForegroundColor;
            Console.ForegroundColor = message.Color;
            Console.WriteLine("{0}  {1}: {2}", message.TimeStamp.ToLocalTime(), message.Name, message.Message);
            Console.ForegroundColor = before;
        }

    }

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ChatManagerService : IChatManager
    {
        Dictionary<string, IChat> clients = new Dictionary<string, IChat>();

        public void BroadcastMessage(ChatMessage message)
        {
            string said = message.Message.Length > 12 ? message.Message.Substring(0, 12) + "..." : message.Message;
            Console.WriteLine("Broadcast message from {0}: {1}", message.Name, said);
            foreach (string key in clients.Keys.Where(k => k != message.Name))
            {
                try
                {
                    var client = clients[key];
                    client.SendMessage(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
            }
        }

        public void RegisterClient(string name)
        {
            try
            {
                IChat client = OperationContext.Current.GetCallbackChannel<IChat>();
                if (!clients.ContainsKey(name))
                {
                    clients.Add(name, client);
                } else
                {
                    name += new Random().Next(100).ToString();
                    clients.Add(name, client);
                    client.ChangeName(name);
                    client.SendMessage(new ChatMessage("SYSTEM", DateTime.Now, "Your name was changed to " + name, ConsoleColor.Red));
                }
                var message = string.Format("{0} joined.", name);
                Console.WriteLine(message);
                var announce = new ChatMessage(name, DateTime.Now, message, ConsoleColor.Yellow);
                BroadcastMessage(announce);

                var welcome = new ChatMessage("SYSTEM", DateTime.Now, "Welcome to the chat! Already here: " + string.Join(", ", clients.Keys), ConsoleColor.Blue);
                client.SendMessage(welcome);
            } catch (Exception ex)
            {
                BroadcastMessage(new ChatMessage("SYSTEM", DateTime.Now, ex.Message, ConsoleColor.Red));
            }
        }

        public void UnRegisterClient(string name)
        {
            
            clients.Remove(name);

            var message = string.Format("{0} left.", name);
            Console.WriteLine(message);
            var announce = new ChatMessage(name, DateTime.Now, message, ConsoleColor.DarkRed);
            BroadcastMessage(announce);
        }
    }

}

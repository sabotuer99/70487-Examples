using ConcurrentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentClient
{
    class Program
    {
        private const char PROCESSING_STARTED = 'B';
        private const char PROCESSING = 'P';
        private const char PROCESSING_FINISHED = 'F';
        private const char CALL_INITIATED = 'I';

        static void Main(string[] args)
        {
            Console.WriteLine("                      Press 'Enter' to start.");
            Console.ReadLine();
            Console.WriteLine("                  Press 'Enter' again at any time to exit...");
            Console.WriteLine("##########################################################################################################");
            Console.WriteLine("#         PerCall                  PerSession                 Singleton              Singleton Again     #");
            Console.WriteLine("#  ######  ######  ######    ######  ######  ######    ######  ######  ######    ######  ######  ######  #");
            Console.WriteLine("#  Multi   Single  Reentr    Multi   Single  Reentr    Multi   Single  Reentr    Multi   Single  Reentr  #");
            Console.WriteLine("#  ######  ######  ######    ######  ######  ######    ######  ######  ######    ######  ######  ######  #");
            Console.WriteLine("#  012345  012345  012345    012345  012345  012345    012345  012345  012345    012345  012345  012345  #");

            messageBuffer = NewBuffer();
            Task.Run(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    ProcessMessageBuffer(messageBuffer);
                    Thread.Sleep(1000);
                }
                Console.WriteLine("##########################################################################################################");
            });

            RunServices(messageBuffer);

            Console.ReadLine();
        }

        private static void RunServices(char[] messageBuffer)
        {
            string[] serviceNames = { "WSDualHttpBinding_IService","WSDualHttpBinding_IService1","NetTcpBinding_IService",
                                      "WSDualHttpBinding_IService2","WSDualHttpBinding_IService3","NetTcpBinding_IService1",
                                      "WSDualHttpBinding_IService4","WSDualHttpBinding_IService5","NetTcpBinding_IService2",
                                      "WSDualHttpBinding_IService4","WSDualHttpBinding_IService5","NetTcpBinding_IService2"};
            int[] offsets = { 3, 11, 19, 29, 37, 45, 55, 63, 71, 81, 89, 97};
            for (int i = 0; i < 12; i++)
            {
                string endpoint = serviceNames[i];
                int offset = offsets[i];
                IService client = getClient(messageBuffer, endpoint);
                for (int j = 0; j < 6; j++)
                {
                    int id = offset + j;
                    Task.Run(() => client.Process(id));
                    messageBuffer[id] = CALL_INITIATED;
                    //Console.WriteLine("Started: " + id);
                }
            }
        }

        private static char[] NewBuffer()
        {
            var messageBuffer = new char[106];
            for (int i = 0; i < messageBuffer.Length; i++)
            {
                messageBuffer[i] = ' ';
            }

            messageBuffer[0] = '#';
            messageBuffer[105] = '#';
            return messageBuffer;
        }

        private static void ProcessMessageBuffer(char[] messageBuffer)
        {
            lock (messageBuffer)
            {
                for (int i = 0; i < messageBuffer.Length; i++)
                {
                    char x = messageBuffer[i];
                    switch (x)
                    {
                        case PROCESSING_STARTED:
                            Console.ForegroundColor = ConsoleColor.Green;
                            messageBuffer[i] = PROCESSING;
                            Console.Write("V");
                            break;
                        case PROCESSING:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("|");
                            break;
                        case PROCESSING_FINISHED:
                            Console.ForegroundColor = ConsoleColor.Red;
                            messageBuffer[i] = ' ';
                            Console.Write("X");
                            break;
                        case CALL_INITIATED:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            messageBuffer[i] = ' ';
                            Console.Write("@");
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(x);
                            break;
                    }
                }
                Console.Write("\n");
            }
        }

        static IService getClient(char[] messageBuffer, string serviceName)
        {
            ICallback localService = new Callback();
            var dcf = new DuplexChannelFactory<IService>(localService, serviceName);
            return dcf.CreateChannel();
        }

        private static char[] messageBuffer;

        [ServiceBehavior(UseSynchronizationContext = true)]
        class Callback : ICallback
        {
            public void NotifyBegin(int id)
            {
                messageBuffer[id] = PROCESSING_STARTED;
            }

            public void NotifyEnd(int id)
            {
                messageBuffer[id] = PROCESSING_FINISHED;
            }
        }
    }
}

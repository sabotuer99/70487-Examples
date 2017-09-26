using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EchoService
{
    public class EchoService : IEchoService
    {
        public string Echo(string message)
        {
            var endpoint = OperationContext.Current.Channel.LocalAddress.Uri.ToString();
            Console.WriteLine("Request on " + endpoint);

            return "You said: " + new string(message.ToCharArray().Reverse().ToArray());
        }
    }
}

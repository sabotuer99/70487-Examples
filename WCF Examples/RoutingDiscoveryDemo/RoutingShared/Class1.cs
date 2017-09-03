using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RoutingShared
{
    [ServiceContract]
    public interface IEchoService
    {
        [OperationContract]
        string Echo(string message);
    }

    public class EchoService : IEchoService
    {

        public string Echo(string message)
        {
            var id = OperationContext.Current.Channel.LocalAddress.Uri.Port.ToString();
            Console.WriteLine("Got new message: {0}, echoing...", message);
            return (id + ": " + message);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceShared
{
    public class Service : IService1, IService2
    {
        string endpoint = OperationContext.Current.Channel.LocalAddress.Uri.ToString();

        public string OperationA()
        {
            string message = "Called OperationA on endpoint " + endpoint;
            Console.WriteLine(message);
            return message;
        }

        public string OperationB()
        {
            string message = "Called OperationB on endpoint " + endpoint;
            Console.WriteLine(message);
            return message;
        }

        public string OperationC()
        {
            string message = "Called OperationC on endpoint " + endpoint;
            Console.WriteLine(message);
            return message;
        }
    }
}

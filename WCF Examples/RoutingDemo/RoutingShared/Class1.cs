using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RoutingShared
{
    [ServiceContract]
    public interface IPrintService
    {
        [OperationContract]
        void Print(string message);
    }

    public class PrintService : IPrintService
    {

        public void Print(string message)
        {
            var id = OperationContext.Current.Channel.LocalAddress.Uri.Port.ToString();
            Console.WriteLine(id + ": " + message);
        }
    }

}

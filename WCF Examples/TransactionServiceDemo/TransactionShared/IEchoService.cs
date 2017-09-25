using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TransactionShared
{
    [ServiceContract(SessionMode=SessionMode.Required)]
    public interface IEchoService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        string Echo(string message);
    }
}

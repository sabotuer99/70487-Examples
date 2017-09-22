using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentServices
{
    [ServiceContract]
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyBegin(int id);

        [OperationContract(IsOneWay = true)]
        void NotifyEnd(int id);

    }
}

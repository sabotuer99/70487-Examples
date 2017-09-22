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
        [OperationContract]
        void NotifyBegin(int id);

        [OperationContract]
        void NotifyEnd(int id);

    }
}

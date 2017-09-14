using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceShared
{
    [ServiceContract(Name ="IService")]
    public interface IService1
    {
        [OperationContract(Action = "OperationA")]
        string OperationA();

        [OperationContract(Action = "OperationB")]
        string OperationB();
    }
}

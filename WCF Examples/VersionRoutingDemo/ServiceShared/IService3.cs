using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceShared
{
    [ServiceContract(Name = "IService")]
    public interface IService3
    {
        [OperationContract(Action = "OperationC")]
        string OperationC();

        [OperationContract(Action = "OperationB")]
        string OperationB();

        [OperationContract(Action = "OperationA")]
        string OperationA();
    }
}


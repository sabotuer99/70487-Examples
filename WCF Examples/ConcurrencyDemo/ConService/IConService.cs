using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConService
{
    [ServiceContract]
    public interface IConService
    {
        [OperationContract]
        string Status();
    }
}

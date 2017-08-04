using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PetService
{
    [ServiceContract]
    public class DogService
    {
        [OperationContract]
        public string speak()
        {
            return "Woof!";
        }
    }
}

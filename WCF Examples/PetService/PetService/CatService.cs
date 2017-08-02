using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PetService
{
    [ServiceContract]
    public interface ICatService
    {
        [OperationContract]
        string speak();
    }


    public class CatService : ICatService
    {
        public string speak()
        {
            return "Meow!";
        }
    }
}

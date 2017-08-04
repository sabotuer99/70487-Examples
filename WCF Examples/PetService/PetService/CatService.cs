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

        [OperationContract]
        Kitten haveKitten();

        [XmlSerializerFormat]
        [OperationContract]
        XKitten haveXKitten();
    }


    public class CatService : ICatService
    {
        public Kitten haveKitten()
        {
            return new Kitten("Maxine", "Calico");
        }

        public XKitten haveXKitten()
        {
            return new XKitten("Sebastian", "Grey");
        }

        public string speak()
        {
            return "Meow!";
        }
    }
}

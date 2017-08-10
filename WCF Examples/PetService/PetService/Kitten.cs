using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;
using System.Net.Security;

namespace PetService
{
    [MessageContract(WrapperName = "CuteKitty")]
    public class Kitten
    {
        public Kitten(string name, string color)
        {
            this.name = name;
            this.color = color;
        }

        [MessageHeader]
        string id = Guid.NewGuid().ToString();

        [MessageBodyMember]
        string name;
        [MessageBodyMember]
        string color;
    }

    [Serializable]
    public class SKitten {
        public SKitten(string name, string color)
        {
            this.name = name;
            this.color = color;
        }
        
        string name;
        string color;
    }
    
    public class XKitten
    {
        public XKitten() { }

        public XKitten(string name, string color)
        {
            this.name = name;
            this.color = color;
        }

        [XmlAttribute]
        public string name;
        [XmlAttribute]
        public string color;
    }
}
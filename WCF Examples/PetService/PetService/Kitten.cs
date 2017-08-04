using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PetService
{
    [DataContract]
    public class Kitten
    {
        public Kitten(string name, string color)
        {
            this.name = name;
            this.color = color;
        }

        [DataMember]
        string name;
        [DataMember]
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
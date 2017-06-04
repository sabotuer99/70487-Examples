using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML_Examples
{
    partial class Program
    {
        public static void AttemptToLoadHugeXmlIntoMemory()
        {
            Console.WriteLine("Loading document into memory... (this won't end well)");
            var dom = new XmlDocument();
            dom.Load(@"C:\XML\psd7003.xml");
        }
    }
}

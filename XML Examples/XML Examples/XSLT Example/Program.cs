using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace XSLT_Example
{
    class Program
    {
        public static void Main(string[] args)
        {
            XsltArgumentList argsList = new XsltArgumentList();
            argsList.AddParam("source", "", "yahoo");

            XslCompiledTransform transform = new XslCompiledTransform(true);
            transform.Load("Transform.xslt");
            var settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            var reader = XmlReader.Create("yahoo.xml", settings);

            using (StreamWriter sw = new StreamWriter("output.html"))
            {
                transform.Transform(reader, argsList, sw);
            }
        }
    }
}

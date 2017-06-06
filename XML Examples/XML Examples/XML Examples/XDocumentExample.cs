using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XML_Examples
{
    partial class Program
    {
        public static void XDocumentSample()
        {
            Console.WriteLine("Loading document into memory...\n\n");
            var dom = XDocument.Load(@"C:\XML\XMLAnatomy.xml");

            var declaration = dom.Declaration;

            
            var ids = dom.Descendants().SelectMany(x => x.Attributes())
                .Where(x => x.Name.LocalName.Equals("id"));
            var xids = ((IEnumerable)dom.XPathEvaluate("descendant::*/@id"))
                .Cast<XAttribute>();

            //remove all the comments
            dom.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Remove();     

            //grab the "items" and clone a cd
            var items = dom.Descendants("items").FirstOrDefault();
            XNamespace ns = items.GetNamespaceOfPrefix("cd");
            var disc = new XElement(dom.Descendants(ns + "artist")
                .Where(x => "Nelly".Equals(x.Value))
                .FirstOrDefault().Parent);

            //change the details
            disc.Descendants(ns + "price")
                .FirstOrDefault().Value = "21.50";
            disc.Descendants(ns + "artist")
                .FirstOrDefault().Value = "Alanis Morrisette";
            disc.Descendants(ns + "title")
                .FirstOrDefault().Value = "Jagged Little Pill";

            //add back into list
            items.Add(disc);

            //create a cd from scratch

            XElement newDisc = new XElement(ns + "compact-disc",
                new XElement(ns + "price", "99.95"),
                new XElement(ns + "artist", "Elton John"),
                new XElement(ns + "title", "The Complete Works")
                );
            items.Add(newDisc);

            //commit changes
            var sb = new StringBuilder();
            dom.Save(new StringWriter(sb));

            Console.WriteLine(sb.ToString() + "\n\n");
        }
    }
}

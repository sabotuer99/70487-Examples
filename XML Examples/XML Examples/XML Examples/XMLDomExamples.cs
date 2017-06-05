using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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

        public static void LoadSmallerXmlFileIntoMemory()
        {
            Console.WriteLine("Loading document into memory...");
            var dom = new XmlDocument();
            dom.Load(@"C:\XML\XMLAnatomy.xml");

            var sb = new StringBuilder();
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            var xw = XmlWriter.Create(sb, settings);
            dom.WriteTo(xw);
            xw.Flush();
            xw.Close();

            Console.WriteLine(sb.ToString());
        }

        public static void SaveChangesToXmlDocumentObject()
        {
            Console.WriteLine("Loading document into memory...\n\n");
            var dom = new XmlDocument();
            dom.Load(@"C:\XML\XMLAnatomy.xml");

            var declaration = dom.ChildNodes.Cast<XmlNode>()
                .Where(x => x.NodeType == XmlNodeType.XmlDeclaration).FirstOrDefault();

            var ids = dom.SelectNodes("descendant::*/@id");

            //add the "cd" namespace prefix
            var nsmgr = new XmlNamespaceManager(dom.NameTable);
            nsmgr.AddNamespace("cd", "http://example.com/2007/Compact-Disc");

            //remove all the comments
            var comments = dom.SelectNodes("descendant::comment()");
            foreach (XmlNode comment in comments)
            {
                comment.ParentNode.RemoveChild(comment);
            }

            //grab the "items" and clone a cd
            var items = dom.SelectSingleNode("//items");
            var disc = dom.SelectSingleNode(
                "//cd:compact-disc[cd:artist='Nelly']", nsmgr)
                .Clone();

            //change the details
            disc.SelectSingleNode("cd:price", nsmgr)
                .InnerText = "21.50";
            disc.SelectSingleNode("cd:artist", nsmgr)
                .InnerText = "Alanis Morrisette";
            disc.SelectSingleNode("cd:title", nsmgr)
                .InnerText = "Jagged Little Pill";

            //add back into list
            items.AppendChild(disc);

            //create a cd from scratch
            string ns = "http://example.com/2007/Compact-Disc";
            var newCd = dom.CreateElement("cd", "compact-disc", ns);
            var price = dom.CreateElement("cd", "price", ns);
            price.InnerText = "99.95";
            var artist = dom.CreateElement("cd", "artist", ns);
            artist.InnerText = "Elton John";
            var title = dom.CreateElement("cd", "title", ns);
            title.InnerText = "The Complete Works";
            newCd.AppendChild(price);
            newCd.AppendChild(artist);
            newCd.AppendChild(title);
            items.AppendChild(newCd);

            //commit changes
            var sb = new StringBuilder();
            dom.Save(new StringWriter(sb));

            Console.WriteLine(sb.ToString() + "\n\n");
        }
    }
}

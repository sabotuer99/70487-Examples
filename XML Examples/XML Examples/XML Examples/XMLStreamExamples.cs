using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML_Examples
{
    partial class Program
    {
        public static void XmlWriterExample()
        {
            //var sb = new StringBuilder();
            var sb = new FileStream(@"C:\XML\writeroutput.xml", FileMode.Create);
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.WriteEndDocumentOnClose = true;
            settings.Encoding = Encoding.GetEncoding("iso-8859-1");
            var xw = XmlWriter.Create(sb, settings);

            xw.WriteStartDocument();
            xw.WriteComment(" XML declaration           ");
            xw.WriteProcessingInstruction("xml-stylesheet", "href='orders.xsl'");
            xw.WriteComment(" XML stylesheet processing ");
            xw.WriteStartElement("order");
            xw.WriteAttributeString("id", "ord123456");
            xw.WriteComment("\"id\" is an attribute of order.  ");
            {  //braces are just for organization
                xw.WriteStartElement("customer");
                xw.WriteAttributeString("id", "cust0921");
                xw.WriteComment("  attributes can be surrounded with ' or \". ");
                {
                    xw.WriteStartElement("title");
                    xw.WriteAttributeString("value", "Sir");
                    xw.WriteEndElement();
                    xw.WriteComment("  self-closing (empty) tag.  ");
                    WriteSimpleTag("first-name", "Dare", xw);
                    WriteSimpleTag("First-Name", "DARE", xw);
                    xw.WriteComment("  XML is case sensitive   ");
                    WriteSimpleTag("last-name", "Obasanjo", xw);
                    xw.WriteStartElement("address");
                    {
                        xw.WriteComment("\"address\" opening tag");
                        WriteSimpleTag("street", "One Microsoft Way", xw);
                        xw.WriteComment("   *********************   ");
                        WriteSimpleTag("city", "Redmond", xw);
                        xw.WriteComment("   children of \"address\"   ");
                        WriteSimpleTag("state", "WA", xw);
                        xw.WriteComment("   *********************   ");
                        WriteSimpleTag("zip", "98052", xw);
                    }
                    xw.WriteEndElement(); //close address
                    xw.WriteComment("\"address\" closing tag");
                    xw.WriteStartElement("special");
                    xw.WriteCData("<><>@#$%^&\"\"\"\"\"''");
                    xw.WriteComment("  CDATA is \"character data\" and can ");
                    xw.WriteEndElement();
                    xw.WriteComment("  contain XML special characters    ");
                }
                xw.WriteEndElement(); //close customer

                xw.WriteComment("  xmlns is used to declare a namespace prefix ");
                xw.WriteStartElement("items");
                xw.WriteAttributeString("xmlns", "cd", null, "http://example.com/2007/Compact-Disc");
                {
                    xw.WriteStartElement("cd", "compact-disc", null);
                    {
                        WriteSimpleTagNs("price", "cd", "16.95", xw);
                        WriteSimpleTagNs("artist", "cd", "Nelly", xw);
                        WriteSimpleTagNs("title", "cd", "Nellyville", xw);
                    }
                    xw.WriteEndElement();
                    xw.WriteStartElement("cd", "compact-disc", null);
                    {
                        WriteSimpleTagNs("price", "cd", "17.55", xw);
                        WriteSimpleTagNs("artist", "cd", "Baby D", xw);
                        WriteSimpleTagNs("title", "cd", "Lil Chopper Toy", xw);
                    }
                    xw.WriteEndElement();
                }
                //the last couple closing tags will be added automatically
            }

            xw.Flush();
            xw.Close();

            //Console.Write(sb.ToString() + "\n\n");
        }

        private static void WriteSimpleTag(string name, string value, XmlWriter xw)
        {
            xw.WriteStartElement(name);
            xw.WriteString(value);
            xw.WriteEndElement();
        }

        private static void WriteSimpleTagNs(string name, string ns, string value, XmlWriter xw)
        {
            xw.WriteStartElement(ns, name, null);
            xw.WriteString(value);
            xw.WriteEndElement();
        }

        public static void XmlReaderPerformanceTest()
        {
            var settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ValidationType = ValidationType.DTD;
            settings.XmlResolver = new XmlUrlResolver();

            var xr = XmlReader.Create(@"C:\XML\psd7003.xml", settings);

            var timer = new Stopwatch();
            Console.WriteLine("Processing started...");
            timer.Start();

            int count = 0;
            int error = 0;
            while (!xr.EOF)
            {
                try
                {
                    xr.Read();
                    count++;
                }
                catch
                {
                    error++;
                }


            }
            timer.Stop();

            Console.WriteLine("Called Read() " + count + " times successfully,\n" +
                "encountered " + error + " validation errors, \n" +
                "took " + timer.ElapsedMilliseconds + "ms");
        }



        public static void XmlReaderSmallerFile()
        {
            var xr = XmlReader.Create(@"C:\XML\XMLAnatomy.xml");
            while (!xr.EOF)
            {
                xr.Read();
                SetConsoleColor(xr);
                Console.Write("(" + xr.NodeType + ")" + xr.LocalName + ": " + xr.Value);
            }
        }


        public static void XmlReaderMoveToContent()
        {
            var xr = XmlReader.Create(@"C:\XML\XMLAnatomy.xml");
            int prevdepth = 0;
            bool prevContent = false;
            while (!xr.EOF)
            {
                xr.Read();
                xr.MoveToContent();
                var indent = "  ";

                if (prevdepth < xr.Depth && xr.NodeType == XmlNodeType.Element)
                {
                    Console.WriteLine();

                }

                if ((xr.NodeType == XmlNodeType.Element || xr.NodeType == XmlNodeType.EndElement) &&
                    !prevContent)
                {
                    indent = string.Join("", Enumerable.Repeat("  ", xr.Depth));
                }

                prevContent = SetConsoleColor(xr);

                Console.Write(indent + "(" + xr.NodeType + ")" + xr.LocalName + ": " + xr.Value);

                if (prevdepth > xr.Depth || xr.IsEmptyElement)
                {
                    Console.WriteLine();
                }
                prevdepth = xr.Depth;
            }
        }

        private static bool SetConsoleColor(XmlReader xr)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            bool prevContent = false;
            switch (xr.NodeType)
            {
                case XmlNodeType.Text:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    prevContent = true;
                    break;
                case XmlNodeType.CDATA:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    prevContent = true;
                    break;
                case XmlNodeType.Comment:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    prevContent = true;
                    break;
                case XmlNodeType.Whitespace:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            return prevContent;
        }
    }
}

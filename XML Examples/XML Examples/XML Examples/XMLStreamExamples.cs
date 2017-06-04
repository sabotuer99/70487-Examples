using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML_Examples
{
    partial class Program
    {
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

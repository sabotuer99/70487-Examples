using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StreamingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReaderPerformanceTest();

            Console.Read();
        }

        private static string BIG = @"C:\XML\psd7003.xml";
        //private static string SMALL = @"C:\XML\reed.xml";

        public static void XmlReaderPerformanceTest()
        {
            using(var client = new FileServiceReference.FileServiceClient())
            {
                var settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.ValidationType = ValidationType.DTD;
                settings.XmlResolver = new XmlUrlResolver();

                //This line is the original
                Console.WriteLine("Processing started via local filesystem...");
                var xr = XmlReader.Create(BIG, settings);
                ProcessReader(xr);

                Console.WriteLine("\n\nProcessing started via WCF file stream service...");
                var xr_wcf = XmlReader.Create(client.getFile(BIG), settings);
                ProcessReader(xr_wcf);
            }

        }

        private static void ProcessReader(XmlReader xr)
        {
            var timer = new Stopwatch();
            timer.Start();

            int count = 0;
            int error = 0;
            while (!xr.EOF)
            {
                if ((count + error) % 100000 == 0)
                {
                    Console.Write(".");
                }

                try
                {
                    xr.Read();
                    count++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    error++;
                }



            }
            timer.Stop();

            Console.WriteLine("\nCalled Read() " + count + " times successfully,\n" +
                "encountered " + error + " validation errors, \n" +
                "took " + timer.ElapsedMilliseconds + "ms");

            xr.Close();
        
        }
    }
}

using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (var app = WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Server started at " + baseAddress);
                Console.WriteLine("Press [ENTER] to close...");
                Console.ReadLine();
            }
        }
    }
}

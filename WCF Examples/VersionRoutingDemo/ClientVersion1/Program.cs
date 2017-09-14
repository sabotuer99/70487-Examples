using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientVersion1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new ServiceReference1.ServiceClient())
            {

                Console.WriteLine(client.OperationA());
                Console.WriteLine(client.OperationB());
                Console.ReadLine();
            }
        }
    }
}

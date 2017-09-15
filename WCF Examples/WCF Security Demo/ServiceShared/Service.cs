using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceShared
{
    public class Service : IService
    {
        public string Echo(string message)
        {
            string result = "You said " + message;
            Console.WriteLine(result);
            return result;
        }
    }
}

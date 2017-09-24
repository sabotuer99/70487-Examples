using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceHosted
{
    public class EchoService : IEchoService
    {
        public string Echo(string message)
        {
            return "Windows Service says " + message + " back!";
        }
    }
}

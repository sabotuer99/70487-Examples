using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConService
{
    public class BaseContract : IConService
    {
        public BaseContract()
        {
            id = getId();
        }

        public string Status()
        {
            return id;
        }


        private static int count;
        private string id;

        private static string getId()
        {
            object thisLock = new object();
            lock (thisLock)
            {
                count++;
            }

            return count.ToString();
        }
    }
}

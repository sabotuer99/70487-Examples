using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels
{

    public class BankAccount : BillingDetail
    {
        public string BankName { get; set; }
        public string Swift { get; set; }
    }
}

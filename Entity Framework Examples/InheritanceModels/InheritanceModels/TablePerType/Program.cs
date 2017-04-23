using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePerType
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TBTContext context = new TBTContext())
            {
                context.BillingDetails.Add(new CreditCard());
                context.BillingDetails.Add(new BankAccount());
                context.BillingDetails.Add(new CreditCard());
                context.BillingDetails.Add(new BankAccount());
                context.SaveChanges();
            }

            Console.Read();
        }
    }
}

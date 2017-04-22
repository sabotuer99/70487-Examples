using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceModels
{
    public abstract class BillingDetail
    {
        public int BillingDetailId { get; set; }
        public string Owner { get; set; }
        public string Number { get; set; }
    }

    public class BankAccount : BillingDetail
    {
        public string BankName { get; set; }
        public string Swift { get; set; }
    }

    public class CreditCard : BillingDetail
    {
        public int CardType { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
    }

    public class TPHContext : DbContext
    {
        public DbSet<BillingDetail> BillingDetails { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {

        }
    }
}

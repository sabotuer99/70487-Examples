using DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePerType
{
    public class TBTContext : DbContext
    {
        //always write logs to console for demo purposes
        public TBTContext()
        {
            this.Database.Log = Console.WriteLine;
        }

        public DbSet<BillingDetail> BillingDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>().ToTable("BankAccounts");
            modelBuilder.Entity<CreditCard>().ToTable("CreditCards");
        }
    }
}

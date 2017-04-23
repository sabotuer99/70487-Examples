using DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TablePerClass
{
    public class TPCContext : DbContext
    {
        public DbSet<BillingDetail> BillingDetails { get; set; }

        //always write logs to console for demo purposes
        public TPCContext()
        {
            this.Database.Log = Console.WriteLine;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("BankAccounts");
            });

            modelBuilder.Entity<CreditCard>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("CreditCards");
            });

            modelBuilder.Entity<BillingDetail>()
            .Property(p => p.BillingDetailId)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }

        //you'd probably want to use some transaction logic in a real setting...
        public override int SaveChanges(){

            //get the maximum id, credit to http://stackoverflow.com/a/30257987
            int lastId = this.BillingDetails.DefaultIfEmpty().Max(r => r == null ? 0 : r.BillingDetailId);
            int nextId = lastId + 1;

            //grab each added item, give it next id, increment id
            foreach (var billingDetail in this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity as BillingDetail))
            {
                billingDetail.BillingDetailId = nextId;
                nextId++;
            }

            return base.SaveChanges();
        }
    }
}

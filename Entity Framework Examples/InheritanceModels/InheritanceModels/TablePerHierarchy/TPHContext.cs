using DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePerHierarchy
{
    public class TPHContext : DbContext
    {
        //always write logs to console for demo purposes
        public TPHContext()
        {
            this.Database.Log = Console.WriteLine;
        }


        public DbSet<BillingDetail> BillingDetails { get; set; }
    }
}

using AdventureWorks.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoEntitiesDemos
{
    partial class Program
    {
        public static void CommandTextTest()
        {
            var dbContext = new AdventureWorks2012Entities();
            ObjectContext context = ((IObjectContextAdapter)dbContext).ObjectContext;

            var custs = context.CreateObjectSet<Customer>().Where( c => c.Person.FirstName.Length < 4)
                            as ObjectQuery<Customer>;

            Console.WriteLine(custs.Count());

            Console.WriteLine(custs.CommandText);

            Console.WriteLine(custs.ToTraceString() + "\n\n");

            Console.WriteLine(dbContext.Customers.Where(c => c.Person.FirstName.Equals("Bob")));
        }
    }
}

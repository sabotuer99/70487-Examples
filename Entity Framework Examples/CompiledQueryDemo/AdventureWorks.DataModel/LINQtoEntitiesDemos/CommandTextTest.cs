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

            Console.WriteLine("CommandText:\n" + context.CreateObjectSet<Customer>()
                .Top("10").Where("FirstName = Bob").CommandText);

        }

        public static void LoggerTest()
        {
            var dbContext = new AdventureWorks2012Entities();

            dbContext.Database.Log = Console.WriteLine;

            dbContext.Customers.Where(c => c.Person.FirstName == "Bob").Count();

        }


        public static void ToStringTests()
        {
            var dbContext = new AdventureWorks2012Entities();
            ObjectContext context = ((IObjectContextAdapter)dbContext).ObjectContext;

            var custs = context.CreateObjectSet<Customer>()
                    .Where(c => c.Person.FirstName.Length < 4);

            Console.WriteLine(custs.Count());

            var custs_oq = custs as ObjectQuery<Customer>;

            Console.WriteLine("\n\nTraceString:\n" + custs_oq.ToTraceString() + "\n\n");
            Console.WriteLine("\n\nToString:\n" + custs.ToString() + "\n\n");

            Console.WriteLine(dbContext.Customers.Where(c => c.Person.FirstName.Equals("Bob")));


            //var result = from c in dbContext.Customers
            //             where 1 == 1
            //             select c;

            //Console.WriteLine(result.ToString());
        }


        public static Boolean isGood(Object thing)
        {
            return true;
        }
    }
}

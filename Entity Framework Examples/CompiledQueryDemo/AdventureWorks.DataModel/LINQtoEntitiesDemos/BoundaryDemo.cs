using AdventureWorks.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoEntitiesDemos
{
    partial class Program
    {
        static void Boundaries()
        {
            var context = new AdventureWorks2012Entities();
            context.Database.Log = notch;
            var custs = context.Customers;


            var query = custs
                .Where(c => c.Person.FirstName == "Bob")
                .OrderBy(c => c.rowguid);

            Console.WriteLine(query);
            Console.WriteLine(query.Count());
            Console.WriteLine("*****  END QUERYABLE  *****\n\n\n");

            var bad = custs.ToList().AsQueryable()
                .Where(c => c.Person != null && "Bob".Equals(c.Person.FirstName))
                .OrderBy(c => c.rowguid);

            Console.WriteLine(bad);
            Console.WriteLine(bad.Count());
            Console.WriteLine("*****  END ENUMERABLE (FINALLY...)  *****\n\n\n");
        }

        static int counter = 0;
        static void notch(object value)
        {
            if(counter % 100 == 0)
                Console.Write("x");
            counter++;
        }
    }
}

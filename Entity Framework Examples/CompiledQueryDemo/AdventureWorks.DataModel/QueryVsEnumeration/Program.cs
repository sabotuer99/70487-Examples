using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryVsEnumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            //this is pretty much straight out of Julie Lerman's PluralSight course
            //"Entity Framework in the Enterprise", module 4, clip 4 - "Non-tracking 
            //Alternatives to a Generic DbSet.Find"
            var item = Expression.Parameter(typeof(string), "s");
            var prop = Expression.Property(item, "Length");
            var value = Expression.Constant(4);
            var gt = Expression.GreaterThan(prop, value);
            var lambda = Expression.Lambda<Func<string, bool>>(gt, item);
            Console.WriteLine(lambda);


            var fruit = new List<string>() {"pear", "banana", "kiwi", "strawberry" };

            Console.WriteLine(fruit.AsQueryable().Where(lambda));
            Console.WriteLine(fruit.AsQueryable().Where(f => f.Length > 4));
            Console.WriteLine(fruit.AsEnumerable().Where(f => f.Length > 4));

            Console.Read();
        }
    }
}

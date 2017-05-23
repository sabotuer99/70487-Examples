using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void Transform()
        {
            var nums = Enumerable.Range(1, 10);
            var abcs = "ABCDEFGHIJ".ToCharArray();

            Console.WriteLine("Numbers: " + PrettyPrint(nums));
            Console.WriteLine("Letters: " + PrettyPrint(abcs));

            var joined = abcs.Take(4)
                .Aggregate("", (last, e) => last += last + e.ToString());
            Console.WriteLine("\nAggregate: " + joined);

            var zipped = nums.Zip(abcs, (n, l) => n.ToString() + l.ToString());
            Console.WriteLine("\nZipped: " + PrettyPrint(zipped));

            var concat = nums.Take(5).Concat(nums.Take(5));
            Console.WriteLine("\nConcat: " + PrettyPrint(concat));

            var listlist = new List<List<string>>(){
                new List<string>(){"The", "quick", "brown" },
                new List<string>(){"fox", "jumped", "over" },
                new List<string>(){"the", "lazy", "dog" }
            };

            Console.WriteLine("\n\n*** LIST OF LISTS ***");
            Console.WriteLine("\nPre-Flattened: \n" + PrettyPrint(listlist, true));
            Console.WriteLine("\nFlattened: \n" + PrettyPrint(listlist.SelectMany(s => s)));
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void Selection()
        {
            var seq = Enumerable.Range(1, 10);
            Console.WriteLine("     Sequence: " + PrettyPrint(seq));
            Console.WriteLine("# of elements: " + seq.Count());
            Console.WriteLine("   # of evens: " + seq.Count(e => e % 2 == 0));
            Console.WriteLine("first element: " + seq.First());
            Console.WriteLine(" last element: " + seq.Last());
            Console.WriteLine("  3rd element: " + seq.ElementAt(2));
            Console.WriteLine(" 99th element: " + seq.ElementAtOrDefault(98));
            Console.WriteLine("      first 3: " + PrettyPrint(seq.Take(3)));
            Console.WriteLine("    first < 6: " + PrettyPrint(seq.TakeWhile(e => e < 6)));

        }
    }
}

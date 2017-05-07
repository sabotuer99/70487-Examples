using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void SetOps()
        {
            var seq1 = "mustache".ToCharArray();
            var seq2 = "mustard".ToCharArray();
            var seq3 = "screwdriver".ToCharArray();

            Console.WriteLine("Sequence 1: " + PrettyPrint(seq1));
            Console.WriteLine("Sequence 2: " + PrettyPrint(seq2));
            Console.WriteLine("Sequence 3: " + PrettyPrint(seq3));
            Console.WriteLine("\n");

            Console.WriteLine("1 - 2: " + PrettyPrint(seq1.Except(seq2)));
            Console.WriteLine("2 - 1: " + PrettyPrint(seq2.Except(seq1)));
            Console.WriteLine("1 - 3: " + PrettyPrint(seq1.Except(seq3)));
            Console.WriteLine("2 - 3: " + PrettyPrint(seq2.Except(seq3)));
            Console.WriteLine("\n");

            Console.WriteLine("1 ∩ 2: " + PrettyPrint(seq1.Intersect(seq2)));
            Console.WriteLine("1 ∩ 3: " + PrettyPrint(seq1.Intersect(seq3)));
            Console.WriteLine("2 ∩ 3: " + PrettyPrint(seq2.Intersect(seq3)));
            Console.WriteLine("\n");

            Console.WriteLine("1 U 2: " + PrettyPrint(seq1.Union(seq2)));
            Console.WriteLine("1 U 3: " + PrettyPrint(seq1.Union(seq3)));
            Console.WriteLine("2 U 3: " + PrettyPrint(seq2.Union(seq3)));
            Console.WriteLine("\n");
        }
    }
}

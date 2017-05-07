using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void Numeric()
        {
            var sequence = new List<int>(){368, 506,  90, 340, 325, 
                                           635, 705, 759, 599,  79};

            var range = Enumerable.Range(0, 10);

            Console.WriteLine("Range(0, 10): " + PrettyPrint(range));
            Console.WriteLine("\n\nSequence: " + PrettyPrint(sequence));
            Console.WriteLine("     Max: " + sequence.Max());
            Console.WriteLine("     Min: " + sequence.Min());
            Console.WriteLine("    Mean: " + sequence.Average());
            Console.WriteLine("     Sum: " + sequence.Sum());

        }
    }
}

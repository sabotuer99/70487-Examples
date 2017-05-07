using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void Booleans()
        {
            var first = "The quick brown fox".Split(' ');
            var second = "jumped over the".Split(' ');
            var third = "lazy dog".Split(' ');

            Console.WriteLine("Part 1: " + PrettyPrint(first));
            Console.WriteLine("Part 2: " + PrettyPrint(second));
            Console.WriteLine("Part 3: " + PrettyPrint(third));
            Console.WriteLine("\n");
            Console.WriteLine("                       |  Part 1  |  Part 2  |  Part 3  |");
            Console.WriteLine("All words have 'e'?    |" +
                    Cell(first.All(w => w.Contains('e'))) + "|" +
                    Cell(second.All(w => w.Contains('e'))) + "|" +
                    Cell(third.All(w => w.Contains('e'))) + "|");
            Console.WriteLine("Any words have 'z'?    |" +
                    Cell(first.Any(w => w.Contains('z'))) + "|" +
                    Cell(second.Any(w => w.Contains('z'))) + "|" +
                    Cell(third.Any(w => w.Contains('z'))) + "|");
            Console.WriteLine("Contains 'the'?        |" +
                    Cell(first.Contains("the")) + "|" +
                    Cell(second.Contains("the")) + "|" +
                    Cell(third.Contains("the")) + "|");
            Console.WriteLine("Seq Equals 'lazy dog?' |" +
                    Cell(first.SequenceEqual(new List<string>() { "lazy", "dog" })) + "|" +
                    Cell(second.SequenceEqual(new List<string>() { "lazy", "dog" })) + "|" +
                    Cell(third.SequenceEqual(new List<string>() { "lazy", "dog" })) + "|");
            Console.WriteLine("Naive == 'lazy dog?'   |" +
                    Cell(first.Equals(new List<string>() { "lazy", "dog" })) + "|" +
                    Cell(second.Equals(new List<string>() { "lazy", "dog" })) + "|" +
                    Cell(third.Equals(new List<string>() { "lazy", "dog" })) + "|");

        }
    }
}

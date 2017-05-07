using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            //Numeric();
            //Sorting();
            //SetOps();
            //InitSeq();
            //Booleans();

            Console.Read();
        }

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
                    Cell(second.All(w => w.Contains('e')))+ "|" +
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
                    Cell(first.SequenceEqual(new List<string>(){"lazy","dog"})) + "|" +
                    Cell(second.SequenceEqual(new List<string>(){"lazy","dog"})) + "|" +
                    Cell(third.SequenceEqual(new List<string>(){"lazy","dog"})) + "|");
            Console.WriteLine("Naive == 'lazy dog?'   |" +
                    Cell(first.Equals(new List<string>() { "lazy", "dog" })) + "|" +
                    Cell(second.Equals(new List<string>() { "lazy", "dog" })) + "|" +
                    Cell(third.Equals(new List<string>() { "lazy", "dog" })) + "|");

        }





        private static string Cell(bool p)
        {
            return (p.ToString().PadRight(7)).PadLeft(10);
        }



        static void InitSeq()
        {
            var repeat = Enumerable.Repeat("A", 5);
            var empty = Enumerable.Empty<String>();

            Console.WriteLine("Repeat: " + PrettyPrint(repeat));
            Console.WriteLine("Empty: " + PrettyPrint(empty));
        }

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

        private class Person
        {
            public Person(string name, DateTime bday)
            {
                Name = name;
                Birthday = bday;
            }
            public string Name { get; set; }
            public DateTime Birthday { get; set; }

            public override string ToString()
            {
                return Name + ": " + Birthday.ToShortDateString();
            }
        }

        static void Sorting()
        {
            var sequence = new List<Person>(){
                new Person("Bobby Tables", new DateTime(1982, 1, 1)),
                new Person("Susan Strong", new DateTime(1977, 8, 22)),
                new Person("Multiple Man", new DateTime(2005, 4, 30)),
                new Person("Multiple Man", new DateTime(2015, 1, 5)),
                new Person("Multiple Man", new DateTime(1995, 11, 16)),
                new Person("Multiple Man", new DateTime(1962, 9, 8)),
                new Person("Finn the Human", new DateTime(2001, 12, 5)),
                new Person("Steve Bobfish", new DateTime(2014, 6, 14))
            };

            Console.WriteLine("Base sequence:\n" + PrettyPrint(sequence, true));

            var sorted = sequence.OrderBy(p => p.Name).ThenByDescending(p => p.Birthday);
            Console.WriteLine("\nSorted by Name, then Birthday:\n" + PrettyPrint(sorted, true));

            var rev = sorted.Reverse();
            Console.WriteLine("\nReverse of sorted list:\n" + PrettyPrint(rev, true));

            var skipped = rev.Skip(2);
            Console.WriteLine("\nSkip first two of reversed:\n" + PrettyPrint(skipped, true));

            var skipMulti = skipped.SkipWhile(p => p.Name.Equals("Multiple Man"));
            Console.WriteLine("\nSkip 'Multiple Man' persons:\n" + PrettyPrint(skipMulti, true));

        }

        private static string PrettyPrint<T>(IEnumerable<T> sequence)
        {
            return PrettyPrint(sequence, false);
        }


        private static string PrettyPrint<T>(IEnumerable<T> sequence, bool newlines)
        {
            
            string newline = "";
            string indent = "";
            if (newlines)
            {
                newline = "\n";
                indent = "    ";
            }
            string seperator = ", " + newline;

            return "[" + newline + string.Join(seperator, sequence.Select(x => indent + x.ToString()).ToArray()) + newline + "]";
        }


    }
}

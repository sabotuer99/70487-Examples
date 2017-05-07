using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void Filters()
        {
            var people = new List<Person>(){
                new SuperHero("Iron Man", "Tony Stark", new DateTime(1970, 5, 29)),
                new SuperHero("Batman", "Bruce Wayne", new DateTime(1940, 4, 25)),
                new Person("Jake the Dog", new DateTime(1995, 5, 30)),
                new Person("Marcelline Anthonsen", new DateTime(1984, 1, 27)),
                new Person("Marcelline Anthonsen", new DateTime(1984, 1, 27)),
                new Person("Steve Bobfish III", new DateTime(2017, 4, 30))
            };

            Console.WriteLine("Population:\n" + PrettyPrint(people, true));

            var deduped = people.Distinct();
            Console.WriteLine("\nNo duplicates:\n" + PrettyPrint(deduped, true));

            var heros = people.OfType<SuperHero>();
            Console.WriteLine("\nOnly superheros:\n" + PrettyPrint(heros, true));

            var heroSeq = people.OfType<SuperHero>()
                .Cast<SuperHero>().Select(h => h.HeroName);
            var heroSeq2 = people.OfType<SuperHero>().Select(h => h.HeroName);
            Console.WriteLine("\nOnly hero names: " + PrettyPrint(heroSeq));
            Console.WriteLine("      No 'Cast': " + PrettyPrint(heroSeq2));

            var fancyNames = people.Where(p => p.Name.Split(' ').Length >= 3);
            Console.WriteLine("\nFancy names:\n" + PrettyPrint(fancyNames, true));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    partial class Program
    {
        static void JoinGroup()
        {
            var heroTypes = new List<HeroType>(){
                new HeroType(1, "Hero"),
                new HeroType(2, "Anti-hero"),
                new HeroType(3, "Villian")
            };

            var characters = new List<SuperHero>(){
                new SuperHero("Iron Man", 1, "Tony Stark", new DateTime()),
                new SuperHero("Superman", 1, "Clark Kent", new DateTime()),
                new SuperHero("Wolverine", 1, "Logan ???", new DateTime()),
                new SuperHero("Deadpool", 2, "Wade Wilson", new DateTime()),
                new SuperHero("Punisher", 2, "Frank Castle", new DateTime()),
                new SuperHero("Loki", 3, "Loki Laufeyson", new DateTime()),
                new SuperHero("Joker", 3, "Jack Napier", new DateTime()),
                new SuperHero("Ozymandias", 4, "Adrian Veidt", new DateTime()),
            };

            Console.WriteLine("Hero Types: " + PrettyPrint(heroTypes, true));
            Console.WriteLine("\nCharacters: " + PrettyPrint(characters, true));

            var grouped = characters.GroupBy(k => k.HeroTypeId)
                .Select(g => g.Key.ToString() + ": " + g.Count().ToString());
            Console.WriteLine("\nGrouped: " + PrettyPrint(grouped, true));

            var gjoined = heroTypes.GroupJoin(
                 characters,
                 ht => ht.Id,
                 h => h.HeroTypeId,
                 (ht, h) => new { ht.TypeName, Heros = h })
                .Select(g => g.TypeName + ": " + PrettyPrint(g.Heros.Select(h => h.HeroName)));
            Console.WriteLine("\nGroupJoined: " + PrettyPrint(gjoined, true));

            var join = characters.Join(heroTypes,
                    ch => ch.HeroTypeId,
                    ht => ht.Id,
                    (ch, ht) => new { ch.HeroName, ht.TypeName })
                    .Select(r => r.HeroName + ": " + r.TypeName);

            Console.WriteLine("\nJoined: " + PrettyPrint(join, true));

            var leftjoin = characters
                .GroupJoin(
                    heroTypes,
                    hero => hero.HeroTypeId,
                    ht => ht.Id,
                    (hero, hts) => new { hero, hts })
                .SelectMany(
                    xy => xy.hts.DefaultIfEmpty(new HeroType(0, "???")),
                    (x, y) => new { HeroName = x.hero.HeroName, Allignment = y.TypeName })
                .Select(
                    s => s.HeroName + ": " + s.Allignment);

            Console.WriteLine("\nLeft Join: " + PrettyPrint(leftjoin, true));
        }
    }
}

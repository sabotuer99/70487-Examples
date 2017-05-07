using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemos
{
    public class SuperHero : Person
    {
        public SuperHero(string heroName, string ordinaryName, DateTime birthday) :
            base(ordinaryName, birthday)
        {
            HeroName = heroName;
        }

        public string HeroName { get; set; }

        public override string ToString()
        {
            return "(" + HeroName + ") " + base.ToString();
        }
    }
}

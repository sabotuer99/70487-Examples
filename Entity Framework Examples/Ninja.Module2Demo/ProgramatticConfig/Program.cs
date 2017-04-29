using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramatticConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new PrgNinjaContext())
            {
                var ninjas = ctx.Ninjas.ToList();

                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Id + ": " + ninja.Name);
                }
            }

            Console.Read();
        }
    }

    [DbConfigurationType(typeof(PrgNinjaDbConfiguration))]
    public class PrgNinjaContext : NinjaContext {}
}

using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AsyncDemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SyncOperations();
            AsyncOperations();



        }

        public static void SyncOperations() 
        {
            PerformDatabaseOperations();

            Console.WriteLine();
            Console.WriteLine("Quote of the day");
            Console.WriteLine(" Don't worry about the world coming to an end today... ");
            Console.WriteLine(" It's already tomorrow in Australia.");

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

        public static void AsyncOperations() 
        {
            var task = PerformDatabaseOperationsAsync();

            Console.WriteLine("Quote of the day");
            Console.WriteLine(" Don't worry about the world coming to an end today... ");
            Console.WriteLine(" It's already tomorrow in Australia.");

            task.Wait();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static async Task PerformDatabaseOperationsAsync()
        {
            using (var db = new NinjaContext())
            {
                // Create a new blog and save it 
                var clan = db.Clans.First();

                db.Ninjas.Add(new Ninja
                {
                    Name = "Ninja #" + (db.Ninjas.Count() + 1),
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1977, 1, 1),
                    Clan = clan
                });


                Console.WriteLine("Calling SaveChanges.");
                await db.SaveChangesAsync();
                Console.WriteLine("SaveChanges completed.");

                // Query for all blogs ordered by name 
                Console.WriteLine("Executing query.");
                var ninjas = await (from n in db.Ninjas
                                   orderby n.Name
                                   select n).ToListAsync();

                // Write all blogs out to Console 
                Console.WriteLine("Query completed with following results:");
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(" - " + ninja.Name);
                }
            }
        } 

        public static void PerformDatabaseOperations()
        {
            using (var db = new NinjaContext())
            {

                var clan = new Clan
                {
                    ClanName = "Clan #" + (db.Clans.Count() + 1)
                };

                db.Ninjas.Add(new Ninja
                {
                    Name = "Ninja #" + (db.Ninjas.Count() + 1),
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1977, 1, 1),
                    Clan = clan
                });
                db.SaveChanges();

                var ninjas = (from n in db.Ninjas
                             orderby n.Name
                             select n).ToList();

                // Write all blogs out to Console 
                Console.WriteLine();
                Console.WriteLine("All ninjas:");
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(" " + ninja.Name);
                }
            }
        }
    }
}

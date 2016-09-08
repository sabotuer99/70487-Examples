using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());

            DeleteAllNinjas();
            pause();

            InsertNinja();
            pause();

            InsertNinjaWithEquipment();
            pause();

            SimpleNinjaGraphQueryEager();
            pause();

            SimpleNinjaGraphQueryExplicit();
            pause();

            SimpleNinjaGraphQueryLazy();
            pause();

            ProjectionQuery();
            pause();
            
              
            SimpleNinjaQueries();
            pause();

            QueryAndUpdateNinja();
            pause();

            QueryAndUpdateNinjaDisconnected();
            pause();

            DeleteNinja();
            pause();

            DeleteNinjaDisconnected();
            pause();

            DeleteNinjaWithKeyValue();
            pause();

            DeleteNinjaViaStoredProcedure();
            pause();
        }

        private static void pause()
        {
            Console.In.Read();
            Console.In.Read();
            Console.Clear();
        }

        private static void ProjectionQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas
                    .Select(n => new {n.Name, n.DateOfBirth, n.EquipmentOwned})
                    .ToList();
            }
        }

        private static void SimpleNinjaGraphQueryLazy()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));

                Console.Out.WriteLine("Ninja Retrieved");

                Console.WriteLine(
                    "Ninja Equipment Count: {0}", ninja.EquipmentOwned.Count());
            }
        }

        private static void SimpleNinjaGraphQueryExplicit()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));

                Console.Out.WriteLine("Ninja Retrieved");

                context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
            }
        }

        private static void SimpleNinjaGraphQueryEager()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas.Include(n => n.EquipmentOwned)
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));
            }
        }

        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var clan = context.Clans.FirstOrDefault();

                var ninja = new Ninja()
                {
                    Name = "Kacy Catanzaro",
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1990, 1, 14),
                    Clan = clan
                };

                var muscles = new NinjaEquipment()
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool
                };

                var spunk = new NinjaEquipment()
                {
                    Name = "Spunk",
                    Type = EquipmentType.Weapon
                };

                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(muscles);
                ninja.EquipmentOwned.Add(spunk);
                context.SaveChanges();
            }
        }

        private static void DeleteAllNinjas()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("DELETE FROM Ninjas; DELETE FROM Clans");
            }
        }

        private static void DeleteNinjaViaStoredProcedure()
        {
            //SETUP
            int keyval;
            using (var context = new NinjaContext())
            {
                keyval = context.Ninjas.FirstOrDefault().Id;
            }
            //END SETUP

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("exec DeleteNinjaViaId {0}", keyval);
            }

            /* Script to create the stored procedure...
             * 
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO

                CREATE PROCEDURE DeleteNinjaViaId
                    @Id int
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE
                    FROM Ninjas
                    WHERE Ninjas.Id = @Id
                END
                GO
             *  
             */
        }

        private static void DeleteNinjaWithKeyValue()
        {
            //SETUP
            int keyval;
            using (var context = new NinjaContext())
            {
               keyval = context.Ninjas.FirstOrDefault().Id;
            }
            //END SETUP

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }

        private static void DeleteNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }

        private static void DeleteNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Entry(ninja).State = EntityState.Deleted; //Attached automatically by Entry()
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = !ninja.ServedInOniwaban;
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.ServedInOniwaban = !ninja.ServedInOniwaban;

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //context.Ninjas.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified; //Attached automatically by Entry()
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                    .Where(n => n.DateOfBirth >= new DateTime(1984, 1, 1));

                Console.Out.WriteLine("Query created, not executed yet...");

                foreach (var ninja in ninjas) {
                    Console.Out.WriteLine(ninja.Name);
                }

            }
        }

        private static void InsertNinja()
        {
            var clan = new Clan
            {
                ClanName = "Vermont Ninjas",
                Id = 1               
            };

            var ninja0 = new Ninja
            {
                Name = "JulieSan",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1980, 1, 1),
                Clan = clan
            };
            var ninja1 = new Ninja
            {
                Name = "SampsonSan",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(2008, 1, 28),
                Clan = clan
            };
            var ninja2 = new Ninja
            {
                Name = "Leonardo",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1985, 11, 11),
                Clan = clan
            };
            var ninja3 = new Ninja
            {
                Name = "Raphael",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1985, 11, 5),
                Clan = clan
            };
            using (var context = new NinjaContext()) {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new Ninja[]{ninja0, ninja1, ninja2, ninja3});
                context.SaveChanges();
            }
        }
    }
}

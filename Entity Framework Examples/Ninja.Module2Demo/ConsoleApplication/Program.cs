using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using nob = NinjaDomain.OldStyleContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;

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


            Console.Out.WriteLine("##########################################");
            Console.Out.WriteLine("Begin Raw SQL Query");
            SimpleNinjaQueryRawSQL();
            Console.Out.WriteLine("\n\n##########################################");
            Console.Out.WriteLine("Begin Entity SQL Query using DataReader");
            SimpleNinjaQueryEntitySQL_Reader();
            Console.Out.WriteLine("\n\n##########################################");
            Console.Out.WriteLine("Begin Entity SQL Query using ObjectContext");
            SimpleNinjaQueryEntitySQL_ObjCtxQuery();
            pause();

            //SimpleNinjaCompiledQuery();
            CompiledQueryPerformanceTest();
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
                    .Select(n => new { n.Name, n.DateOfBirth, n.EquipmentOwned })
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

        private static void SimpleNinjaQueryRawSQL()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninjas = context.Ninjas
                    .SqlQuery("SELECT * FROM dbo.Ninjas WHERE DateOfBirth > {0}", 
                        new object[]{new DateTime(1984,1,1)}).ToList();

                foreach (Ninja ninja in ninjas)
                {
                    Console.Out.WriteLine(ninja.Name + " " + ninja.DateOfBirth);
                }
            }
        }

        private static void SimpleNinjaQueryEntitySQL_Reader()
        {
            using (EntityConnection conn = new EntityConnection("name=NinjaObjectContext"))
            {
                //context.Database.Log = Console.WriteLine;
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT VALUE n 
                                    FROM NinjaObjectContext.Ninjas AS n
                                    WHERE n.DateOfBirth > @dob";
                cmd.Parameters.AddWithValue("dob", new DateTime(1984, 1, 1));


                using (EntityDataReader dr = cmd.ExecuteReader(
                    System.Data.CommandBehavior.SequentialAccess)) 
                {
                    while (dr.Read()) 
                    {
                        Console.Out.WriteLine(dr.GetString(1) + " " + dr.GetDateTime(4));
                    }
                }
            }
        }

        private static void SimpleNinjaQueryEntitySQL_ObjCtxQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var adapter = (IObjectContextAdapter)context;
                var objctx = adapter.ObjectContext;
                var param = new ObjectParameter("dob", new DateTime(1984, 1, 1));
                ObjectQuery<Ninja> ninjas = objctx.CreateQuery<Ninja>(
                        @"SELECT VALUE n 
                          FROM NinjaContext.Ninjas AS n
                          WHERE n.DateOfBirth > @dob",
                        new ObjectParameter[] { param });

                foreach (Ninja ninja in ninjas)
                {
                    Console.Out.WriteLine(ninja.Name + " " + ninja.DateOfBirth);
                }
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

                /*  //throw away example code
                var parameters = new object[1];
                //add all your parameters in here
                context.Ninjas.SqlQuery("GetNinjasByX", parameters);
                context.Database.SqlQuery<Ninja>("GetNinjasByX", parameters);
                context.Database.SqlQuery(typeof(Ninja), "GetNinjasByX", parameters);
                */

            }
        }

        static readonly Func<nob.NinjaObjectContext, DateTime, IQueryable<nob.Ninja>> s_compiledNinjaQuery =
            CompiledQuery.Compile<nob.NinjaObjectContext, DateTime, IQueryable<nob.Ninja>>(
                    (ctx, date) => from ninja in ctx.Ninjas
                                   where ninja.DateOfBirth < date
                                   select ninja);

        private static void SimpleNinjaCompiledQuery()
        {
            using (var context = new nob.NinjaObjectContext())
            {
                var oldninjas = s_compiledNinjaQuery.Invoke(context, new DateTime(1982, 1, 1));

                foreach (nob.Ninja ninja in oldninjas)
                {
                    Console.Out.WriteLine(ninja.Name);
                }
            }
        }

        private static void CompiledQueryPerformanceTest()
        {
            for (var j = 0; j < 5; j++)
            {
                using (var context = new nob.NinjaObjectContext())
                {
                    var start = DateTime.Now;
                    for (var i = 0; i < 10000; i++)
                    {
                        var result1 = s_compiledNinjaQuery.Invoke(context, new DateTime(1982, 1, 1));
                    }
                    double ms = (DateTime.Now - start).TotalMilliseconds;
                    Console.Out.WriteLine("Compiled query : " + ms.ToString() + "ms");
                }

                using (var ctx = new nob.NinjaObjectContext())
                {
                    var start = DateTime.Now;
                    var date = new DateTime(1982, 1, 1);
                    for (var i = 0; i < 10000; i++)
                    {
                        var result2 = from ninja in ctx.Ninjas
                                      where ninja.DateOfBirth < date
                                      select ninja;
                    }
                    double ms = (DateTime.Now - start).TotalMilliseconds;
                    Console.Out.WriteLine("Non-Compiled query : " + ms.ToString() + "ms");
                }

                using (var ctx = new NinjaContext())
                {
                    var start = DateTime.Now;
                    var date = new DateTime(1982, 1, 1);
                    for (var i = 0; i < 10000; i++)
                    {
                        var result2 = from ninja in ctx.Ninjas
                                      where ninja.DateOfBirth < date
                                      select ninja;
                    }
                    double ms = (DateTime.Now - start).TotalMilliseconds;
                    Console.Out.WriteLine("DbContext : " + ms.ToString() + "ms");
                }

                Console.Out.WriteLine("#######################");
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

                foreach (var ninja in ninjas)
                {
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
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new Ninja[] { ninja0, ninja1, ninja2, ninja3 });
                context.SaveChanges();
            }
        }
    }
}

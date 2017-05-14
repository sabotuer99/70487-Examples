using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks.DataModel;
using System.Data.Entity;
using System.Threading;

namespace LINQtoEntitiesDemos
{
    partial class Program
    {
        static void AsyncDemo()
        {
            var tasks = new List<Task<int>>();
            for (int i = 0; i < 10; i++)
            {
                int iteration = i;
                tasks.Add(Task.Factory.StartNew<int>(() =>
                {
                    using (var context = new AdventureWorks2012Entities())
                    {
                        context.Database.Log = Console.WriteLine;
                        Console.WriteLine("Starting task " + iteration);
                        int result = context.Customers
                            .Where(c => c.CustomerID % 10 == iteration)
                            .Count();
                        Console.WriteLine("Finished computing task " + iteration);
                        return result;
                    }
                }));
            }

            var results = new int[10];
            for (int i = 0; i < 10; i++)
            {
                results[i] = tasks[i].Result;
            }

            Console.WriteLine("[" + String.Join(", ", results) + "]");
        }

        static void AsyncDemo2()
        {
            var tasks = new List<Task<int>>();
            for (int i = 0; i < 26; i++)
            {
                string firstLetter = ((char)((int)'A' + i)).ToString();
                tasks.Add(Task.Factory.StartNew<int>(() =>
                {
                    using (var context = new AdventureWorks2012Entities())
                    {
                        //context.Database.Log = Console.WriteLine;
                        Console.WriteLine("Starting task " + firstLetter);
                        int result = context.People
                            .Where(p => p.FirstName.StartsWith(firstLetter))
                            .Count();
                        Console.WriteLine("Finished computing task " + firstLetter);
                        return result;
                    }
                }));
            }

            var results = new string[26];
            for (int i = 0; i < 26; i++)
            {
                string firstLetter = ((char)((int)'A' + i)).ToString();
                results[i] = firstLetter + ": " + tasks[i].Result;
            }

            Console.WriteLine("[" + String.Join(", ", results) + "]");
        }

        static void AsyncDemo3()
        {
            var tasks = new List<Task<int>>();
            for (int i = 0; i < 26; i++)
            {
                string firstLetter = ((char)((int)'A' + i)).ToString();
                tasks.Add(Task<int>.Run(async () =>
                {
                    using (var context = new AdventureWorks2012Entities())
                    {
                        Console.WriteLine("Starting task " + firstLetter);
                        int result = await context.People
                            .Where(p => p.FirstName.StartsWith(firstLetter))
                            .CountAsync();
                        Console.WriteLine("Finished computing task " + firstLetter);
                        return result;
                    }
                }));
                //Thread.Sleep(500);
            }

            var results = new string[26];
            for (int i = 0; i < 26; i++)
            {
                string firstLetter = ((char)((int)'A' + i)).ToString();
                results[i] = firstLetter + ": " + tasks[i].Result;
            }

            Console.WriteLine("[" + String.Join(", ", results) + "]");
        }
    }
}

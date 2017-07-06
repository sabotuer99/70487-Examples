using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = 
                @"***.redis.cache.windows.net:6380,password=***,ssl=True,abortConnect=False";

            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(connectionString);

            // Connection refers to a property that returns a ConnectionMultiplexer
            // as shown in the previous example.
            IDatabase cache = connection.GetDatabase();
            ISet<string> keys = new HashSet<string>();

            for(string key = " "; key != ""; )
            {
                Console.Write("\n\nEnter a key (empty to quit): ");
                key = Console.ReadLine();
                if(key != "")
                {
                    Console.Write("\nEnter a value: ");
                    string value = Console.ReadLine();

                    cache.StringSet(key, value);
                    keys.Add(key);
                }

                Console.WriteLine("\nCurrent Values in the cache: ");
                foreach(string k in keys)
                {
                    if (cache.KeyExists(k))
                    {
                        Console.WriteLine("    " + k + ": " + cache.StringGet(k));
                    }
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CachingSample
{
    class Program
    {
        static ObjectCache cache = MemoryCache.Default;

        static void Main(string[] args)
        {
            UpdateCache();

            Console.WriteLine("Waiting for caching changes");
            Console.Read();
        }

        static void CacheItemRemoved(CacheEntryRemovedArguments args)
        {
            Console.WriteLine(string.Format("Item %s removed because %s", 
                args.CacheItem.Key, args.RemovedReason));

            UpdateCache();
        }

        static void CacheItemUpdated(CacheEntryUpdateArguments args)
        {
            Console.WriteLine(string.Format("Item %s updated because %s",
                args.Key, args.RemovedReason));
        }

        static void UpdateCache()
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Person.Person WHERE PersonType = 'EM'");
            SqlDependency sqlDependency = new SqlDependency(command);
            sqlDependency.OnChange += new OnChangeEventHandler((x, y) => Console.WriteLine("Change detected!"));

            ChangeMonitor sqlMonitor = new SqlChangeMonitor(sqlDependency);
            sqlMonitor.NotifyOnChanged(x => Console.WriteLine("Changed!!"));

            var policy = new CacheItemPolicy();
            //policy.RemovedCallback = CacheItemRemoved;
            //policy.UpdateCallback = CacheItemUpdated;
            

            var empCount = GetCurrentEmployeeCount();
            cache.Set("employee_count", empCount, policy);
            policy.ChangeMonitors.Add(sqlMonitor);

            Console.WriteLine(string.Format("Current Employee count: %f", empCount));
        }

        static int GetCurrentEmployeeCount()
        {
            var connectionString = 
                ConfigurationManager.ConnectionStrings["Default"]
                .ConnectionString;
            var conn = new SqlConnection(connectionString);

            var cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT COUNT(*) FROM Person.Person WHERE PersonType = 'EM'";
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
                
            var dr = (int) cmd.ExecuteScalar();

            conn.Close();

            return dr;
        }
    }
}

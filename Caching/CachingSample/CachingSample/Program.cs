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

        static void UpdateCache()
        {

            var policy = new CacheItemPolicy();

            policy.RemovedCallback = (s) => UpdateCache();

            var connectionString =
                ConfigurationManager.ConnectionStrings["Default"]
                .ConnectionString;

            SqlDependency.Start(connectionString);

            using (var conn = new SqlConnection(connectionString))
            {

                using (SqlCommand command =
                    new SqlCommand("SELECT PersonType FROM Person.Person WHERE PersonType = 'EM'",
                    conn))
                {
                    SqlDependency sqlDependency = new SqlDependency();
                    sqlDependency.AddCommandDependency(command);

                    //sqlDependency.OnChange += (s, x) => UpdateCache();

                    ChangeMonitor sqlMonitor = new SqlChangeMonitor(sqlDependency);

                    conn.Open();
                    command.ExecuteScalar();

                    var empCount = GetCurrentEmployeeCount();
                    policy.ChangeMonitors.Add(sqlMonitor);
                    cache.Add("employee_count", empCount, policy);

                    Console.WriteLine(string.Format("Updated Employee count: {0}", empCount));

                }
            }
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

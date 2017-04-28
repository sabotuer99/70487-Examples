using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //grab (Sql) connection from context
            using (var context = new NinjaContext())
            {
                using (var conn = context.Database.Connection)
                {
                    var cmd = conn.CreateCommand();
                    PrintResults(conn, cmd, "SELECT * FROM [dbo].[Ninjas]");
                }
            }

            //manually build connection
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
            connBuilder.InitialCatalog = "NinjaDomain.DataModel.NinjaContext";
            connBuilder.DataSource = ".\\SQLEXPRESS";
            connBuilder.IntegratedSecurity = true;

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
            efBuilder.Provider = "System.Data.SqlClient";
            efBuilder.ProviderConnectionString = connBuilder.ToString();
            efBuilder.Metadata = @"res://NinjaDomain.OldStyleContext/NinjaObjectContext.csdl|" +
                                 @"res://NinjaDomain.OldStyleContext/NinjaObjectContext.ssdl|" +
                                 @"res://NinjaDomain.OldStyleContext/NinjaObjectContext.msl";

            using (var conn = new EntityConnection(efBuilder.ToString()))
            {
                var cmd = conn.CreateCommand();
                PrintEntityResults(conn, cmd, 
                    "SELECT Ninjas FROM NinjaObjectContext.Ninjas WHERE Ninjas.Id > 0");
            }

            Console.ReadLine();
        }

        //Sql Provider
        private static void PrintResults(DbConnection conn, DbCommand cmd, 
            string commandText)
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = commandText;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var dr = cmd.ExecuteReader();
            var ninjas = new DataTable();
            ninjas.Load(dr);

            foreach (DataRow row in ninjas.Rows)
            {
                Console.WriteLine(row["Id"] + ": " + row["Name"]);
            }
        }

        //Entity Provider
        private static void PrintEntityResults(DbConnection conn, DbCommand cmd, 
            string commandText)
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = commandText;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var dr = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            while(dr.Read()){
                DbDataRecord record = (DbDataRecord)dr[0];
                Console.WriteLine(record.GetInt32(0) + ": " + record.GetString(1));
            }
        }
    }
}

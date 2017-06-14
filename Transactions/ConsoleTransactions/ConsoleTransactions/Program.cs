using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ConsoleTransactions
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (var conn = new SqlConnection(connectionString))
            {
                var tx = new CommittableTransaction();
                Transaction.Current = tx;
                try
                {
                    SqlCommand cmd = new SqlCommand(
                            @"SELECT COUNT(*) FROM Person.Person WHERE PersonType = 'EM'", conn);

                    var count = (int)cmd.ExecuteScalar();

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                }
            }
        }
    }
}

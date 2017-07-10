using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ConsoleTransactionScope
{
    class Program
    {
        static void Main(string[] args)
        {
            //var txOptions = new TransactionOptions();
            //txOptions.IsolationLevel = IsolationLevel.ReadCommitted;

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOptions))
            using (TransactionScope scope = new TransactionScope())
            {
                string connStandard =
                    ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                string connExpress =
                    ConfigurationManager.ConnectionStrings["Express"].ConnectionString;
                using (var conn = new SqlConnection(connStandard))
                {
                    conn.Open();

                    try
                    {
                        SqlCommand cmd = new SqlCommand(
                                @"SELECT COUNT(*) FROM Person.Person WHERE PersonType = 'EM'", conn);

                        var count = (int)cmd.ExecuteScalar();

                        //open up a second connection to a different database
                        using (var conn2 = new SqlConnection(connExpress))
                        {
                            conn2.Open();
                            SqlCommand cmd2 = new SqlCommand(
                                @"SELECT COUNT(*) FROM Person.Person WHERE PersonType = 'EM'", conn2);

                            var count2 = (int)cmd2.ExecuteScalar();
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}

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
                conn.Open();

                var txOptions = new TransactionOptions();
                txOptions.IsolationLevel = IsolationLevel.ReadCommitted;
                var tx = new CommittableTransaction(txOptions);
                conn.EnlistTransaction(tx);

                try
                {
                    
                    SqlCommand cmd = new SqlCommand(
                            @"SELECT COUNT(*) FROM Person.Person WHERE PersonType = 'EM'", conn);

                    var count = (int)cmd.ExecuteScalar();

                    tx.Commit();
                }
                catch(Exception ex)
                {
                    tx.Rollback();
                }
                tx.Dispose(); //dispose CommitableTransaction to avoid an exception

                var sqlTx = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {

                    SqlCommand cmd = new SqlCommand(
                            @"SELECT COUNT(*) FROM Person.Person WHERE PersonType = 'EM'", conn);
                    cmd.Transaction = sqlTx;  //transaction must be attached to command in this case

                    var count = (int)cmd.ExecuteScalar();

                    sqlTx.Commit();
                }
                catch (Exception ex)
                {
                    sqlTx.Rollback();
                }
            }
        }
    }
}

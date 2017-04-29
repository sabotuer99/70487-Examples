using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProgramatticConfig
{
    public class PrgNinjaDbConfiguration : DbConfiguration
    {
        public PrgNinjaDbConfiguration()
        {
            this.SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());

            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
            connBuilder.InitialCatalog = "NinjaDomain.DataModel.NinjaContext";
            connBuilder.DataSource = ".\\SQLEXPRESS";
            connBuilder.IntegratedSecurity = true;

            DumbConnectionFactory scf = new DumbConnectionFactory(connBuilder.ToString());
            //SqlConnectionFactory scf = new SqlConnectionFactory(connBuilder.ToString());

            this.SetDefaultConnectionFactory(scf);
            this.SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        } 
    }

    public class DumbConnectionFactory : IDbConnectionFactory
    {
        public DumbConnectionFactory(string connString)
        {
            this.connString = connString;
        }

        public string connString { get; set; }

        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            return new SqlConnection(connString);
        }
    }
}

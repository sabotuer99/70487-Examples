using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    public class ViewModel
    {
        static ObjectCache cache = MemoryCache.Default;
        private ObservableCollection<string> _listSource = new ObservableCollection<string>();
        public ObservableCollection<string> listSource
        {
            get
            {
                return _listSource;
            }
            set
            {
                _listSource = value;
            }
        }


        public ViewModel()
        {
            //UpdateCache();
        }

        public void UpdateCache()
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
                    SqlDependency sqlDependency = new SqlDependency(command);
                    //sqlDependency.AddCommandDependency();

                    //sqlDependency.OnChange += (s,x) => UpdateCache();

                    ChangeMonitor sqlMonitor = new SqlChangeMonitor(sqlDependency);

                    conn.Open();
                    command.ExecuteScalar();

                    var empCount = GetCurrentEmployeeCount();
                    policy.ChangeMonitors.Add(sqlMonitor);
                    cache.Add("employee_count", empCount, policy);

                    UpdateList(empCount);

                }
            }
        }

        public void UpdateList(int empCount)
        {
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    listSource.Add(
                        string.Format("Updated Employee count: {0}", empCount));
                });
        }

        private void SqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        int GetCurrentEmployeeCount()
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

            var dr = (int)cmd.ExecuteScalar();

            conn.Close();

            return dr;
        }
    }
}

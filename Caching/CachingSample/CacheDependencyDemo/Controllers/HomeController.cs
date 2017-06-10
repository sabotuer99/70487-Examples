using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace CacheDependencyDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cache = HttpContext.Cache;

            if(cache["employee_count"] == null)
            {
                UpdateCache();
            }

            ViewBag.cached = cache["employee_count"];
            ViewBag.requested = "Requested at : " + DateTime.Now.ToLongTimeString();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private int GetCurrentEmployeeCount()
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["AdventureWorks"]
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

        private void UpdateCache()
        { 
            var connectionString =
                ConfigurationManager.ConnectionStrings["AdventureWorks"]
                .ConnectionString;
            SqlDependency.Start(connectionString);

            using (var conn = new SqlConnection(connectionString))
            {

                using (SqlCommand command =
                    new SqlCommand("SELECT PersonType FROM Person.Person WHERE PersonType = 'EM'",
                    conn))
                {
                    var empCount = GetCurrentEmployeeCount();

                    string key = "employee_count";
                    string value = "Count: " + empCount + ", as of " + DateTime.Now.ToLongTimeString();
                    SqlCacheDependency dep = new SqlCacheDependency("AdventureWorks", "Person");
                    DateTime exp = DateTime.Now.AddMinutes(5);
                    conn.Open();
                    command.ExecuteScalar();

                    HttpContext.Cache
                        .Add(key, value, dep, exp, 
                        Cache.NoSlidingExpiration, 
                        CacheItemPriority.Default, CallBack);
                }
            }
        }

        private void CallBack(string key, object value, CacheItemRemovedReason reason)
        {
            UpdateCache();
        }
    }
}
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

            if (cache["customer_count"] == null)
            {
                UpdateCacheSimple();
            }

            ViewBag.employees = cache["employee_count"];
            ViewBag.customers = cache["customer_count"];
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
            return GetPersonCount("EM");
        }

        private int GetCurrentCustomerCount()
        {
            return GetPersonCount("SC");
        }

        private int GetPersonCount(string personType)
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["AdventureWorks"]
                .ConnectionString;
            var conn = new SqlConnection(connectionString);

            var cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT COUNT(*) FROM Person.Person WHERE PersonType = @personType";
            cmd.Parameters.AddWithValue("@personType", personType);
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
                    SqlCacheDependency dep = new SqlCacheDependency(command);

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

        private void UpdateCacheSimple()
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["AdventureWorks"]
                .ConnectionString;

            var custCount = GetCurrentCustomerCount();

            string key = "customer_count";
            string value = "Count: " + custCount + ", as of " + DateTime.Now.ToLongTimeString();
            SqlCacheDependency dep = new SqlCacheDependency("AdventureWorks", "Person");
            DateTime exp = DateTime.Now.AddMinutes(5);

            HttpContext.Cache
                .Add(key, value, dep, exp,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default, CallBackSimple);
        }


        private void CallBack(string key, object value, CacheItemRemovedReason reason)
        {
            UpdateCache();
        }

        private void CallBackSimple(string key, object value, CacheItemRemovedReason reason)
        {

            UpdateCacheSimple();
        }
    }
}
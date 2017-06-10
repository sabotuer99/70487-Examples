using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CacheDependencyDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PrepareDatabase();
        }

        private void PrepareDatabase()
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings["AdventureWorks"]
                .ConnectionString;

            try
            {
                SqlCacheDependencyAdmin.EnableNotifications(connectionString);
                SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, "Person.Person");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


    }
}

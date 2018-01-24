using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Batch;
using System.Web.Http.Routing;
using OwinBasicAuth.BasicAuth;

namespace OwinBasicAuth
{
    class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            //configure basic authentication
            //a valid username/password combination is one in which they match
            //hey, it's a demo, give me a break...
            appBuilder.UseBasicAuthentication("DemoRealm", (u,p) => u.Equals(p));


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}

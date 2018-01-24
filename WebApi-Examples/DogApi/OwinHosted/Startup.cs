using DogApi.Models;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Batch;
using System.Web.Http.Routing;

namespace OwinHosted
{
    class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Web API configuration and services
            config.Formatters.Clear();
            var json = new JsonMediaTypeFormatter();
            json.SerializerSettings.PreserveReferencesHandling =
                Newtonsoft.Json.PreserveReferencesHandling.All;
            json.SupportedMediaTypes.Clear();
            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            config.Formatters.Add(json);
            //config.Formatters.Add(new XmlMediaTypeFormatter());
            config.Formatters.Add(new DogMediaFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ConstrainedApi",
                routeTemplate: "api/Constraints/{alpha}/{id}",
                defaults: new
                {
                    controller = "Constraints",
                    id = RouteParameter.Optional,
                    alpha = "alpha"
                },
                constraints: new
                {
                    alpha = @"[A-Za-z]+",
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                }
            );

            appBuilder.UseWebApi(config); 
        }

        private class CustomHttpBatchHandler : DefaultHttpBatchHandler
        {
            public CustomHttpBatchHandler(HttpServer httpServer) : base(httpServer)
            {
                this.ExecutionOrder = BatchExecutionOrder.NonSequential;
            }
        }


    }
}

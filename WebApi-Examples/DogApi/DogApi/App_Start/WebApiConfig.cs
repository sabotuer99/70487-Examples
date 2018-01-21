using DogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DogApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Clear();
            var json = new JsonMediaTypeFormatter();
            json.SerializerSettings.PreserveReferencesHandling =
                Newtonsoft.Json.PreserveReferencesHandling.All;
            json.SupportedMediaTypes.Clear();
            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            config.Formatters.Add(json);
            config.Formatters.Add(new XmlMediaTypeFormatter());
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
                defaults: new {
                    controller = "Constraints",
                    id = RouteParameter.Optional,
                    alpha = "alpha" },
                constraints: new {
                    alpha = @"[A-Za-z]+",
                    httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );
        }
    }
}

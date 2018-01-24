using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinBasicAuth.BasicAuth
{
    public static class BasicAuthenticationMiddlewareExtensions
    {
        public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, string realm, Func<string,string,bool> validator)
        {
            var options = new BasicAuthenticationOptions(realm, validator);
            return app.UseBasicAuthentication(options);
        }

        public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, BasicAuthenticationOptions options)
        {
            return app.Use<BasicAuthenticationMiddleware>(options);
        }
    }
}

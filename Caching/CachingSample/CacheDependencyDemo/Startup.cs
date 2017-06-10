using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CacheDependencyDemo.Startup))]
namespace CacheDependencyDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

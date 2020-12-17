using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesBackEnd.Startup))]
namespace SalesBackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

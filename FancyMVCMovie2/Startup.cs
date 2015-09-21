using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FancyMVCMovie2.Startup))]
namespace FancyMVCMovie2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

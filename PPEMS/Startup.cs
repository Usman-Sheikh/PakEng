using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PPEMS.Startup))]
namespace PPEMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

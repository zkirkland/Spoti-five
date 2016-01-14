using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Spoti_five.Startup))]
namespace Spoti_five
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

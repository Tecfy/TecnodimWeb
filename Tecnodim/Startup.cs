using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Tecnodim.Startup))]

namespace Tecnodim
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
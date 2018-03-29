using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PayRoll.Startup))]
namespace PayRoll
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StoreTicket.Startup))]
namespace StoreTicket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

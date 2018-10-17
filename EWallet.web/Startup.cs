using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EWallet.web.Startup))]
namespace EWallet.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

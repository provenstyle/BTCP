using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BTCP.Web.UI.Startup))]
namespace BTCP.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

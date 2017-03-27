using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BibleTraining.Web.UI.Startup))]
namespace BibleTraining.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

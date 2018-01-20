using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCKanban.Startup))]
namespace MVCKanban
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

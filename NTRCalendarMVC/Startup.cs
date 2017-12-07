using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NTRCalendarMVC.Startup))]
namespace NTRCalendarMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hotel_Management.Startup))]
namespace Hotel_Management
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

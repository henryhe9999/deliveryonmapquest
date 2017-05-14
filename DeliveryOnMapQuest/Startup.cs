using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeliveryOnMapQuest.Startup))]
namespace DeliveryOnMapQuest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

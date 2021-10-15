using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASM.Startup))]
namespace ASM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

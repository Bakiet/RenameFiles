using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RenameFiles.Startup))]
namespace RenameFiles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

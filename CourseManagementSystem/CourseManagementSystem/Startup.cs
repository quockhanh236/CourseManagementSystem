using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CourseManagementSystem.Startup))]
namespace CourseManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

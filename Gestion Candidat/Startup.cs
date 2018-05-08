using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Gestion_Candidat.Startup))]
namespace Gestion_Candidat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

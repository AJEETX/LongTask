using Microsoft.Owin;
using Owin;
namespace Engine
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
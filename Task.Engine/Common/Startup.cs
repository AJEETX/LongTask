using Microsoft.Owin;
using Owin;
namespace LongTask.Engine
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
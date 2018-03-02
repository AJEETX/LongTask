using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
namespace Engine
{
    [HubName("progressHub")]
    public class ProgressHub : Hub
    {
    }
    public static class Functions
    {
        public static void StartProgress(string message, int count)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            var percentage = count;
            hubContext.Clients.All.AddProgress(message, percentage + "%");
        }
    }
}
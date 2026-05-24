using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VisualizationDSA.WebApi.Hubs
{
    public class LeaderboardHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "leaderboard");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "leaderboard");
            await base.OnDisconnectedAsync(exception);
        }
    }
}

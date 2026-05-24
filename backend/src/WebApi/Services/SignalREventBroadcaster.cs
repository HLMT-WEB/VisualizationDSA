using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;
using VisualizationDSA.WebApi.Hubs;

namespace VisualizationDSA.WebApi.Services
{
    public class SignalREventBroadcaster : IEventBroadcaster
    {
        private readonly IHubContext<LeaderboardHub> _leaderboardHub;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public SignalREventBroadcaster(
            IHubContext<LeaderboardHub> leaderboardHub,
            IHubContext<NotificationHub> notificationHub)
        {
            _leaderboardHub = leaderboardHub;
            _notificationHub = notificationHub;
        }

        public async Task BroadcastLeaderboardUpdate(LeaderboardUpdate update)
        {
            await _leaderboardHub.Clients.Group("leaderboard")
                .SendAsync("LeaderboardUpdated", update);
        }

        public async Task BroadcastBadgeNotification(Guid userId, BadgeNotification notification)
        {
            await _notificationHub.Clients.Group($"user:{userId}")
                .SendAsync("BadgeAwarded", notification);
        }

        public async Task BroadcastLevelUp(Guid userId, LevelUpNotification notification)
        {
            await _notificationHub.Clients.Group($"user:{userId}")
                .SendAsync("LevelUp", notification);
        }
    }
}

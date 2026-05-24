using System;
using System.Threading.Tasks;
using VisualizationDSA.Application.DTOs;

namespace VisualizationDSA.Application.Services
{
    public interface IEventBroadcaster
    {
        Task BroadcastLeaderboardUpdate(LeaderboardUpdate update);
        Task BroadcastBadgeNotification(Guid userId, BadgeNotification notification);
        Task BroadcastLevelUp(Guid userId, LevelUpNotification notification);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;
using VisualizationDSA.Domain.Entities;
using VisualizationDSA.Domain.Exceptions;
using VisualizationDSA.Domain.Interfaces;

namespace VisualizationDSA.Infrastructure.Services
{
    public class GamificationService : IGamificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBroadcaster _eventBroadcaster;

        public GamificationService(IUnitOfWork unitOfWork, IEventBroadcaster eventBroadcaster)
        {
            _unitOfWork = unitOfWork;
            _eventBroadcaster = eventBroadcaster;
        }

        public async Task AwardXPAsync(Guid userId, int amount, string reason)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            var oldLevel = user.CurrentLevel;
            user.AwardXP(amount);
            await _unitOfWork.CommitAsync();

            await _eventBroadcaster.BroadcastLeaderboardUpdate(new LeaderboardUpdate
            {
                Username = user.Username,
                TotalXP = user.TotalXP,
                CurrentLevel = user.CurrentLevel,
                Rank = 0,
                XPGained = amount
            });

            if (user.CurrentLevel > oldLevel)
            {
                await _eventBroadcaster.BroadcastLevelUp(userId, new LevelUpNotification
                {
                    UserId = userId,
                    Username = user.Username,
                    OldLevel = oldLevel,
                    NewLevel = user.CurrentLevel,
                    TotalXP = user.TotalXP
                });
            }
        }

        public async Task CompleteModuleAsync(Guid userId, string moduleId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            user.CompleteModule(moduleId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Badge>> CheckAndAwardBadgesAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            var newBadges = new List<Badge>();
            var allBadges = await _unitOfWork.Badges.GetAllAsync();

            foreach (var badge in allBadges)
            {
                if (user.UserBadges.Any(ub => ub.BadgeId == badge.Id))
                    continue;

                if (ShouldAwardBadge(user, badge))
                {
                    var userBadge = new UserBadge(userId, badge.Id);
                    await _unitOfWork.Users.AddAsync(user);
                    user.UserBadges.Add(userBadge);
                    newBadges.Add(badge);
                }
            }

            if (newBadges.Any())
            {
                await _unitOfWork.CommitAsync();

                foreach (var badge in newBadges)
                {
                    await _eventBroadcaster.BroadcastBadgeNotification(userId, new BadgeNotification
                    {
                        UserId = userId,
                        Username = user.Username,
                        BadgeName = badge.Name,
                        BadgeDescription = badge.Description,
                        AwardedAt = DateTime.UtcNow
                    });
                }
            }

            return newBadges;
        }

        public async Task<UserProgressStats> GetUserProgressAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            var nextLevelXp = CalculateXpForLevel(user.CurrentLevel + 1);
            var currentLevelXp = CalculateXpForLevel(user.CurrentLevel);
            var xpInCurrentLevel = user.TotalXP - currentLevelXp;
            var xpNeeded = nextLevelXp - currentLevelXp;
            var progressPercent = Math.Min(100, (int)((double)xpInCurrentLevel / xpNeeded * 100));

            return new UserProgressStats
            {
                TotalXP = user.TotalXP,
                CurrentLevel = user.CurrentLevel,
                XpToNextLevel = nextLevelXp - user.TotalXP,
                LevelProgressPercent = progressPercent,
                BadgesEarned = user.UserBadges.Count,
                ModulesCompleted = user.LearningProgresses.Count,
                CurrentStreak = user.StreakDays
            };
        }

        private static bool ShouldAwardBadge(User user, Badge badge)
        {
            return badge.Name switch
            {
                "First Steps" => user.QuizAttempts.Count >= 1,
                "Sorting Wizard" => user.LearningProgresses.Any(lp => lp.ModuleId.Contains("sort")),
                "OOP Guru" => user.LearningProgresses.Any(lp => lp.ModuleId.Contains("oop")),
                "SOLID Master" => user.LearningProgresses.Any(lp => lp.ModuleId.Contains("solid")),
                "Pattern Hunter" => user.LearningProgresses.Any(lp => lp.ModuleId.Contains("pattern")),
                "Streak Keeper" => user.StreakDays >= 7,
                "System Architect" => user.LearningProgresses.Any(lp => lp.ModuleId.Contains("system")),
                "DSA Champion" => user.CurrentLevel >= 5,
                _ => false
            };
        }

        private static int CalculateXpForLevel(int level)
        {
            return (level - 1) * (level - 1) * 100;
        }
    }
}

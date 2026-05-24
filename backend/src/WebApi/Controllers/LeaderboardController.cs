using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualizationDSA.Application.Constants;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;
using VisualizationDSA.Domain.Interfaces;

namespace VisualizationDSA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public LeaderboardController(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<PagedResult<LeaderboardEntry>>> GetLeaderboard(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var cacheKey = $"{CacheKeys.LeaderboardPrefix}{page}:{pageSize}";
            var cached = _cacheService.Get<PagedResult<LeaderboardEntry>>(cacheKey);
            if (cached != null)
                return Ok(cached);

            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var sortedUsers = allUsers.OrderByDescending(u => u.TotalXP).ToList();
            var totalCount = sortedUsers.Count;

            var pagedUsers = sortedUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select((u, index) => new LeaderboardEntry
                {
                    Rank = (page - 1) * pageSize + index + 1,
                    Username = u.Username,
                    TotalXP = u.TotalXP,
                    CurrentLevel = u.CurrentLevel,
                    BadgeCount = u.UserBadges.Count
                })
                .ToList();

            var result = new PagedResult<LeaderboardEntry>(pagedUsers, page, pageSize, totalCount);
            _cacheService.Set(cacheKey, result, CacheDurations.Leaderboard);

            return Ok(result);
        }
    }

    public class LeaderboardEntry
    {
        public int Rank { get; set; }
        public string Username { get; set; } = string.Empty;
        public int TotalXP { get; set; }
        public int CurrentLevel { get; set; }
        public int BadgeCount { get; set; }
    }
}

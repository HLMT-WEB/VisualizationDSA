using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualizationDSA.Application.Constants;
using VisualizationDSA.Application.Services;
using VisualizationDSA.Domain.Entities;
using VisualizationDSA.Domain.Interfaces;

namespace VisualizationDSA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BadgesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGamificationService _gamificationService;
        private readonly ICacheService _cacheService;

        public BadgesController(IUnitOfWork unitOfWork, IGamificationService gamificationService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _gamificationService = gamificationService;
            _cacheService = cacheService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<Badge>>> GetAll()
        {
            var cached = _cacheService.Get<List<Badge>>(CacheKeys.BadgeList);
            if (cached != null)
                return Ok(cached);

            var badges = await _unitOfWork.Badges.GetAllAsync();
            var badgeList = new List<Badge>(badges);
            _cacheService.Set(CacheKeys.BadgeList, badgeList, CacheDurations.BadgeList);

            return Ok(badgeList);
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<Badge>>> GetMyBadges()
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            
            if (user == null) return NotFound();

            var badges = new List<Badge>();
            foreach (var userBadge in user.UserBadges)
            {
                var badge = await _unitOfWork.Badges.GetByIdAsync(userBadge.BadgeId);
                if (badge != null)
                {
                    badges.Add(badge);
                }
            }

            return Ok(badges);
        }

        [HttpPost("check")]
        public async Task<ActionResult<IEnumerable<Badge>>> CheckNewBadges()
        {
            var userId = GetCurrentUserId();
            var newBadges = await _gamificationService.CheckAndAwardBadgesAsync(userId);
            _cacheService.Remove(CacheKeys.BadgeList);
            return Ok(newBadges);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }
    }
}

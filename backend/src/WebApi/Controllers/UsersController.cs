using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Domain.Exceptions;
using VisualizationDSA.Domain.Interfaces;

namespace VisualizationDSA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("progress")]
        public async Task<ActionResult> GetUserProgress()
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            return Ok(new
            {
                TotalXP = user.TotalXP,
                CurrentLevel = user.CurrentLevel,
                StreakDays = user.StreakDays,
                Badges = user.UserBadges,
                CompletedModules = user.LearningProgresses
            });
        }

        [HttpPost("xp")]
        public async Task<ActionResult> AwardXP([FromBody] XPAwardRequest request)
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            user.AwardXP(request.Amount);
            await _unitOfWork.CommitAsync();

            return Ok(new { Message = $"Đã trao {request.Amount} XP", TotalXP = user.TotalXP });
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VisualizationDSA.Domain.Exceptions;
using VisualizationDSA.Domain.Interfaces;

namespace VisualizationDSA.WebApi.Controllers
{
    [ApiController]
    [Route("api/learning-progress")]
    [Authorize]
    public class LearningProgressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LearningProgressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LearningProgressDto>>> GetMyProgress()
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            var progresses = user.LearningProgresses.Select(lp => new LearningProgressDto
            {
                ModuleId = lp.ModuleId,
                CompletedAt = lp.CompletedAt,
                TimeSpentMinutes = lp.TimeSpentMinutes,
            }).ToList();

            return Ok(progresses);
        }

        [HttpPost("complete")]
        public async Task<ActionResult> CompleteModule([FromBody] CompleteModuleRequest request)
        {
            var userId = GetCurrentUserId();
            var user = await _unitOfWork.Users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            var alreadyCompleted = user.LearningProgresses
                .Any(lp => lp.ModuleId == request.ModuleId);

            if (alreadyCompleted)
            {
                return Ok(new { Message = "Module đã hoàn thành trước đó", ModuleId = request.ModuleId });
            }

            user.CompleteModule(request.ModuleId);
            await _unitOfWork.CommitAsync();

            return Ok(new { Message = $"Hoàn thành module {request.ModuleId}", ModuleId = request.ModuleId });
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }
    }

    public class LearningProgressDto
    {
        public string ModuleId { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
        public int TimeSpentMinutes { get; set; }
    }

    public class CompleteModuleRequest
    {
        public string ModuleId { get; set; } = string.Empty;
    }
}

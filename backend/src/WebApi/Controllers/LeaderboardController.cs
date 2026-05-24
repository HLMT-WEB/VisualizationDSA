using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualizationDSA.Domain.Interfaces;

namespace VisualizationDSA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaderboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardEntryDto>>> GetTopPlayers([FromQuery] int top = 10)
        {
            var clampedTop = top > 50 ? 50 : (top < 1 ? 10 : top);

            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var sorted = allUsers
                .OrderByDescending(u => u.TotalXP)
                .Take(clampedTop)
                .Select((u, index) => new LeaderboardEntryDto
                {
                    Rank = index + 1,
                    Username = u.Username,
                    TotalXP = u.TotalXP,
                    CurrentLevel = u.CurrentLevel,
                    BadgeCount = u.UserBadges?.Count ?? 0,
                })
                .ToList();

            return Ok(sorted);
        }
    }

    public class LeaderboardEntryDto
    {
        public int Rank { get; set; }
        public string Username { get; set; } = string.Empty;
        public int TotalXP { get; set; }
        public int CurrentLevel { get; set; }
        public int BadgeCount { get; set; }
    }
}

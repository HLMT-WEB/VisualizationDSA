using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VisualizationDSA.Application.Constants;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;

namespace VisualizationDSA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly ICacheService _cacheService;

        public QuizzesController(IQuizService quizService, ICacheService cacheService)
        {
            _quizService = quizService;
            _cacheService = cacheService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAll()
        {
            var cached = _cacheService.Get<List<QuizDto>>(CacheKeys.QuizList);
            if (cached != null)
                return Ok(cached);

            var quizzes = await _quizService.GetAllQuizzesAsync();
            var quizList = new List<QuizDto>(quizzes);
            _cacheService.Set(CacheKeys.QuizList, quizList, CacheDurations.QuizList);

            return Ok(quizList);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<QuizDto>> GetById(Guid id)
        {
            var cacheKey = $"{CacheKeys.QuizByIdPrefix}{id}";
            var cached = _cacheService.Get<QuizDto>(cacheKey);
            if (cached != null)
                return Ok(cached);

            var quiz = await _quizService.GetQuizByIdAsync(id);
            _cacheService.Set(cacheKey, quiz, CacheDurations.QuizList);

            return Ok(quiz);
        }

        [HttpGet("topic/{topic}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetByTopic(string topic)
        {
            var cacheKey = $"{CacheKeys.QuizByTopicPrefix}{topic.ToLowerInvariant()}";
            var cached = _cacheService.Get<List<QuizDto>>(cacheKey);
            if (cached != null)
                return Ok(cached);

            var quizzes = await _quizService.GetQuizzesByTopicAsync(topic);
            var quizList = new List<QuizDto>(quizzes);
            _cacheService.Set(cacheKey, quizList, CacheDurations.QuizList);

            return Ok(quizList);
        }

        [HttpPost("attempt")]
        public async Task<ActionResult<QuizAttemptResult>> SubmitAttempt([FromBody] QuizAttemptRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _quizService.SubmitQuizAttemptAsync(userId, request);
            _cacheService.RemoveByPrefix(CacheKeys.LeaderboardPrefix);
            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<ActionResult<PagedResult<QuizAttempt>>> GetHistory(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var userId = GetCurrentUserId();
            var history = await _quizService.GetUserQuizHistoryAsync(userId);
            var historyList = history.ToList();

            var pagedItems = historyList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<QuizAttempt>(pagedItems, page, pageSize, historyList.Count);
            return Ok(result);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }
    }
}

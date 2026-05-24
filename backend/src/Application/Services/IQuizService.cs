using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualizationDSA.Application.DTOs;

namespace VisualizationDSA.Application.Services
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
        Task<QuizDto> GetQuizByIdAsync(Guid id);
        Task<IEnumerable<QuizDto>> GetQuizzesByTopicAsync(string topic);
        Task<QuizAttemptResult> SubmitQuizAttemptAsync(Guid userId, QuizAttemptRequest request);
        Task<IEnumerable<QuizAttempt>> GetUserQuizHistoryAsync(Guid userId);
        Task<QuizWithAnswersDto> GetQuizWithAnswersAsync(Guid id);
    }

    public class QuizWithAnswersDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int XPReward { get; set; }
        public List<QuizQuestionWithAnswerDto> Questions { get; set; } = new();
    }

    public class QuizQuestionWithAnswerDto
    {
        public Guid Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string[] Options { get; set; } = Array.Empty<string>();
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }

    public class QuizAttempt
    {
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public bool Passed { get; set; }
        public DateTime AttemptedAt { get; set; }
    }
}

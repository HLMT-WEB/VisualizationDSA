using System;
using System.Collections.Generic;

namespace VisualizationDSA.Application.DTOs
{
    public class LeaderboardUpdate
    {
        public string Username { get; set; } = string.Empty;
        public int TotalXP { get; set; }
        public int CurrentLevel { get; set; }
        public int Rank { get; set; }
        public int XPGained { get; set; }
    }

    public class BadgeNotification
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string BadgeName { get; set; } = string.Empty;
        public string BadgeDescription { get; set; } = string.Empty;
        public DateTime AwardedAt { get; set; }
    }

    public class LevelUpNotification
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int OldLevel { get; set; }
        public int NewLevel { get; set; }
        public int TotalXP { get; set; }
    }

    public class QuizRoomDto
    {
        public string RoomCode { get; set; } = string.Empty;
        public string QuizTitle { get; set; } = string.Empty;
        public Guid QuizId { get; set; }
        public string HostUsername { get; set; } = string.Empty;
        public List<QuizRoomParticipant> Participants { get; set; } = new();
        public QuizRoomStatus Status { get; set; }
        public int CurrentQuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
    }

    public class QuizRoomParticipant
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Score { get; set; }
        public bool HasAnswered { get; set; }
        public bool IsHost { get; set; }
    }

    public class QuizQuestionBroadcast
    {
        public int QuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
        public string Question { get; set; } = string.Empty;
        public string[] Options { get; set; } = Array.Empty<string>();
        public int TimeLimitSeconds { get; set; }
    }

    public class QuizAnswerResult
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
        public int TotalScore { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }

    public class QuizRoomResults
    {
        public string RoomCode { get; set; } = string.Empty;
        public string QuizTitle { get; set; } = string.Empty;
        public List<QuizRoomParticipant> FinalRankings { get; set; } = new();
        public int XPAwarded { get; set; }
    }

    public enum QuizRoomStatus
    {
        Waiting,
        InProgress,
        ShowingResults,
        Completed
    }
}

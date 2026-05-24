using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;

namespace VisualizationDSA.Infrastructure.Services
{
    public class QuizRoomService : IQuizRoomService
    {
        private readonly ConcurrentDictionary<string, QuizRoomState> _rooms = new();
        private static readonly Random _random = new();

        public QuizRoomDto CreateRoom(Guid quizId, Guid hostUserId, string hostUsername, string quizTitle, int totalQuestions)
        {
            var roomCode = GenerateRoomCode();
            var room = new QuizRoomState
            {
                RoomCode = roomCode,
                QuizId = quizId,
                QuizTitle = quizTitle,
                HostUserId = hostUserId,
                Status = QuizRoomStatus.Waiting,
                CurrentQuestionIndex = 0,
                TotalQuestions = totalQuestions,
                Participants = new ConcurrentDictionary<Guid, ParticipantState>()
            };

            room.Participants.TryAdd(hostUserId, new ParticipantState
            {
                UserId = hostUserId,
                Username = hostUsername,
                Score = 0,
                HasAnswered = false,
                IsHost = true
            });

            _rooms.TryAdd(roomCode, room);

            return MapToDto(room);
        }

        public QuizRoomDto? GetRoom(string roomCode)
        {
            return _rooms.TryGetValue(roomCode, out var room) ? MapToDto(room) : null;
        }

        public bool JoinRoom(string roomCode, Guid userId, string username)
        {
            if (!_rooms.TryGetValue(roomCode, out var room))
                return false;

            if (room.Status != QuizRoomStatus.Waiting)
                return false;

            if (room.Participants.Count >= 10)
                return false;

            return room.Participants.TryAdd(userId, new ParticipantState
            {
                UserId = userId,
                Username = username,
                Score = 0,
                HasAnswered = false,
                IsHost = false
            });
        }

        public bool LeaveRoom(string roomCode, Guid userId)
        {
            if (!_rooms.TryGetValue(roomCode, out var room))
                return false;

            room.Participants.TryRemove(userId, out _);

            if (room.Participants.IsEmpty)
            {
                _rooms.TryRemove(roomCode, out _);
            }

            return true;
        }

        public bool StartQuiz(string roomCode, Guid hostUserId)
        {
            if (!_rooms.TryGetValue(roomCode, out var room))
                return false;

            if (room.HostUserId != hostUserId)
                return false;

            if (room.Status != QuizRoomStatus.Waiting)
                return false;

            if (room.Participants.Count < 1)
                return false;

            room.Status = QuizRoomStatus.InProgress;
            room.CurrentQuestionIndex = 0;

            foreach (var participant in room.Participants.Values)
            {
                participant.HasAnswered = false;
            }

            return true;
        }

        public QuizAnswerResult? SubmitAnswer(string roomCode, Guid userId, string username, int questionIndex, int answerIndex, int correctIndex, string explanation, int basePoints)
        {
            if (!_rooms.TryGetValue(roomCode, out var room))
                return null;

            if (room.Status != QuizRoomStatus.InProgress)
                return null;

            if (questionIndex != room.CurrentQuestionIndex)
                return null;

            if (!room.Participants.TryGetValue(userId, out var participant))
                return null;

            if (participant.HasAnswered)
                return null;

            participant.HasAnswered = true;
            var isCorrect = answerIndex == correctIndex;
            var pointsEarned = isCorrect ? basePoints : 0;
            participant.Score += pointsEarned;

            return new QuizAnswerResult
            {
                UserId = userId,
                Username = username,
                IsCorrect = isCorrect,
                PointsEarned = pointsEarned,
                TotalScore = participant.Score,
                CorrectIndex = correctIndex,
                Explanation = explanation
            };
        }

        public bool AdvanceQuestion(string roomCode)
        {
            if (!_rooms.TryGetValue(roomCode, out var room))
                return false;

            if (room.Status != QuizRoomStatus.InProgress)
                return false;

            room.CurrentQuestionIndex++;

            foreach (var participant in room.Participants.Values)
            {
                participant.HasAnswered = false;
            }

            if (room.CurrentQuestionIndex >= room.TotalQuestions)
            {
                room.Status = QuizRoomStatus.ShowingResults;
                return false;
            }

            return true;
        }

        public QuizRoomResults? CompleteQuiz(string roomCode, int xpAwarded)
        {
            if (!_rooms.TryGetValue(roomCode, out var room))
                return null;

            room.Status = QuizRoomStatus.Completed;

            var rankings = room.Participants.Values
                .OrderByDescending(p => p.Score)
                .Select(p => new QuizRoomParticipant
                {
                    UserId = p.UserId,
                    Username = p.Username,
                    Score = p.Score,
                    HasAnswered = true,
                    IsHost = p.IsHost
                })
                .ToList();

            return new QuizRoomResults
            {
                RoomCode = roomCode,
                QuizTitle = room.QuizTitle,
                FinalRankings = rankings,
                XPAwarded = xpAwarded
            };
        }

        public IReadOnlyList<QuizRoomDto> GetActiveRooms()
        {
            return _rooms.Values
                .Where(r => r.Status == QuizRoomStatus.Waiting)
                .Select(MapToDto)
                .ToList();
        }

        public bool RemoveRoom(string roomCode)
        {
            return _rooms.TryRemove(roomCode, out _);
        }

        private static QuizRoomDto MapToDto(QuizRoomState room)
        {
            return new QuizRoomDto
            {
                RoomCode = room.RoomCode,
                QuizId = room.QuizId,
                QuizTitle = room.QuizTitle,
                HostUsername = room.Participants.Values.FirstOrDefault(p => p.IsHost)?.Username ?? "",
                Participants = room.Participants.Values.Select(p => new QuizRoomParticipant
                {
                    UserId = p.UserId,
                    Username = p.Username,
                    Score = p.Score,
                    HasAnswered = p.HasAnswered,
                    IsHost = p.IsHost
                }).ToList(),
                Status = room.Status,
                CurrentQuestionIndex = room.CurrentQuestionIndex,
                TotalQuestions = room.TotalQuestions
            };
        }

        private static string GenerateRoomCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            return new string(Enumerable.Range(0, 6).Select(_ => chars[_random.Next(chars.Length)]).ToArray());
        }

        private class QuizRoomState
        {
            public string RoomCode { get; set; } = string.Empty;
            public Guid QuizId { get; set; }
            public string QuizTitle { get; set; } = string.Empty;
            public Guid HostUserId { get; set; }
            public QuizRoomStatus Status { get; set; }
            public int CurrentQuestionIndex { get; set; }
            public int TotalQuestions { get; set; }
            public ConcurrentDictionary<Guid, ParticipantState> Participants { get; set; } = new();
        }

        private class ParticipantState
        {
            public Guid UserId { get; set; }
            public string Username { get; set; } = string.Empty;
            public int Score { get; set; }
            public bool HasAnswered { get; set; }
            public bool IsHost { get; set; }
        }
    }
}

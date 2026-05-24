using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;

namespace VisualizationDSA.WebApi.Hubs
{
    [Authorize]
    public class QuizRoomHub : Hub
    {
        private readonly IQuizRoomService _roomService;
        private readonly IQuizService _quizService;

        public QuizRoomHub(IQuizRoomService roomService, IQuizService quizService)
        {
            _roomService = roomService;
            _quizService = quizService;
        }

        public async Task CreateRoom(Guid quizId)
        {
            var userId = GetUserId();
            var username = GetUsername();
            var quiz = await _quizService.GetQuizWithAnswersAsync(quizId);

            var room = _roomService.CreateRoom(quizId, userId, username, quiz.Title, quiz.Questions.Count);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"quiz:{room.RoomCode}");
            await Clients.Caller.SendAsync("RoomCreated", room);
        }

        public async Task JoinRoom(string roomCode)
        {
            var userId = GetUserId();
            var username = GetUsername();

            var joined = _roomService.JoinRoom(roomCode, userId, username);
            if (!joined)
            {
                await Clients.Caller.SendAsync("JoinFailed", "Không thể tham gia phòng. Phòng đã đầy hoặc đang thi đấu.");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"quiz:{roomCode}");
            var room = _roomService.GetRoom(roomCode);
            await Clients.Group($"quiz:{roomCode}").SendAsync("ParticipantJoined", room);
        }

        public async Task LeaveRoom(string roomCode)
        {
            var userId = GetUserId();
            _roomService.LeaveRoom(roomCode, userId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"quiz:{roomCode}");

            var room = _roomService.GetRoom(roomCode);
            if (room != null)
            {
                await Clients.Group($"quiz:{roomCode}").SendAsync("ParticipantLeft", room);
            }
        }

        public async Task StartQuiz(string roomCode)
        {
            var userId = GetUserId();
            var started = _roomService.StartQuiz(roomCode, userId);
            if (!started)
            {
                await Clients.Caller.SendAsync("StartFailed", "Chỉ host mới có thể bắt đầu quiz.");
                return;
            }

            var room = _roomService.GetRoom(roomCode);
            if (room == null) return;

            var quiz = await _quizService.GetQuizWithAnswersAsync(room.QuizId);
            var questions = quiz.Questions;

            if (questions.Count == 0) return;

            var firstQuestion = questions[0];
            var broadcast = new QuizQuestionBroadcast
            {
                QuestionIndex = 0,
                TotalQuestions = questions.Count,
                Question = firstQuestion.Question,
                Options = firstQuestion.Options,
                TimeLimitSeconds = 30
            };

            await Clients.Group($"quiz:{roomCode}").SendAsync("QuizStarted", room);
            await Clients.Group($"quiz:{roomCode}").SendAsync("NewQuestion", broadcast);
        }

        public async Task SubmitAnswer(string roomCode, int questionIndex, int answerIndex)
        {
            var userId = GetUserId();
            var username = GetUsername();

            var room = _roomService.GetRoom(roomCode);
            if (room == null) return;

            var quiz = await _quizService.GetQuizWithAnswersAsync(room.QuizId);
            var questions = quiz.Questions;

            if (questionIndex >= questions.Count) return;

            var question = questions[questionIndex];
            var basePoints = 100;

            var result = _roomService.SubmitAnswer(
                roomCode, userId, username,
                questionIndex, answerIndex,
                question.CorrectIndex, question.Explanation,
                basePoints);

            if (result == null) return;

            await Clients.Group($"quiz:{roomCode}").SendAsync("AnswerResult", result);

            var updatedRoom = _roomService.GetRoom(roomCode);
            if (updatedRoom != null)
            {
                await Clients.Group($"quiz:{roomCode}").SendAsync("ScoreUpdate", updatedRoom.Participants);
            }
        }

        public async Task NextQuestion(string roomCode)
        {
            var room = _roomService.GetRoom(roomCode);
            if (room == null) return;

            var advanced = _roomService.AdvanceQuestion(roomCode);

            if (!advanced)
            {
                var quiz = await _quizService.GetQuizWithAnswersAsync(room.QuizId);
                var results = _roomService.CompleteQuiz(roomCode, quiz.XPReward);
                if (results != null)
                {
                    await Clients.Group($"quiz:{roomCode}").SendAsync("QuizCompleted", results);
                }
                return;
            }

            var updatedRoom = _roomService.GetRoom(roomCode);
            if (updatedRoom == null) return;

            var quizData = await _quizService.GetQuizWithAnswersAsync(updatedRoom.QuizId);
            var questions = quizData.Questions;
            var nextQ = questions[updatedRoom.CurrentQuestionIndex];

            var broadcast = new QuizQuestionBroadcast
            {
                QuestionIndex = updatedRoom.CurrentQuestionIndex,
                TotalQuestions = questions.Count,
                Question = nextQ.Question,
                Options = nextQ.Options,
                TimeLimitSeconds = 30
            };

            await Clients.Group($"quiz:{roomCode}").SendAsync("NewQuestion", broadcast);
        }

        public async Task GetActiveRooms()
        {
            var rooms = _roomService.GetActiveRooms();
            await Clients.Caller.SendAsync("ActiveRooms", rooms);
        }

        private Guid GetUserId()
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }

        private string GetUsername()
        {
            return Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
        }
    }
}

using System;
using System.Collections.Generic;
using VisualizationDSA.Application.DTOs;

namespace VisualizationDSA.Application.Services
{
    public interface IQuizRoomService
    {
        QuizRoomDto CreateRoom(Guid quizId, Guid hostUserId, string hostUsername, string quizTitle, int totalQuestions);
        QuizRoomDto? GetRoom(string roomCode);
        bool JoinRoom(string roomCode, Guid userId, string username);
        bool LeaveRoom(string roomCode, Guid userId);
        bool StartQuiz(string roomCode, Guid hostUserId);
        QuizAnswerResult? SubmitAnswer(string roomCode, Guid userId, string username, int questionIndex, int answerIndex, int correctIndex, string explanation, int basePoints);
        bool AdvanceQuestion(string roomCode);
        QuizRoomResults? CompleteQuiz(string roomCode, int xpAwarded);
        IReadOnlyList<QuizRoomDto> GetActiveRooms();
        bool RemoveRoom(string roomCode);
    }
}

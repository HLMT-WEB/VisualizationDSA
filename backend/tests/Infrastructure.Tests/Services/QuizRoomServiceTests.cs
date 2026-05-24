using FluentAssertions;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Infrastructure.Services;

namespace Infrastructure.Tests.Services;

public class QuizRoomServiceTests
{
    private readonly QuizRoomService _service;
    private readonly Guid _hostId = Guid.NewGuid();
    private readonly Guid _quizId = Guid.NewGuid();

    public QuizRoomServiceTests()
    {
        _service = new QuizRoomService();
    }

    [Fact]
    public void CreateRoom_ShouldReturnRoomWithCode()
    {
        var room = _service.CreateRoom(_quizId, _hostId, "HostUser", "Test Quiz", 5);

        room.Should().NotBeNull();
        room.RoomCode.Should().HaveLength(6);
        room.QuizTitle.Should().Be("Test Quiz");
        room.HostUsername.Should().Be("HostUser");
        room.Status.Should().Be(QuizRoomStatus.Waiting);
        room.TotalQuestions.Should().Be(5);
        room.Participants.Should().HaveCount(1);
        room.Participants[0].IsHost.Should().BeTrue();
    }

    [Fact]
    public void GetRoom_ExistingRoom_ShouldReturnRoom()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);

        var room = _service.GetRoom(created.RoomCode);

        room.Should().NotBeNull();
        room!.RoomCode.Should().Be(created.RoomCode);
    }

    [Fact]
    public void GetRoom_NonExistentRoom_ShouldReturnNull()
    {
        var room = _service.GetRoom("XXXXXX");

        room.Should().BeNull();
    }

    [Fact]
    public void JoinRoom_ValidRoom_ShouldAddParticipant()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        var userId = Guid.NewGuid();

        var joined = _service.JoinRoom(created.RoomCode, userId, "Player2");

        joined.Should().BeTrue();
        var room = _service.GetRoom(created.RoomCode);
        room!.Participants.Should().HaveCount(2);
    }

    [Fact]
    public void JoinRoom_NonExistentRoom_ShouldReturnFalse()
    {
        var joined = _service.JoinRoom("XXXXXX", Guid.NewGuid(), "Player");

        joined.Should().BeFalse();
    }

    [Fact]
    public void JoinRoom_InProgressRoom_ShouldReturnFalse()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        var joined = _service.JoinRoom(created.RoomCode, Guid.NewGuid(), "Late");

        joined.Should().BeFalse();
    }

    [Fact]
    public void JoinRoom_DuplicateUser_ShouldReturnFalse()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);

        var joined = _service.JoinRoom(created.RoomCode, _hostId, "Host");

        joined.Should().BeFalse();
    }

    [Fact]
    public void LeaveRoom_ShouldRemoveParticipant()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        var userId = Guid.NewGuid();
        _service.JoinRoom(created.RoomCode, userId, "Player");

        var left = _service.LeaveRoom(created.RoomCode, userId);

        left.Should().BeTrue();
        var room = _service.GetRoom(created.RoomCode);
        room!.Participants.Should().HaveCount(1);
    }

    [Fact]
    public void LeaveRoom_LastParticipant_ShouldRemoveRoom()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.LeaveRoom(created.RoomCode, _hostId);

        var room = _service.GetRoom(created.RoomCode);
        room.Should().BeNull();
    }

    [Fact]
    public void StartQuiz_ByHost_ShouldSetInProgress()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);

        var started = _service.StartQuiz(created.RoomCode, _hostId);

        started.Should().BeTrue();
        var room = _service.GetRoom(created.RoomCode);
        room!.Status.Should().Be(QuizRoomStatus.InProgress);
        room.CurrentQuestionIndex.Should().Be(0);
    }

    [Fact]
    public void StartQuiz_ByNonHost_ShouldReturnFalse()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);

        var started = _service.StartQuiz(created.RoomCode, Guid.NewGuid());

        started.Should().BeFalse();
    }

    [Fact]
    public void StartQuiz_AlreadyInProgress_ShouldReturnFalse()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        var started = _service.StartQuiz(created.RoomCode, _hostId);

        started.Should().BeFalse();
    }

    [Fact]
    public void SubmitAnswer_CorrectAnswer_ShouldAwardPoints()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        var result = _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 2, 2, "Correct!", 100);

        result.Should().NotBeNull();
        result!.IsCorrect.Should().BeTrue();
        result.PointsEarned.Should().Be(100);
        result.TotalScore.Should().Be(100);
    }

    [Fact]
    public void SubmitAnswer_WrongAnswer_ShouldNotAwardPoints()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        var result = _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 1, 2, "Wrong!", 100);

        result.Should().NotBeNull();
        result!.IsCorrect.Should().BeFalse();
        result.PointsEarned.Should().Be(0);
        result.TotalScore.Should().Be(0);
    }

    [Fact]
    public void SubmitAnswer_AlreadyAnswered_ShouldReturnNull()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);
        _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 2, 2, "First", 100);

        var result = _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 1, 2, "Double", 100);

        result.Should().BeNull();
    }

    [Fact]
    public void SubmitAnswer_WrongQuestionIndex_ShouldReturnNull()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        var result = _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 5, 2, 2, "Future", 100);

        result.Should().BeNull();
    }

    [Fact]
    public void AdvanceQuestion_ShouldIncrementIndex()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        var advanced = _service.AdvanceQuestion(created.RoomCode);

        advanced.Should().BeTrue();
        var room = _service.GetRoom(created.RoomCode);
        room!.CurrentQuestionIndex.Should().Be(1);
    }

    [Fact]
    public void AdvanceQuestion_LastQuestion_ShouldReturnFalseAndShowResults()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 2);
        _service.StartQuiz(created.RoomCode, _hostId);
        _service.AdvanceQuestion(created.RoomCode); // Q0 → Q1

        var advanced = _service.AdvanceQuestion(created.RoomCode); // Q1 → past end

        advanced.Should().BeFalse();
        var room = _service.GetRoom(created.RoomCode);
        room!.Status.Should().Be(QuizRoomStatus.ShowingResults);
    }

    [Fact]
    public void CompleteQuiz_ShouldReturnRankedResults()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 1);
        var player2 = Guid.NewGuid();
        _service.JoinRoom(created.RoomCode, player2, "Player2");
        _service.StartQuiz(created.RoomCode, _hostId);
        _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 2, 2, "OK", 100);
        _service.SubmitAnswer(created.RoomCode, player2, "Player2", 0, 1, 2, "OK", 100);

        var results = _service.CompleteQuiz(created.RoomCode, 50);

        results.Should().NotBeNull();
        results!.FinalRankings.Should().HaveCount(2);
        results.FinalRankings[0].Username.Should().Be("Host");
        results.FinalRankings[0].Score.Should().Be(100);
        results.FinalRankings[1].Username.Should().Be("Player2");
        results.FinalRankings[1].Score.Should().Be(0);
        results.XPAwarded.Should().Be(50);
    }

    [Fact]
    public void GetActiveRooms_ShouldReturnWaitingRooms()
    {
        _service.CreateRoom(_quizId, _hostId, "Host1", "Quiz1", 3);
        var room2 = _service.CreateRoom(_quizId, Guid.NewGuid(), "Host2", "Quiz2", 5);
        _service.StartQuiz(room2.RoomCode, Guid.Parse(room2.Participants.First(p => p.IsHost).UserId.ToString()));

        var activeRooms = _service.GetActiveRooms();

        activeRooms.Should().HaveCount(1);
        activeRooms[0].QuizTitle.Should().Be("Quiz1");
    }

    [Fact]
    public void RemoveRoom_ShouldDeleteRoom()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);

        var removed = _service.RemoveRoom(created.RoomCode);

        removed.Should().BeTrue();
        _service.GetRoom(created.RoomCode).Should().BeNull();
    }

    [Fact]
    public void RemoveRoom_NonExistent_ShouldReturnFalse()
    {
        var removed = _service.RemoveRoom("XXXXXX");

        removed.Should().BeFalse();
    }

    [Fact]
    public void SubmitAnswer_NonExistentRoom_ShouldReturnNull()
    {
        var result = _service.SubmitAnswer("XXXXXX", _hostId, "Host", 0, 1, 1, "Test", 100);

        result.Should().BeNull();
    }

    [Fact]
    public void SubmitAnswer_WaitingRoom_ShouldReturnNull()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);

        var result = _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 1, 1, "Test", 100);

        result.Should().BeNull();
    }

    [Fact]
    public void SubmitAnswer_NonParticipant_ShouldReturnNull()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        var result = _service.SubmitAnswer(created.RoomCode, Guid.NewGuid(), "Stranger", 0, 1, 1, "Test", 100);

        result.Should().BeNull();
    }

    [Fact]
    public void RoomCode_ShouldBe6Characters()
    {
        var room = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);

        room.RoomCode.Should().MatchRegex("^[A-Z2-9]{6}$");
    }

    [Fact]
    public void MultipleRooms_ShouldHaveUniqueCode()
    {
        var room1 = _service.CreateRoom(_quizId, _hostId, "Host1", "Quiz1", 3);
        var room2 = _service.CreateRoom(_quizId, Guid.NewGuid(), "Host2", "Quiz2", 3);

        room1.RoomCode.Should().NotBe(room2.RoomCode);
    }

    [Fact]
    public void AdvanceQuestion_ResetsHasAnswered()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);
        _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 2, 2, "OK", 100);

        _service.AdvanceQuestion(created.RoomCode);

        var room = _service.GetRoom(created.RoomCode);
        room!.Participants.All(p => !p.HasAnswered).Should().BeTrue();
    }

    [Fact]
    public void ScoreAccumulation_MultipleQuestions()
    {
        var created = _service.CreateRoom(_quizId, _hostId, "Host", "Quiz", 3);
        _service.StartQuiz(created.RoomCode, _hostId);

        _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 0, 2, 2, "OK", 100);
        _service.AdvanceQuestion(created.RoomCode);
        _service.SubmitAnswer(created.RoomCode, _hostId, "Host", 1, 1, 1, "OK", 100);

        var room = _service.GetRoom(created.RoomCode);
        room!.Participants[0].Score.Should().Be(200);
    }
}

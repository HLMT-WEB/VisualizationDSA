using FluentAssertions;
using VisualizationDSA.Application.DTOs;

namespace Application.Tests.DTOs;

public class SignalRDtosTests
{
    [Fact]
    public void LeaderboardUpdate_ShouldHaveDefaultValues()
    {
        var update = new LeaderboardUpdate();

        update.Username.Should().BeEmpty();
        update.TotalXP.Should().Be(0);
        update.CurrentLevel.Should().Be(0);
        update.Rank.Should().Be(0);
        update.XPGained.Should().Be(0);
    }

    [Fact]
    public void LeaderboardUpdate_ShouldSetProperties()
    {
        var update = new LeaderboardUpdate
        {
            Username = "TestUser",
            TotalXP = 500,
            CurrentLevel = 3,
            Rank = 1,
            XPGained = 50
        };

        update.Username.Should().Be("TestUser");
        update.TotalXP.Should().Be(500);
        update.CurrentLevel.Should().Be(3);
        update.Rank.Should().Be(1);
        update.XPGained.Should().Be(50);
    }

    [Fact]
    public void BadgeNotification_ShouldSetProperties()
    {
        var userId = Guid.NewGuid();
        var notification = new BadgeNotification
        {
            UserId = userId,
            Username = "Player1",
            BadgeName = "First Quiz",
            BadgeDescription = "Complete your first quiz",
            AwardedAt = DateTime.UtcNow
        };

        notification.UserId.Should().Be(userId);
        notification.Username.Should().Be("Player1");
        notification.BadgeName.Should().Be("First Quiz");
        notification.BadgeDescription.Should().Be("Complete your first quiz");
    }

    [Fact]
    public void LevelUpNotification_ShouldSetProperties()
    {
        var userId = Guid.NewGuid();
        var notification = new LevelUpNotification
        {
            UserId = userId,
            Username = "Player1",
            OldLevel = 2,
            NewLevel = 3,
            TotalXP = 900
        };

        notification.OldLevel.Should().Be(2);
        notification.NewLevel.Should().Be(3);
        notification.TotalXP.Should().Be(900);
    }

    [Fact]
    public void QuizRoomDto_ShouldHaveDefaultValues()
    {
        var room = new QuizRoomDto();

        room.RoomCode.Should().BeEmpty();
        room.QuizTitle.Should().BeEmpty();
        room.HostUsername.Should().BeEmpty();
        room.Participants.Should().BeEmpty();
        room.Status.Should().Be(QuizRoomStatus.Waiting);
        room.CurrentQuestionIndex.Should().Be(0);
        room.TotalQuestions.Should().Be(0);
    }

    [Fact]
    public void QuizRoomParticipant_ShouldSetProperties()
    {
        var participant = new QuizRoomParticipant
        {
            UserId = Guid.NewGuid(),
            Username = "Player",
            Score = 300,
            HasAnswered = true,
            IsHost = false
        };

        participant.Username.Should().Be("Player");
        participant.Score.Should().Be(300);
        participant.HasAnswered.Should().BeTrue();
        participant.IsHost.Should().BeFalse();
    }

    [Fact]
    public void QuizQuestionBroadcast_ShouldSetProperties()
    {
        var broadcast = new QuizQuestionBroadcast
        {
            QuestionIndex = 2,
            TotalQuestions = 10,
            Question = "What is Big O of Binary Search?",
            Options = new[] { "O(1)", "O(log n)", "O(n)", "O(n²)" },
            TimeLimitSeconds = 30
        };

        broadcast.QuestionIndex.Should().Be(2);
        broadcast.TotalQuestions.Should().Be(10);
        broadcast.Question.Should().Contain("Binary Search");
        broadcast.Options.Should().HaveCount(4);
        broadcast.TimeLimitSeconds.Should().Be(30);
    }

    [Fact]
    public void QuizAnswerResult_ShouldSetProperties()
    {
        var result = new QuizAnswerResult
        {
            UserId = Guid.NewGuid(),
            Username = "Player",
            IsCorrect = true,
            PointsEarned = 100,
            TotalScore = 300,
            CorrectIndex = 1,
            Explanation = "O(log n) because binary search halves the search space."
        };

        result.IsCorrect.Should().BeTrue();
        result.PointsEarned.Should().Be(100);
        result.TotalScore.Should().Be(300);
        result.CorrectIndex.Should().Be(1);
        result.Explanation.Should().Contain("binary search");
    }

    [Fact]
    public void QuizRoomResults_ShouldSetProperties()
    {
        var results = new QuizRoomResults
        {
            RoomCode = "ABC123",
            QuizTitle = "DSA Challenge",
            FinalRankings = new List<QuizRoomParticipant>
            {
                new() { Username = "Winner", Score = 500, IsHost = true },
                new() { Username = "Second", Score = 300, IsHost = false }
            },
            XPAwarded = 50
        };

        results.RoomCode.Should().Be("ABC123");
        results.QuizTitle.Should().Be("DSA Challenge");
        results.FinalRankings.Should().HaveCount(2);
        results.FinalRankings[0].Score.Should().BeGreaterThan(results.FinalRankings[1].Score);
        results.XPAwarded.Should().Be(50);
    }

    [Fact]
    public void QuizRoomStatus_ShouldHaveExpectedValues()
    {
        QuizRoomStatus.Waiting.Should().Be(QuizRoomStatus.Waiting);
        QuizRoomStatus.InProgress.Should().Be(QuizRoomStatus.InProgress);
        QuizRoomStatus.ShowingResults.Should().Be(QuizRoomStatus.ShowingResults);
        QuizRoomStatus.Completed.Should().Be(QuizRoomStatus.Completed);
    }

    [Fact]
    public void QuizRoomStatus_ShouldHave4Values()
    {
        Enum.GetValues<QuizRoomStatus>().Should().HaveCount(4);
    }
}

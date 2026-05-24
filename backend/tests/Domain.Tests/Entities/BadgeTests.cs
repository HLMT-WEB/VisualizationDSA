using FluentAssertions;
using VisualizationDSA.Domain.Entities;

namespace Domain.Tests.Entities;

public class BadgeTests
{
    [Fact]
    public void Constructor_ShouldInitializeAllProperties()
    {
        var badge = new Badge("First Steps", "Complete your first quiz", "star", "#FFD700", "quiz_count >= 1");

        badge.Id.Should().NotBeEmpty();
        badge.Name.Should().Be("First Steps");
        badge.Description.Should().Be("Complete your first quiz");
        badge.Icon.Should().Be("star");
        badge.Color.Should().Be("#FFD700");
        badge.Criteria.Should().Be("quiz_count >= 1");
        badge.UserBadges.Should().BeEmpty();
    }
}

public class UserBadgeTests
{
    [Fact]
    public void Constructor_ShouldSetUserIdAndBadgeId()
    {
        var userId = Guid.NewGuid();
        var badgeId = Guid.NewGuid();

        var userBadge = new UserBadge(userId, badgeId);

        userBadge.Id.Should().NotBeEmpty();
        userBadge.UserId.Should().Be(userId);
        userBadge.BadgeId.Should().Be(badgeId);
        userBadge.EarnedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}

public class QuizTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var quiz = new Quiz("Sorting Quiz", "Test sorting knowledge", "sorting", 2, 50);

        quiz.Id.Should().NotBeEmpty();
        quiz.Title.Should().Be("Sorting Quiz");
        quiz.Topic.Should().Be("sorting");
        quiz.Difficulty.Should().Be(2);
        quiz.XPReward.Should().Be(50);
        quiz.Questions.Should().BeEmpty();
    }

    [Fact]
    public void AddQuestion_ShouldAppendToQuestions()
    {
        var quiz = new Quiz("Quiz", "Desc", "sorting", 1, 10);

        quiz.AddQuestion("What is O(n^2)?", new[] { "Bubble", "Merge" }, 0, "Bubble Sort is O(n^2)");

        quiz.Questions.Should().HaveCount(1);
        quiz.Questions.First().Question.Should().Be("What is O(n^2)?");
        quiz.Questions.First().CorrectIndex.Should().Be(0);
    }
}

public class QuizAttemptTests
{
    [Theory]
    [InlineData(7, 10, true)]
    [InlineData(8, 10, true)]
    [InlineData(6, 10, false)]
    [InlineData(3, 5, false)]
    [InlineData(4, 5, true)]
    public void Constructor_ShouldCalculatePassedBasedOn70Percent(int score, int maxScore, bool expectedPassed)
    {
        var attempt = new QuizAttempt(Guid.NewGuid(), Guid.NewGuid(), new[] { 0, 1, 2 }, score, maxScore);

        attempt.Passed.Should().Be(expectedPassed);
        attempt.Score.Should().Be(score);
        attempt.MaxScore.Should().Be(maxScore);
    }
}

public class LearningProgressTests
{
    [Fact]
    public void Constructor_ShouldSetModuleIdAndTimestamp()
    {
        var userId = Guid.NewGuid();

        var progress = new LearningProgress(userId, "bubble-sort", 15);

        progress.UserId.Should().Be(userId);
        progress.ModuleId.Should().Be("bubble-sort");
        progress.TimeSpentMinutes.Should().Be(15);
        progress.CompletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void UpdateTimeSpent_ShouldOverwriteMinutes()
    {
        var progress = new LearningProgress(Guid.NewGuid(), "oop", 10);

        progress.UpdateTimeSpent(25);

        progress.TimeSpentMinutes.Should().Be(25);
    }
}

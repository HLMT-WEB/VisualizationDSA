using FluentAssertions;
using VisualizationDSA.Application.Constants;

namespace Application.Tests.Constants;

public class CacheKeysTests
{
    [Fact]
    public void AlgorithmList_ShouldHaveCorrectValue()
    {
        CacheKeys.AlgorithmList.Should().Be("algorithms:list");
    }

    [Fact]
    public void AlgorithmMetadataPrefix_ShouldEndWithColon()
    {
        CacheKeys.AlgorithmMetadataPrefix.Should().EndWith(":");
    }

    [Fact]
    public void QuizList_ShouldHaveCorrectValue()
    {
        CacheKeys.QuizList.Should().Be("quizzes:list");
    }

    [Fact]
    public void BadgeList_ShouldHaveCorrectValue()
    {
        CacheKeys.BadgeList.Should().Be("badges:list");
    }

    [Fact]
    public void LeaderboardPrefix_ShouldEndWithColon()
    {
        CacheKeys.LeaderboardPrefix.Should().EndWith(":");
    }

    [Fact]
    public void CacheDurations_AlgorithmMetadata_ShouldBe24Hours()
    {
        CacheDurations.AlgorithmMetadata.Should().Be(TimeSpan.FromHours(24));
    }

    [Fact]
    public void CacheDurations_QuizList_ShouldBe30Minutes()
    {
        CacheDurations.QuizList.Should().Be(TimeSpan.FromMinutes(30));
    }

    [Fact]
    public void CacheDurations_BadgeList_ShouldBe1Hour()
    {
        CacheDurations.BadgeList.Should().Be(TimeSpan.FromHours(1));
    }

    [Fact]
    public void CacheDurations_Leaderboard_ShouldBe5Minutes()
    {
        CacheDurations.Leaderboard.Should().Be(TimeSpan.FromMinutes(5));
    }
}

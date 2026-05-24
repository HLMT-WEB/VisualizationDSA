using FluentAssertions;
using VisualizationDSA.Domain.Entities;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeAllProperties()
    {
        var user = new User("test@example.com", "testuser", "hashed_pw");

        user.Id.Should().NotBeEmpty();
        user.Email.Should().Be("test@example.com");
        user.Username.Should().Be("testuser");
        user.PasswordHash.Should().Be("hashed_pw");
        user.TotalXP.Should().Be(0);
        user.CurrentLevel.Should().Be(1);
        user.StreakDays.Should().Be(0);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpiry.Should().BeNull();
    }

    [Theory]
    [InlineData(50, 1)]
    [InlineData(100, 2)]
    [InlineData(400, 3)]
    [InlineData(900, 4)]
    public void AwardXP_ShouldUpdateTotalXP_AndCalculateLevel(int xp, int expectedLevel)
    {
        var user = new User("a@b.com", "user1", "hash");

        user.AwardXP(xp);

        user.TotalXP.Should().Be(xp);
        user.CurrentLevel.Should().Be(expectedLevel);
    }

    [Fact]
    public void AwardXP_MultipleCalls_ShouldAccumulate()
    {
        var user = new User("a@b.com", "user1", "hash");

        user.AwardXP(50);
        user.AwardXP(60);

        user.TotalXP.Should().Be(110);
    }

    [Fact]
    public void RecordLogin_ShouldSetLastLoginAt()
    {
        var user = new User("a@b.com", "user1", "hash");

        user.RecordLogin();

        user.LastLoginAt.Should().NotBeNull();
        user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void SetRefreshToken_ShouldStoreTokenAndExpiry()
    {
        var user = new User("a@b.com", "user1", "hash");
        var expiry = DateTime.UtcNow.AddDays(30);

        user.SetRefreshToken("refresh_token_value", expiry);

        user.RefreshToken.Should().Be("refresh_token_value");
        user.RefreshTokenExpiry.Should().Be(expiry);
    }

    [Fact]
    public void RevokeRefreshToken_ShouldClearTokenAndExpiry()
    {
        var user = new User("a@b.com", "user1", "hash");
        user.SetRefreshToken("token", DateTime.UtcNow.AddDays(30));

        user.RevokeRefreshToken();

        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpiry.Should().BeNull();
    }

    [Fact]
    public void CompleteModule_ShouldAddLearningProgress()
    {
        var user = new User("a@b.com", "user1", "hash");

        user.CompleteModule("bubble-sort");

        user.LearningProgresses.Should().HaveCount(1);
        user.LearningProgresses.First().ModuleId.Should().Be("bubble-sort");
    }

    [Fact]
    public void LevelUp_ShouldNeverDecrease()
    {
        var user = new User("a@b.com", "user1", "hash");
        user.AwardXP(400); // Level 3

        user.CurrentLevel.Should().Be(3);

        user.AwardXP(1); // 401 XP should still be level 3
        user.CurrentLevel.Should().BeGreaterThanOrEqualTo(3);
    }
}

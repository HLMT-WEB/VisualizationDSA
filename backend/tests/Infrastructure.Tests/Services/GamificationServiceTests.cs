using FluentAssertions;
using Moq;
using VisualizationDSA.Domain.Entities;
using VisualizationDSA.Domain.Exceptions;
using VisualizationDSA.Domain.Interfaces;
using VisualizationDSA.Infrastructure.Services;

namespace Infrastructure.Tests.Services;

public class GamificationServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IRepository<User>> _userRepo;
    private readonly Mock<IRepository<Badge>> _badgeRepo;
    private readonly GamificationService _service;

    public GamificationServiceTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _userRepo = new Mock<IRepository<User>>();
        _badgeRepo = new Mock<IRepository<Badge>>();
        _unitOfWork.Setup(u => u.Users).Returns(_userRepo.Object);
        _unitOfWork.Setup(u => u.Badges).Returns(_badgeRepo.Object);
        _unitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);
        _service = new GamificationService(_unitOfWork.Object);
    }

    [Fact]
    public async Task AwardXPAsync_ValidUser_ShouldIncreaseXP()
    {
        var user = new User("a@b.com", "user1", "hash");
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

        await _service.AwardXPAsync(user.Id, 50, "test");

        user.TotalXP.Should().Be(50);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task AwardXPAsync_NonexistentUser_ShouldThrowNotFoundException()
    {
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = () => _service.AwardXPAsync(Guid.NewGuid(), 50, "test");

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CompleteModuleAsync_ValidUser_ShouldAddProgress()
    {
        var user = new User("a@b.com", "user1", "hash");
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

        await _service.CompleteModuleAsync(user.Id, "bubble-sort");

        user.LearningProgresses.Should().HaveCount(1);
        user.LearningProgresses.First().ModuleId.Should().Be("bubble-sort");
    }

    [Fact]
    public async Task CompleteModuleAsync_NonexistentUser_ShouldThrowNotFoundException()
    {
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = () => _service.CompleteModuleAsync(Guid.NewGuid(), "test");

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetUserProgressAsync_ShouldReturnCorrectStats()
    {
        var user = new User("a@b.com", "user1", "hash");
        user.AwardXP(150);
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

        var stats = await _service.GetUserProgressAsync(user.Id);

        stats.TotalXP.Should().Be(150);
        stats.CurrentLevel.Should().BeGreaterThanOrEqualTo(1);
        stats.BadgesEarned.Should().Be(0);
        stats.ModulesCompleted.Should().Be(0);
    }

    [Fact]
    public async Task GetUserProgressAsync_NonexistentUser_ShouldThrowNotFoundException()
    {
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = () => _service.GetUserProgressAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CheckAndAwardBadgesAsync_NoBadgesEligible_ShouldReturnEmpty()
    {
        var user = new User("a@b.com", "user1", "hash");
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
        _badgeRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Badge>
        {
            new Badge("DSA Champion", "Reach level 5", "trophy", "#gold", "level >= 5")
        });

        var result = await _service.CheckAndAwardBadgesAsync(user.Id);

        result.Should().BeEmpty();
    }
}

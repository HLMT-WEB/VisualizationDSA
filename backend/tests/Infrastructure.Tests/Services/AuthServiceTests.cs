using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Domain.Entities;
using VisualizationDSA.Domain.Exceptions;
using VisualizationDSA.Domain.Interfaces;
using VisualizationDSA.Infrastructure.Services;

namespace Infrastructure.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IRepository<User>> _userRepo;
    private readonly IConfiguration _config;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _userRepo = new Mock<IRepository<User>>();
        _unitOfWork.Setup(u => u.Users).Returns(_userRepo.Object);
        _unitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var configData = new Dictionary<string, string?>
        {
            { "Jwt:Key", "SuperSecretTestKeyThatIsAtLeast32BytesLongForHmacSha256!!" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" }
        };
        _config = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();
        _authService = new AuthService(_unitOfWork.Object, _config);
    }

    [Fact]
    public async Task RegisterAsync_NewUser_ShouldReturnTokenAndUser()
    {
        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(Enumerable.Empty<User>());
        _userRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var request = new RegisterRequest
        {
            Email = "new@test.com",
            Username = "newuser",
            Password = "Password1"
        };

        var result = await _authService.RegisterAsync(request);

        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be("new@test.com");
        result.User.Username.Should().Be("newuser");
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_ShouldThrowConflictException()
    {
        var existingUser = new User("existing@test.com", "existing", "hash");
        var callCount = 0;
        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? new[] { existingUser } : Enumerable.Empty<User>();
            });

        var request = new RegisterRequest
        {
            Email = "existing@test.com",
            Username = "newuser",
            Password = "Password1"
        };

        var act = () => _authService.RegisterAsync(request);

        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ShouldReturnToken()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Password1", workFactor: 12);
        var user = new User("login@test.com", "loginuser", passwordHash);

        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new[] { user });

        var request = new LoginRequest { Email = "login@test.com", Password = "Password1" };

        var result = await _authService.LoginAsync(request);

        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be("login@test.com");
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ShouldThrowAuthException()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword1", workFactor: 4);
        var user = new User("login@test.com", "loginuser", passwordHash);

        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new[] { user });

        var request = new LoginRequest { Email = "login@test.com", Password = "WrongPassword1" };

        var act = () => _authService.LoginAsync(request);

        await act.Should().ThrowAsync<AuthenticationException>();
    }

    [Fact]
    public async Task LoginAsync_NonexistentUser_ShouldThrowAuthException()
    {
        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(Enumerable.Empty<User>());

        var request = new LoginRequest { Email = "nobody@test.com", Password = "Password1" };

        var act = () => _authService.LoginAsync(request);

        await act.Should().ThrowAsync<AuthenticationException>();
    }

    [Fact]
    public async Task GetCurrentUserAsync_ExistingUser_ShouldReturnUserDto()
    {
        var userId = Guid.NewGuid();
        var user = new User("me@test.com", "me", "hash");

        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        var result = await _authService.GetCurrentUserAsync(userId);

        result.Should().NotBeNull();
        result.Email.Should().Be("me@test.com");
    }

    [Fact]
    public async Task GetCurrentUserAsync_NonexistentUser_ShouldThrowNotFoundException()
    {
        _userRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        var act = () => _authService.GetCurrentUserAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task RefreshTokenAsync_ValidToken_ShouldReturnNewTokens()
    {
        var user = new User("refresh@test.com", "refreshuser", "hash");
        user.SetRefreshToken("valid_refresh_token", DateTime.UtcNow.AddDays(30));

        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new[] { user });

        var result = await _authService.RefreshTokenAsync("valid_refresh_token");

        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBe("valid_refresh_token");
    }

    [Fact]
    public async Task RefreshTokenAsync_ExpiredToken_ShouldThrowAuthException()
    {
        var user = new User("expired@test.com", "expireduser", "hash");
        user.SetRefreshToken("expired_token", DateTime.UtcNow.AddDays(-1));

        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new[] { user });

        var act = () => _authService.RefreshTokenAsync("expired_token");

        await act.Should().ThrowAsync<AuthenticationException>();
    }

    [Fact]
    public async Task RefreshTokenAsync_InvalidToken_ShouldThrowAuthException()
    {
        _userRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(Enumerable.Empty<User>());

        var act = () => _authService.RefreshTokenAsync("nonexistent_token");

        await act.Should().ThrowAsync<AuthenticationException>();
    }
}

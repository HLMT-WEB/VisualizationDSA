using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Services;
using VisualizationDSA.Domain.Entities;
using VisualizationDSA.Domain.Exceptions;
using VisualizationDSA.Domain.Interfaces;
using VisualizationDSA.Infrastructure.Services;
using DomainQuizAttempt = VisualizationDSA.Domain.Entities.QuizAttempt;

namespace Infrastructure.Tests.Services;

public class QuizServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IRepository<Quiz>> _quizRepo;
    private readonly Mock<IRepository<DomainQuizAttempt>> _attemptRepo;
    private readonly Mock<IGamificationService> _gamificationService;
    private readonly QuizService _service;

    public QuizServiceTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _quizRepo = new Mock<IRepository<Quiz>>();
        _attemptRepo = new Mock<IRepository<DomainQuizAttempt>>();
        _gamificationService = new Mock<IGamificationService>();

        _unitOfWork.Setup(u => u.Quizzes).Returns(_quizRepo.Object);
        _unitOfWork.Setup(u => u.QuizAttempts).Returns(_attemptRepo.Object);
        _unitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        _service = new QuizService(_unitOfWork.Object, _gamificationService.Object);
    }

    [Fact]
    public async Task GetAllQuizzesAsync_ShouldReturnMappedDtos()
    {
        var quiz = new Quiz("Test Quiz", "Description", "sorting", 2, 50);
        _quizRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { quiz });

        var result = await _service.GetAllQuizzesAsync();

        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Test Quiz");
        result.First().Topic.Should().Be("sorting");
    }

    [Fact]
    public async Task GetQuizByIdAsync_ExistingQuiz_ShouldReturnDto()
    {
        var quiz = new Quiz("Test", "Desc", "oop", 3, 30);
        _quizRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(quiz);

        var result = await _service.GetQuizByIdAsync(quiz.Id);

        result.Title.Should().Be("Test");
        result.Difficulty.Should().Be(3);
    }

    [Fact]
    public async Task GetQuizByIdAsync_NonExisting_ShouldThrowNotFoundException()
    {
        _quizRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Quiz?)null);

        var act = () => _service.GetQuizByIdAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetQuizzesByTopicAsync_ShouldFilterByTopic()
    {
        var quizzes = new[]
        {
            new Quiz("Sort Quiz", "Desc", "sorting", 1, 10),
            new Quiz("OOP Quiz", "Desc", "oop", 2, 20)
        };
        _quizRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Quiz, bool>>>()))
            .ReturnsAsync(quizzes.Where(q => q.Topic == "sorting"));

        var result = await _service.GetQuizzesByTopicAsync("sorting");

        result.Should().HaveCount(1);
        result.First().Topic.Should().Be("sorting");
    }

    [Fact]
    public async Task SubmitQuizAttemptAsync_AllCorrect_ShouldPassAndAwardXP()
    {
        var quiz = new Quiz("Test", "Desc", "sorting", 1, 50);
        quiz.AddQuestion("Q1", new[] { "A", "B" }, 0, "Explain");
        quiz.AddQuestion("Q2", new[] { "C", "D" }, 1, "Explain");

        _quizRepo.Setup(r => r.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        _attemptRepo.Setup(r => r.AddAsync(It.IsAny<DomainQuizAttempt>()))
            .ReturnsAsync((DomainQuizAttempt a) => a);

        var request = new QuizAttemptRequest { QuizId = quiz.Id, Answers = new[] { 0, 1 } };

        var result = await _service.SubmitQuizAttemptAsync(Guid.NewGuid(), request);

        result.Score.Should().Be(2);
        result.MaxScore.Should().Be(2);
        result.Passed.Should().BeTrue();
        result.XPEarned.Should().Be(50);

        _gamificationService.Verify(g => g.AwardXPAsync(It.IsAny<Guid>(), 50, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SubmitQuizAttemptAsync_AllWrong_ShouldFailAndNoXP()
    {
        var quiz = new Quiz("Test", "Desc", "sorting", 1, 50);
        quiz.AddQuestion("Q1", new[] { "A", "B" }, 0, "Explain");
        quiz.AddQuestion("Q2", new[] { "C", "D" }, 1, "Explain");
        quiz.AddQuestion("Q3", new[] { "E", "F" }, 0, "Explain");

        _quizRepo.Setup(r => r.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        _attemptRepo.Setup(r => r.AddAsync(It.IsAny<DomainQuizAttempt>()))
            .ReturnsAsync((DomainQuizAttempt a) => a);

        var request = new QuizAttemptRequest { QuizId = quiz.Id, Answers = new[] { 1, 0, 1 } };

        var result = await _service.SubmitQuizAttemptAsync(Guid.NewGuid(), request);

        result.Score.Should().Be(0);
        result.Passed.Should().BeFalse();
        result.XPEarned.Should().Be(0);

        _gamificationService.Verify(g => g.AwardXPAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SubmitQuizAttemptAsync_WrongAnswerCount_ShouldThrowValidationException()
    {
        var quiz = new Quiz("Test", "Desc", "sorting", 1, 50);
        quiz.AddQuestion("Q1", new[] { "A", "B" }, 0, "Explain");
        quiz.AddQuestion("Q2", new[] { "C", "D" }, 1, "Explain");

        _quizRepo.Setup(r => r.GetByIdAsync(quiz.Id)).ReturnsAsync(quiz);

        var request = new QuizAttemptRequest { QuizId = quiz.Id, Answers = new[] { 0 } };

        var act = () => _service.SubmitQuizAttemptAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task SubmitQuizAttemptAsync_NonexistentQuiz_ShouldThrowNotFoundException()
    {
        _quizRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Quiz?)null);

        var request = new QuizAttemptRequest { QuizId = Guid.NewGuid(), Answers = new[] { 0 } };

        var act = () => _service.SubmitQuizAttemptAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetUserQuizHistoryAsync_ShouldReturnOrderedByDate()
    {
        var quizId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var attempt = new DomainQuizAttempt(userId, quizId, new[] { 0 }, 1, 1);
        var quiz = new Quiz("Test", "Desc", "sorting", 1, 10);

        _attemptRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<DomainQuizAttempt, bool>>>()))
            .ReturnsAsync(new[] { attempt });
        _quizRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(quiz);

        var result = await _service.GetUserQuizHistoryAsync(userId);

        result.Should().NotBeEmpty();
    }
}

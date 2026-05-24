using FluentAssertions;
using FluentValidation.TestHelper;
using VisualizationDSA.Application.DTOs;
using VisualizationDSA.Application.Validators;

namespace Application.Tests.Validators;

public class QuizAttemptRequestValidatorTests
{
    private readonly QuizAttemptRequestValidator _validator = new();

    [Fact]
    public void Valid_ShouldPass()
    {
        var request = new QuizAttemptRequest { QuizId = Guid.NewGuid(), Answers = new[] { 0, 1, 2 } };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EmptyQuizId_ShouldFail()
    {
        var request = new QuizAttemptRequest { QuizId = Guid.Empty, Answers = new[] { 0 } };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.QuizId);
    }

    [Fact]
    public void NullAnswers_ShouldFail()
    {
        var request = new QuizAttemptRequest { QuizId = Guid.NewGuid(), Answers = null! };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Answers);
    }
}

public class XPAwardRequestValidatorTests
{
    private readonly XPAwardRequestValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(200)]
    public void ValidAmount_ShouldPass(int amount)
    {
        var request = new XPAwardRequest { Amount = amount };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(201)]
    [InlineData(1000)]
    public void InvalidAmount_ShouldFail(int amount)
    {
        var request = new XPAwardRequest { Amount = amount };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }
}

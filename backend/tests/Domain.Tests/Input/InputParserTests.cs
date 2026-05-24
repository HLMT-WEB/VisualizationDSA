using FluentAssertions;
using VisualizationDSA.Domain.Input;

namespace Domain.Tests.Input;

public class InputParserTests
{
    [Theory]
    [InlineData("1,2,3", new[] { 1, 2, 3 })]
    [InlineData("10, 20, 30", new[] { 10, 20, 30 })]
    [InlineData("42", new[] { 42 })]
    [InlineData("-5, 0, 5", new[] { -5, 0, 5 })]
    [InlineData("+10, -20", new[] { 10, -20 })]
    public void ParseArray_ValidInput_ShouldReturnCorrectArray(string input, int[] expected)
    {
        var result = InputParser.ParseArray(input);

        result.Should().Equal(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ParseArray_EmptyOrNull_ShouldThrowArgumentException(string? input)
    {
        var act = () => InputParser.ParseArray(input!);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("1,2,abc")]
    [InlineData("1;;2")]
    [InlineData("1.5, 2.3")]
    public void ParseArray_InvalidFormat_ShouldThrowFormatException(string input)
    {
        var act = () => InputParser.ParseArray(input);

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void ParseArray_ShouldTrimWhitespace()
    {
        var result = InputParser.ParseArray("  1 , 2 , 3  ");

        result.Should().Equal(1, 2, 3);
    }
}

public class ConstraintResolverTests
{
    [Theory]
    [InlineData("bubble-sort", 50)]
    [InlineData("quick-sort", 150)]
    [InlineData("linear-search", 100)]
    [InlineData("bst", 15)]
    [InlineData("stack", 20)]
    [InlineData("queue", 20)]
    public void GetAllowedLimit_KnownAlgorithm_ShouldReturnCorrectLimit(string algorithmId, int expectedLimit)
    {
        var limit = ConstraintResolver.GetAllowedLimit(algorithmId);

        limit.Should().Be(expectedLimit);
    }

    [Fact]
    public void GetAllowedLimit_UnknownAlgorithm_ShouldReturnDefault10()
    {
        var limit = ConstraintResolver.GetAllowedLimit("unknown-algo");

        limit.Should().Be(10);
    }

    [Fact]
    public void GetAllowedLimit_ShouldBeCaseInsensitive()
    {
        var limit = ConstraintResolver.GetAllowedLimit("BUBBLE-SORT");

        limit.Should().Be(50);
    }

    [Fact]
    public void ValidateSize_WithinLimit_ShouldReturnTrue()
    {
        var isValid = ConstraintResolver.ValidateSize("bubble-sort", 30, out int allowedLimit);

        isValid.Should().BeTrue();
        allowedLimit.Should().Be(50);
    }

    [Fact]
    public void ValidateSize_ExceedsLimit_ShouldReturnFalse()
    {
        var isValid = ConstraintResolver.ValidateSize("bubble-sort", 51, out int allowedLimit);

        isValid.Should().BeFalse();
        allowedLimit.Should().Be(50);
    }

    [Fact]
    public void ValidateSize_ExactlyAtLimit_ShouldReturnTrue()
    {
        var isValid = ConstraintResolver.ValidateSize("bst", 15, out _);

        isValid.Should().BeTrue();
    }
}

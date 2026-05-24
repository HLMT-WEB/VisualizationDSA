using FluentAssertions;
using VisualizationDSA.Domain.Strategies;

namespace Domain.Tests.Strategies;

public class LinearSearchStrategyTests
{
    private readonly LinearSearchStrategy _strategy = new();

    [Fact]
    public void AlgorithmId_ShouldBeLinearSearch()
    {
        _strategy.AlgorithmId.Should().Be("linear-search");
        _strategy.Category.Should().Be("Searching");
    }

    [Fact]
    public void Execute_ShouldProduceFrames()
    {
        var frames = _strategy.Execute(new[] { 2, 3, 4, 10, 40, 10 });

        frames.Should().NotBeEmpty();
        frames.Should().AllSatisfy(f =>
        {
            f.StepId.Should().BeGreaterThan(0);
            f.Explanation.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public void GetMetadata_ShouldReturnLinearComplexity()
    {
        var meta = _strategy.GetMetadata();

        meta.TimeComplexity.Should().Be("O(N)");
        meta.SpaceComplexity.Should().Be("O(1)");
    }
}

public class BinarySearchStrategyTests
{
    private readonly BinarySearchStrategy _strategy = new();

    [Fact]
    public void AlgorithmId_ShouldBeBinarySearch()
    {
        _strategy.AlgorithmId.Should().Be("binary-search");
        _strategy.Category.Should().Be("Searching");
    }

    [Fact]
    public void Execute_SortedArray_ShouldFindTarget()
    {
        // Last element is the target
        var frames = _strategy.Execute(new[] { 2, 5, 8, 12, 16, 23, 38, 12 });

        frames.Should().NotBeEmpty();
        frames.Should().Contain(f => f.Explanation.Contains("Tìm thấy"));
    }

    [Fact]
    public void Execute_UnsortedArray_ShouldThrowArgumentException()
    {
        var act = () => _strategy.Execute(new[] { 10, 5, 3, 20, 5 });

        act.Should().Throw<ArgumentException>()
            .WithMessage("*sắp xếp*");
    }

    [Fact]
    public void Execute_EmptyArray_ShouldReturnFrame()
    {
        var frames = _strategy.Execute(Array.Empty<int>());

        frames.Should().HaveCount(1);
        frames[0].Explanation.Should().Contain("rỗng");
    }

    [Fact]
    public void GetMetadata_ShouldReturnLogComplexity()
    {
        var meta = _strategy.GetMetadata();

        meta.TimeComplexity.Should().Be("O(log N)");
    }
}

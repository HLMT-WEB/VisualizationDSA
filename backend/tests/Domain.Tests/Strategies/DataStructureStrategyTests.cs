using FluentAssertions;
using VisualizationDSA.Domain.Strategies;

namespace Domain.Tests.Strategies;

public class StackStrategyTests
{
    private readonly StackStrategy _strategy = new();

    [Fact]
    public void AlgorithmId_ShouldBeStack()
    {
        _strategy.AlgorithmId.Should().Be("stack");
        _strategy.Category.Should().Be("Stack-Queue");
    }

    [Fact]
    public void Execute_ShouldProduceFrames()
    {
        var frames = _strategy.Execute(new[] { 10, 20, 30 });

        frames.Should().NotBeEmpty();
        frames.Should().AllSatisfy(f =>
        {
            f.StepId.Should().BeGreaterThan(0);
            f.Explanation.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public void Execute_PushAndPopAll_ShouldEndEmpty()
    {
        var frames = _strategy.Execute(new[] { 10, 20, 30 });

        frames.Last().DataState.Should().BeEmpty();
        frames.Last().Explanation.Should().Contain("rỗng");
    }

    [Fact]
    public void GetMetadata_ShouldReturnO1Complexity()
    {
        var meta = _strategy.GetMetadata();

        meta.TimeComplexity.Should().Contain("O(1)");
    }
}

public class QueueStrategyTests
{
    private readonly QueueStrategy _strategy = new();

    [Fact]
    public void AlgorithmId_ShouldBeQueue()
    {
        _strategy.AlgorithmId.Should().Be("queue");
        _strategy.Category.Should().Be("Stack-Queue");
    }

    [Fact]
    public void Execute_ShouldProduceFrames()
    {
        var frames = _strategy.Execute(new[] { 5, 10, 15 });

        frames.Should().NotBeEmpty();
    }

    [Fact]
    public void Execute_EnqueueAndDequeueAll_ShouldEndEmpty()
    {
        var frames = _strategy.Execute(new[] { 5, 10, 15 });

        frames.Last().DataState.Should().BeEmpty();
        frames.Last().Explanation.Should().Contain("rỗng");
    }

    [Fact]
    public void GetMetadata_ShouldReturnO1Complexity()
    {
        var meta = _strategy.GetMetadata();

        meta.TimeComplexity.Should().Contain("O(1)");
    }
}

public class BSTStrategyTests
{
    private readonly BSTStrategy _strategy = new();

    [Fact]
    public void AlgorithmId_ShouldBeBST()
    {
        _strategy.AlgorithmId.Should().Be("bst");
        _strategy.Category.Should().Be("Tree");
    }

    [Fact]
    public void Execute_ShouldProduceFrames()
    {
        var frames = _strategy.Execute(new[] { 50, 30, 70, 20, 40 });

        frames.Should().NotBeEmpty();
    }

    [Fact]
    public void Execute_ShouldProduceInorderSortedResult()
    {
        var frames = _strategy.Execute(new[] { 50, 30, 70, 20, 40 });

        frames.Last().Explanation.Should().Contain("LNR");
    }

    [Fact]
    public void GetMetadata_ShouldReturnLogNComplexity()
    {
        var meta = _strategy.GetMetadata();

        meta.TimeComplexity.Should().Contain("log");
    }
}

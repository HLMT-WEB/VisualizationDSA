using FluentAssertions;
using VisualizationDSA.Domain.Strategies;

namespace Domain.Tests.Strategies;

public class BubbleSortStrategyTests
{
    private readonly BubbleSortStrategy _strategy = new();

    [Fact]
    public void AlgorithmId_ShouldBeBubbleSort()
    {
        _strategy.AlgorithmId.Should().Be("bubble-sort");
        _strategy.Category.Should().Be("Sorting");
    }

    [Fact]
    public void Execute_ShouldSortArray()
    {
        var frames = _strategy.Execute(new[] { 64, 34, 25, 12, 22, 11, 90 });

        frames.Should().NotBeEmpty();
        var lastFrame = frames.Last();
        lastFrame.DataState.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Execute_AlreadySorted_ShouldProduceFrames()
    {
        var frames = _strategy.Execute(new[] { 1, 2, 3, 4 });

        frames.Should().NotBeEmpty();
        frames.Last().DataState.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Execute_SingleElement_ShouldReturnFrames()
    {
        var frames = _strategy.Execute(new[] { 42 });

        frames.Should().NotBeEmpty();
        frames.Last().DataState.Should().Equal(42);
    }

    [Fact]
    public void GetMetadata_ShouldReturnCorrectComplexity()
    {
        var meta = _strategy.GetMetadata();

        meta.TimeComplexity.Should().Be("O(N²)");
        meta.SpaceComplexity.Should().Be("O(1)");
        meta.PseudoCode.Should().NotBeEmpty();
    }
}

public class SelectionSortStrategyTests
{
    private readonly SelectionSortStrategy _strategy = new();

    [Fact]
    public void Execute_ShouldSortArray()
    {
        var frames = _strategy.Execute(new[] { 29, 10, 14, 37, 13 });

        frames.Should().NotBeEmpty();
        frames.Last().DataState.Should().BeInAscendingOrder();
    }

    [Fact]
    public void AlgorithmId_ShouldBeSelectionSort()
    {
        _strategy.AlgorithmId.Should().Be("selection-sort");
    }
}

public class InsertionSortStrategyTests
{
    private readonly InsertionSortStrategy _strategy = new();

    [Fact]
    public void Execute_ShouldSortArray()
    {
        var frames = _strategy.Execute(new[] { 12, 11, 13, 5, 6 });

        frames.Should().NotBeEmpty();
        frames.Last().DataState.Should().BeInAscendingOrder();
    }

    [Fact]
    public void AlgorithmId_ShouldBeInsertionSort()
    {
        _strategy.AlgorithmId.Should().Be("insertion-sort");
    }
}

public class QuickSortStrategyTests
{
    private readonly QuickSortStrategy _strategy = new();

    [Fact]
    public void Execute_ShouldSortArray()
    {
        var frames = _strategy.Execute(new[] { 10, 80, 30, 90, 40, 50, 70 });

        frames.Should().NotBeEmpty();
        frames.Last().DataState.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Execute_ReverseOrder_ShouldSort()
    {
        var frames = _strategy.Execute(new[] { 5, 4, 3, 2, 1 });

        frames.Should().NotBeEmpty();
        frames.Last().DataState.Should().BeInAscendingOrder();
    }

    [Fact]
    public void AlgorithmId_ShouldBeQuickSort()
    {
        _strategy.AlgorithmId.Should().Be("quick-sort");
    }
}

public class MergeSortStrategyTests
{
    private readonly MergeSortStrategy _strategy = new();

    [Fact]
    public void Execute_ShouldSortArray()
    {
        var frames = _strategy.Execute(new[] { 38, 27, 43, 3, 9, 82, 10 });

        frames.Should().NotBeEmpty();
        frames.Last().DataState.Should().BeInAscendingOrder();
    }

    [Fact]
    public void AlgorithmId_ShouldBeMergeSort()
    {
        _strategy.AlgorithmId.Should().Be("merge-sort");
    }
}

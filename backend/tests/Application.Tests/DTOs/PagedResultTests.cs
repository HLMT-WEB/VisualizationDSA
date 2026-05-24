using FluentAssertions;
using VisualizationDSA.Application.DTOs;

namespace Application.Tests.DTOs;

public class PagedResultTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        var items = new List<string> { "a", "b", "c" };

        var result = new PagedResult<string>(items, 1, 10, 25);

        result.Items.Should().HaveCount(3);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(25);
    }

    [Fact]
    public void TotalPages_ShouldCalculateCorrectly()
    {
        var result = new PagedResult<int>(new List<int>(), 1, 10, 25);

        result.TotalPages.Should().Be(3);
    }

    [Fact]
    public void TotalPages_ExactDivision_ShouldNotRoundUp()
    {
        var result = new PagedResult<int>(new List<int>(), 1, 10, 20);

        result.TotalPages.Should().Be(2);
    }

    [Fact]
    public void TotalPages_SingleItem_ShouldBe1()
    {
        var result = new PagedResult<int>(new List<int> { 1 }, 1, 10, 1);

        result.TotalPages.Should().Be(1);
    }

    [Fact]
    public void TotalPages_EmptyCollection_ShouldBe0()
    {
        var result = new PagedResult<int>(new List<int>(), 1, 10, 0);

        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public void HasPreviousPage_FirstPage_ShouldBeFalse()
    {
        var result = new PagedResult<int>(new List<int>(), 1, 10, 50);

        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void HasPreviousPage_SecondPage_ShouldBeTrue()
    {
        var result = new PagedResult<int>(new List<int>(), 2, 10, 50);

        result.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_LastPage_ShouldBeFalse()
    {
        var result = new PagedResult<int>(new List<int>(), 5, 10, 50);

        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void HasNextPage_NotLastPage_ShouldBeTrue()
    {
        var result = new PagedResult<int>(new List<int>(), 3, 10, 50);

        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_OnlyOnePage_ShouldBeFalse()
    {
        var result = new PagedResult<int>(new List<int> { 1 }, 1, 10, 1);

        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void Items_ShouldBeReadOnly()
    {
        var items = new List<string> { "x", "y" };
        var result = new PagedResult<string>(items, 1, 10, 2);

        result.Items.Should().BeAssignableTo<IReadOnlyList<string>>();
    }

    [Fact]
    public void PageSize1_ManyItems_ShouldCalculateManyPages()
    {
        var result = new PagedResult<int>(new List<int> { 1 }, 1, 1, 100);

        result.TotalPages.Should().Be(100);
        result.HasNextPage.Should().BeTrue();
    }
}

using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using VisualizationDSA.Infrastructure.Services;

namespace Infrastructure.Tests.Services;

public class MemoryCacheServiceTests
{
    private readonly MemoryCacheService _cacheService;

    public MemoryCacheServiceTests()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheService = new MemoryCacheService(memoryCache);
    }

    [Fact]
    public void Set_And_Get_ShouldReturnCachedValue()
    {
        _cacheService.Set("key1", "value1");

        var result = _cacheService.Get<string>("key1");

        result.Should().Be("value1");
    }

    [Fact]
    public void Get_NonExistentKey_ShouldReturnDefault()
    {
        var result = _cacheService.Get<string>("nonexistent");

        result.Should().BeNull();
    }

    [Fact]
    public void Set_WithAbsoluteExpiration_ShouldCacheValue()
    {
        _cacheService.Set("key2", 42, absoluteExpiration: TimeSpan.FromMinutes(30));

        var result = _cacheService.Get<int>("key2");

        result.Should().Be(42);
    }

    [Fact]
    public void Set_WithSlidingExpiration_ShouldCacheValue()
    {
        _cacheService.Set("key3", new[] { 1, 2, 3 }, slidingExpiration: TimeSpan.FromMinutes(10));

        var result = _cacheService.Get<int[]>("key3");

        result.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Remove_ShouldDeleteCachedValue()
    {
        _cacheService.Set("remove-me", "data");
        _cacheService.Get<string>("remove-me").Should().Be("data");

        _cacheService.Remove("remove-me");

        _cacheService.Get<string>("remove-me").Should().BeNull();
    }

    [Fact]
    public void RemoveByPrefix_ShouldDeleteMatchingKeys()
    {
        _cacheService.Set("quizzes:list", "all");
        _cacheService.Set("quizzes:topic:sorting", "sorting");
        _cacheService.Set("quizzes:topic:oop", "oop");
        _cacheService.Set("badges:list", "badges");

        _cacheService.RemoveByPrefix("quizzes:");

        _cacheService.Get<string>("quizzes:list").Should().BeNull();
        _cacheService.Get<string>("quizzes:topic:sorting").Should().BeNull();
        _cacheService.Get<string>("quizzes:topic:oop").Should().BeNull();
        _cacheService.Get<string>("badges:list").Should().Be("badges");
    }

    [Fact]
    public void TryGet_ExistingKey_ShouldReturnTrueAndValue()
    {
        _cacheService.Set("try-key", "try-value");

        var found = _cacheService.TryGet<string>("try-key", out var value);

        found.Should().BeTrue();
        value.Should().Be("try-value");
    }

    [Fact]
    public void TryGet_NonExistentKey_ShouldReturnFalse()
    {
        var found = _cacheService.TryGet<string>("missing", out var value);

        found.Should().BeFalse();
        value.Should().BeNull();
    }

    [Fact]
    public void Set_ComplexObject_ShouldCacheCorrectly()
    {
        var data = new TestData { Name = "Test", Value = 100 };
        _cacheService.Set("complex", data);

        var result = _cacheService.Get<TestData>("complex");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Test");
        result.Value.Should().Be(100);
    }

    [Fact]
    public void Set_OverwriteExistingKey_ShouldUpdateValue()
    {
        _cacheService.Set("overwrite", "original");
        _cacheService.Set("overwrite", "updated");

        var result = _cacheService.Get<string>("overwrite");

        result.Should().Be("updated");
    }

    [Fact]
    public void RemoveByPrefix_NoMatchingKeys_ShouldNotThrow()
    {
        _cacheService.Set("abc:1", "val");

        var act = () => _cacheService.RemoveByPrefix("xyz:");

        act.Should().NotThrow();
        _cacheService.Get<string>("abc:1").Should().Be("val");
    }

    [Fact]
    public void Set_ListOfObjects_ShouldCacheCorrectly()
    {
        var list = new List<TestData>
        {
            new() { Name = "A", Value = 1 },
            new() { Name = "B", Value = 2 }
        };
        _cacheService.Set("list-key", list, TimeSpan.FromHours(1));

        var result = _cacheService.Get<List<TestData>>("list-key");

        result.Should().HaveCount(2);
        result![0].Name.Should().Be("A");
        result[1].Value.Should().Be(2);
    }

    private class TestData
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}

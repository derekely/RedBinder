using FluentAssertions;
using Xunit;

namespace TestProject1;

public class Tests
{
    [Fact]
    public void Setup()
    {
    }

    [Fact]
    public void Test1()
    {
        true.Should().BeFalse(); // Fail so that I write these tests
    }
}
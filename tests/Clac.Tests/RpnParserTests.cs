namespace Clac.Tests;

using Xunit;
using Clac.Core;

public class RpnParserTests
{
    [Fact]
    public void Parse_EmptyString_ShouldReturnEmptyList()
    {
        var result = RpnParser.Parse("   ");
        Assert.True(result.IsSuccessful);
        Assert.Empty(result.Value);
    }

    [Fact]
    public void Parse_InvalidString_ShouldReturnError()
    {
        var result = RpnParser.Parse("1 2 3 + - * / bad content");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Invalid input", result.Error.Message);
        Assert.Contains("bad content", result.Error.Message);
    }
}
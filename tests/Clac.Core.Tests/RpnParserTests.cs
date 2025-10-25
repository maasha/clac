namespace Clac.Core.Tests;

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

    [Fact]
    public void Parse_ValidString_ShouldReturnListOfTokens()
    {
        var result = RpnParser.Parse("1 2 3 + - * / -1 0.2 .3 -0.4");

        Assert.True(result.IsSuccessful);
        Assert.Equal(11, result.Value.Count);

        Token[] expected =
        [
            Token.CreateNumber(1),
            Token.CreateNumber(2),
            Token.CreateNumber(3),
            Token.CreateOperator(OperatorSymbol.Add),
            Token.CreateOperator(OperatorSymbol.Subtract),
            Token.CreateOperator(OperatorSymbol.Multiply),
            Token.CreateOperator(OperatorSymbol.Divide),
            Token.CreateNumber(-1),
            Token.CreateNumber(0.2),
            Token.CreateNumber(0.3),
            Token.CreateNumber(-0.4),
        ];

        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData("1e10")]
    [InlineData("1E-5")]
    [InlineData("2.5e3")]
    public void Parse_ScientificNotation_ShouldParseCorrectly(string input)
    {
        var result = RpnParser.Parse(input);
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
    }
}
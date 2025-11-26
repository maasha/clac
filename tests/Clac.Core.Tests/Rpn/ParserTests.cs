using Clac.Core.Rpn;
using Clac.Core.Operations;
using Clac.Core.Functions;

namespace Clac.Core.Tests.Rpn;

public class ParserTests
{
    private readonly OperatorRegistry _operatorRegistry;
    private readonly FunctionRegistry _functionRegistry;
    private readonly Parser _parser;

    public ParserTests()
    {
        _operatorRegistry = new DefaultOperatorRegistry();
        _functionRegistry = new DefaultFunctionRegistry();
        _parser = new Parser(_operatorRegistry, _functionRegistry);
    }

    [Fact]
    public void Parse_EmptyString_ShouldReturnEmptyList()
    {
        var result = _parser.Parse("   ");
        Assert.True(result.IsSuccessful);
        Assert.Empty(result.Value);
    }

    [Fact]
    public void Parse_InvalidString_ShouldReturnError()
    {
        var result = _parser.Parse("1 2 3 + - * / bad content");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Invalid input", result.Error.Message);
        Assert.Contains("bad content", result.Error.Message);
    }

    [Fact]
    public void Parse_ValidString_ShouldReturnListOfTokens()
    {
        var result = _parser.Parse("1 2 3 + - * / -1 0.2 .3 -0.4");

        Assert.True(result.IsSuccessful);
        Assert.Equal(11, result.Value.Count);

        Token[] expected =
        [
            Token.CreateNumber(1),
            Token.CreateNumber(2),
            Token.CreateNumber(3),
            Token.CreateOperator("+"),
            Token.CreateOperator("-"),
            Token.CreateOperator("*"),
            Token.CreateOperator("/"),
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
        var result = _parser.Parse(input);
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
    }

    [Fact]
    public void Parse_ClearFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("clear()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }

    [Fact]
    public void Parse_PopFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("pop()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }

    [Fact]
    public void Parse_SwapFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("swap()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }

    [Fact]
    public void Parse_SumFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("sum()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }

    [Fact]
    public void Parse_SquareRootFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("sqrt()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }

    [Fact]
    public void Parse_PowFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("pow()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }

    [Fact]
    public void Parse_recipFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("recip()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }

    [Fact]
    public void Parse_UppercaseFunction_ShouldReturnFunctionToken()
    {
        var result = _parser.Parse("recip()");
        Assert.True(result.IsSuccessful);
        Assert.Single(result.Value);
        Assert.IsType<Token.FunctionToken>(result.Value[0]);
    }
}
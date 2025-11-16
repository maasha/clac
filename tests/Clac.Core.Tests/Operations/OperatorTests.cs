namespace Clac.Core.Tests.Operations;

using Clac.Core.Operations;
using Xunit;
using Clac.Core.Enums;

public class OperatorTests
{
    [Fact]
    public void GetOperatorSymbol_ValidAddSymbol_ShouldReturnAdd()
    {
        var result = Operator.GetOperatorSymbol("+");
        Assert.True(result.IsSuccessful);
        Assert.Equal(OperatorSymbol.Add, result.Value);
    }

    [Fact]
    public void GetOperatorSymbol_ValidSubtractSymbol_ShouldReturnSubtract()
    {
        var result = Operator.GetOperatorSymbol("-");
        Assert.True(result.IsSuccessful);
        Assert.Equal(OperatorSymbol.Subtract, result.Value);
    }

    [Fact]
    public void GetOperatorSymbol_ValidMultiplySymbol_ShouldReturnMultiply()
    {
        var result = Operator.GetOperatorSymbol("*");
        Assert.True(result.IsSuccessful);
        Assert.Equal(OperatorSymbol.Multiply, result.Value);
    }

    [Fact]
    public void GetOperatorSymbol_ValidDivideSymbol_ShouldReturnDivide()
    {
        var result = Operator.GetOperatorSymbol("/");
        Assert.True(result.IsSuccessful);
        Assert.Equal(OperatorSymbol.Divide, result.Value);
    }

    [Fact]
    public void IsValidOperator_ValidAddSymbol_ShouldReturnTrue()
    {
        var result = Operator.IsValidOperator("+");
        Assert.True(result);
    }

    [Fact]
    public void IsValidOperator_ValidSubtractSymbol_ShouldReturnTrue()
    {
        var result = Operator.IsValidOperator("-");
        Assert.True(result);
    }

    [Fact]
    public void IsValidOperator_InvalidSymbol_ShouldReturnFalse()
    {
        var result = Operator.IsValidOperator("%");
        Assert.False(result);
    }

    [Fact]
    public void GetOperatorSymbol_InvalidSymbol_ShouldReturnError()
    {
        var result = Operator.GetOperatorSymbol("%");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Invalid operator symbol", result.Error.Message);
    }
}


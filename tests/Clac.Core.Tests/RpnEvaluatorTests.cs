namespace Clac.Core.Tests;

using Xunit;
using Clac.Core;
using static Clac.Core.ErrorMessages;
using Clac.Core.Enums;

public class RpnEvaluatorTests
{
    [Fact]
    public void BadOperator_ShouldReturnError()
    {
        var badOperator = (OperatorSymbol)999;
        var result = RpnEvaluator.Evaluate(1, 2, badOperator);
        Assert.False(result.IsSuccessful);
        Assert.Contains(UnknownOperator(badOperator), result.Error.Message);
    }

    [Fact]
    public void SimpleAddition_ShouldReturnCorrectResult()
    {
        var result = RpnEvaluator.Evaluate(1, 2, OperatorSymbol.Add);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
    }

    [Fact]
    public void SimpleSubtraction_ShouldReturnCorrectResult()
    {
        var result = RpnEvaluator.Evaluate(1, 2, OperatorSymbol.Subtract);
        Assert.True(result.IsSuccessful);
        Assert.Equal(-1, result.Value);
    }

    [Fact]
    public void SimpleMultiplication_ShouldReturnCorrectResult()
    {
        var result = RpnEvaluator.Evaluate(1, 2, OperatorSymbol.Multiply);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Value);
    }

    [Fact]
    public void SimpleDivision_ShouldReturnCorrectResult()
    {
        var result = RpnEvaluator.Evaluate(1, 2, OperatorSymbol.Divide);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0.5, result.Value);
    }

    [Fact]
    public void DivisionByZero_ShouldReturnError()
    {
        var result = RpnEvaluator.Evaluate(1, 0, OperatorSymbol.Divide);
        Assert.False(result.IsSuccessful);
        Assert.Contains(DivisionByZero, result.Error.Message);
    }
}
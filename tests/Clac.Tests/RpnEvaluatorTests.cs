namespace Clac.Tests;

using Xunit;
using Clac.Core;

public class RpnEvaluatorTests
{
    [Fact]
    public void SimpleAddition_ShouldReturnCorrectResult()
    {
        var evaluator = new RpnEvaluator();
        var result = evaluator.Evaluate(1, 2, OperatorSymbol.Add);
        Assert.Equal(3, result);
    }
}
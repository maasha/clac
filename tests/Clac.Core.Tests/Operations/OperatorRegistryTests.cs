using Clac.Core.Operations;

namespace Clac.Core.Tests.Operations;

public class OperatorRegistryTests
{
    [Fact]
    public void Register_ShouldAddOperatorToRegistry()
    {
        var operatorRegistry = new OperatorRegistry();
        var addOperator = new AddOperator();
        operatorRegistry.Register(addOperator);
        Assert.Equal(addOperator.Name, operatorRegistry.GetOperator("+").Value.Name);
    }

    [Fact]
    public void GetOperator_WithUnRegisteredOperator_ShouldReturnError()
    {
        var operatorRegistry = new OperatorRegistry();
        var result = operatorRegistry.GetOperator("+");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Operator '+' not found", result.Error.Message);
    }

    [Fact]
    public void IsValidOperator_WithUnRegisteredOperator_ShouldReturnFalse()
    {
        var operatorRegistry = new OperatorRegistry();
        var result = operatorRegistry.IsValidOperator("%");
        Assert.False(result.IsSuccessful);
    }

    [Theory]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("*")]
    [InlineData("/")]
    public void IsValidOperator_WithRegisteredOperator_ShouldReturnTrue(string symbol)
    {
        var operatorRegistry = new DefaultOperatorRegistry();
        var result = operatorRegistry.IsValidOperator(symbol);
        Assert.True(result.IsSuccessful);
    }
}
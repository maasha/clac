namespace Clac.Core.Tests.Operations;

using Clac.Core.Operations;
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
        Assert.Contains("Operator 'Add' not found", result.Error.Message);
    }

    [Fact]
    public void IsValidOperator_WithInValidOperator_ShouldReturnFalse()
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
    public void IsValidOperator_WithValidOperator_ShouldReturnTrue(string symbol)
    {
        var operatorRegistry = new OperatorRegistry();
        var result = operatorRegistry.IsValidOperator(symbol);
        Assert.True(result.IsSuccessful);
    }
}
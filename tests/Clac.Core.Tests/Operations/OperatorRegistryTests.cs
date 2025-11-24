namespace Clac.Core.Tests.Operations;

using Clac.Core.Operations;
using static Clac.Core.Enums.OperatorSymbol;
public class OperatorRegistryTests
{
    [Fact]
    public void Register_ShouldAddOperatorToRegistry()
    {
        var operatorRegistry = new OperatorRegistry();
        var addOperator = new AddOperator();
        operatorRegistry.Register(addOperator);
        Assert.Equal(addOperator.Name, operatorRegistry.GetOperator(Add).Value.Name);
    }

    [Fact]
    public void GetOperator_WithUnRegisteredOperator_ShouldReturnError()
    {
        var operatorRegistry = new OperatorRegistry();
        var result = operatorRegistry.GetOperator(Add);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Operator 'Add' not found", result.Error.Message);
    }

    [Fact]
    public void IsValidOperator_WithInValidOperator_ShouldReturnFalse()
    {
        Assert.False(OperatorRegistry.IsValidOperator("%"));
    }

    [Theory]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("*")]
    [InlineData("/")]
    public void IsValidOperator_WithValidOperator_ShouldReturnTrue(string symbol)
    {
        Assert.True(OperatorRegistry.IsValidOperator(symbol));
    }
}
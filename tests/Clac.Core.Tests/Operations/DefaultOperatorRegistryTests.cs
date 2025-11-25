using Clac.Core.Operations;

namespace Clac.Core.Tests.Operations;

public class DefaultOperatorRegistryTests
{
    [Theory]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("*")]
    [InlineData("/")]
    public void CreateDefaultOperatorRegistry_ShouldHaveDefaultOperators(string symbol)
    {
        var registry = new DefaultOperatorRegistry();
        var result = registry.IsValidOperator(symbol);
        Assert.True(result.IsSuccessful);
    }
}
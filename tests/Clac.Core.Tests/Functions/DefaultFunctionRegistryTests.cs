using Clac.Core.Functions;
namespace Clac.Core.Tests.Functions;

public class DefaultFunctionRegistryTests
{
    [Theory]
    [InlineData("pop")]
    [InlineData("swap")]
    [InlineData("sum")]
    [InlineData("sqrt")]
    [InlineData("pow")]
    public void CreateDefaultFunctionRegistry_ShouldHaveDefaultFunctions(string functionName)
    {
        var registry = new DefaultFunctionRegistry();
        var result = registry.IsValidFunction(functionName);
        Assert.True(result.IsSuccessful);
    }
}
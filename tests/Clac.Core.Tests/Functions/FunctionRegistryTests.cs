using Clac.Core.Functions;
namespace Clac.Core.Tests.Functions;

public class FunctionRegistryTests
{
    [Fact]
    public void Register_ShouldAddFunctionToRegistry()
    {
        var functionRegistry = new FunctionRegistry();
        var function = new PopFunction();
        functionRegistry.Register(function);
        Assert.Equal(function.Name, functionRegistry.GetFunction(function.Name).Value.Name);
    }

    [Fact]
    public void GetFunction_WithUnRegisteredFunction_ShouldReturnError()
    {
        var functionRegistry = new FunctionRegistry();
        var result = functionRegistry.GetFunction("clear");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Function 'clear' not found", result.Error.Message);
    }

    [Fact]
    public void IsValidFunction_WithUnRegisteredFunction_ShouldReturnFalse()
    {
        var functionRegistry = new FunctionRegistry();
        var result = functionRegistry.IsValidFunction("clear");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Function 'clear' not found", result.Error.Message);
    }

    [Fact]
    public void IsValidFunction_WithRegisteredFunction_ShouldReturnTrue()
    {
        var functionRegistry = new FunctionRegistry();
        var function = new PopFunction();
        functionRegistry.Register(function);
        var result = functionRegistry.IsValidFunction(function.Name.ToLowerInvariant());
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void IsValidFunction_WithRegisteredFunction_ShouldReturnTrue_CaseInsensitive()
    {
        var functionRegistry = new FunctionRegistry();
        var function = new PopFunction();
        functionRegistry.Register(function);
        var result = functionRegistry.IsValidFunction(function.Name.ToUpperInvariant());
        Assert.True(result.IsSuccessful);
    }
}
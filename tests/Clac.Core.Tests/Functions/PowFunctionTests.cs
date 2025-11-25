using Clac.Core.Functions;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Tests.Functions;

public class PowFunctionTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var function = new PowFunction();
        Assert.Equal("Pow", function.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var function = new PowFunction();
        Assert.Equal("Calculates the power of the last two numbers on the stack", function.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var function = new PowFunction();
        var stack = new Stack();
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithLessThanTwoNumbersOnStack_ShouldDoNothing()
    {
        var function = new PowFunction();
        var stack = new Stack();
        stack.Push(1);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithThreeNumbersOnStack_ShouldCalculateThePowerOfTheLastTwoNumbers()
    {
        var function = new PowFunction();
        var stack = new Stack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal([1, 8], stack.ToArray());
    }
}

using Clac.Core.Functions;
using Clac.Core.Rpn;

namespace Clac.Core.Tests.Functions;

public class SwapFunctionTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var function = new SwapFunction();
        Assert.Equal("Swap", function.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var function = new SwapFunction();
        Assert.Equal("Swaps the last two numbers on the stack", function.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var function = new SwapFunction();
        var stack = new Stack();
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithOneNumberOnStack_ShouldDoNothing()
    {
        var function = new SwapFunction();
        var stack = new Stack();
        stack.Push(1);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
        Assert.Equal(1, stack.Count);
    }

    [Fact]
    public void Execute_WithThreeNumbersOnStack_ShouldSwapTheLastTwoNumbers()
    {
        var function = new SwapFunction();
        var stack = new Stack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        Assert.Equal(3, stack.Count);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal([1, 3, 2], stack.ToArray());
    }
}
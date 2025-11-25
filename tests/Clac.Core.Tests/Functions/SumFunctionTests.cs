using Clac.Core.Functions;
using Clac.Core.Rpn;

namespace Clac.Core.Tests.Functions;

public class SumFunctionTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var function = new SumFunction();
        Assert.Equal("Sum", function.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var function = new SumFunction();
        Assert.Equal("Sums all the numbers on the stack", function.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var function = new SumFunction();
        var stack = new Stack();
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithNonEmptyStack_ShouldSumTheNumbers()
    {
        var function = new SumFunction();
        var stack = new Stack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        Assert.Equal(3, stack.Count);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(6, result.Value);
        Assert.Equal(1, stack.Count);
    }
}
using Clac.Core.Commands;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Tests.Commands;

public class SwapCommandTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var command = new SwapCommand();
        Assert.Equal("Swap", command.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var command = new SwapCommand();
        Assert.Equal("Swaps the last two numbers on the stack", command.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var command = new SwapCommand();
        var stack = new Stack();
        var result = command.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithOneNumberOnStack_ShouldDoNothing()
    {
        var command = new SwapCommand();
        var stack = new Stack();
        stack.Push(1);
        var result = command.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
        Assert.Equal(1, stack.Count);
    }

    [Fact]
    public void Execute_WithThreeNumbersOnStack_ShouldSwapTheLastTwoNumbers()
    {
        var command = new SwapCommand();
        var stack = new Stack();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        Assert.Equal(3, stack.Count);
        var result = command.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal([1, 3, 2], stack.ToArray());
    }
}
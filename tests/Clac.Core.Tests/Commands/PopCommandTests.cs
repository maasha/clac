using Clac.Core.Commands;
using Clac.Core.Rpn;

namespace Clac.Core.Tests.Commands;

public class PopCommandTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var command = new PopCommand();
        Assert.Equal("Pop", command.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var command = new PopCommand();
        Assert.Equal("Removes the last number from the stack", command.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var command = new PopCommand();
        var stack = new Stack();
        var result = command.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithNonEmptyStack_ShouldPopTheLastNumber()
    {
        var command = new PopCommand();
        var stack = new Stack();
        stack.Push(1);
        stack.Push(2);
        Assert.Equal(2, stack.Count);
        var result = command.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Value);
        Assert.Equal(1, stack.Count);
    }
}
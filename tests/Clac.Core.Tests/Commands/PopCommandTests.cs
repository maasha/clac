using Clac.Core.Commands;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Tests.Commands;

public class PopCommandTests
{
    [Fact]
    public void Symbol_ShouldBeCorrect()
    {
        var popCommand = new PopCommand();
        Assert.Equal("pop()", popCommand.Symbol);
    }

    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var popCommand = new PopCommand();
        Assert.Equal("Pop", popCommand.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var popCommand = new PopCommand();
        Assert.Equal("Removes the last number from the stack", popCommand.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var popCommand = new PopCommand();
        var stack = new Stack();
        var result = popCommand.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithNonEmptyStack_ShouldPopTheLastNumber()
    {
        var popCommand = new PopCommand();
        var stack = new Stack();
        stack.Push(1);
        stack.Push(2);
        Assert.Equal(2, stack.Count);
        var result = popCommand.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Value);
        Assert.Equal(1, stack.Count);
    }
}
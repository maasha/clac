using Clac.Core;
using Clac.Core.History;
using Xunit;

namespace Clac.Core.Tests.History;

public class StackAndInputHistoryTests
{
    [Fact]
    public void Push_WithValidStackAndInput_ShouldSucceed()
    {
        StackAndInputHistory history = new();
        var stack = new Stack();
        stack.Push(123);

        var result = history.Push(stack, "1 2 3");

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Push_WhenInputPushFails_ShouldRollbackStackPush()
    {
        StackAndInputHistory history = new();
        var stack = new Stack();
        stack.Push(123);

        var result = history.Push(stack, "");

        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public void Pop_WithHistory_ShouldReturnBothStackAndInput()
    {
        StackAndInputHistory history = new();
        var stack = new Stack();
        stack.Push(123);
        history.Push(stack, "1 2 3");

        var result = history.Pop();

        Assert.True(result.IsSuccessful);
        Assert.Equal(123, result.Value.stack.ToArray()[0]);
        Assert.Equal("1 2 3", result.Value.input);
    }

    [Fact]
    public void CanUndo_WithNoHistory_ShouldReturnFalse()
    {
        StackAndInputHistory history = new();

        Assert.False(history.CanUndo);
    }

    [Fact]
    public void CanUndo_WithHistory_ShouldReturnTrue()
    {
        StackAndInputHistory history = new();
        var stack = new Stack();
        stack.Push(123);
        history.Push(stack, "1 2 3");

        Assert.True(history.CanUndo);
    }
}


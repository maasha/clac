using Clac.Core.History;
using Clac.Core.Rpn;
using Xunit;

namespace Clac.Core.Tests.History;

public class HistoryComparerTests
{
    private StackAndInputHistory CreateHistoryWithOneEntry()
    {
        var history = new StackAndInputHistory();
        var stack = new Stack();
        stack.Push(123);
        history.Push(stack, "1 2 3");
        return history;
    }

    private StackAndInputHistory CreateHistoryWithMultipleEntries()
    {
        var history = new StackAndInputHistory();

        var stack1 = new Stack();
        stack1.Push(123);
        history.Push(stack1, "1 2 3");

        var stack2 = new Stack();
        stack2.Push(456);
        history.Push(stack2, "4 5 6");

        return history;
    }

    [Fact]
    public void AreEqual_WhenFirstIsNull_ShouldReturnFalse()
    {
        var second = new StackAndInputHistory();
        var stack = new Stack();
        stack.Push(123);
        second.Push(stack, "1 2 3");

        var result = HistoryComparer.AreEqual(null, second);

        Assert.False(result);
    }

    [Fact]
    public void AreEqual_WhenHistoriesAreEqual_ShouldReturnTrue()
    {
        var first = CreateHistoryWithOneEntry();
        var second = CreateHistoryWithOneEntry();

        var result = HistoryComparer.AreEqual(first, second);

        Assert.True(result);
    }

    [Fact]
    public void AreEqual_WhenStackCountsDiffer_ShouldReturnFalse()
    {
        var first = new StackAndInputHistory();
        var firstStack = new Stack();
        firstStack.Push(123);
        first.Push(firstStack, "1 2 3");

        var second = new StackAndInputHistory();
        var secondStack1 = new Stack();
        secondStack1.Push(123);
        second.Push(secondStack1, "1 2 3");
        var secondStack2 = new Stack();
        secondStack2.Push(456);
        second.Push(secondStack2, "4 5 6");

        var result = HistoryComparer.AreEqual(first, second);

        Assert.False(result);
    }

    [Fact]
    public void AreEqual_WhenInputCountsDiffer_ShouldReturnFalse()
    {
        var first = new StackAndInputHistory();
        var firstStack = new Stack();
        firstStack.Push(123);
        first.Push(firstStack, "1 2 3");

        var second = new StackAndInputHistory();
        var secondStack = new Stack();
        secondStack.Push(123);
        second.Push(secondStack, "1 2 3");
        var secondStack2 = new Stack();
        secondStack2.Push(456);
        second.Push(secondStack2, "4 5 6");

        var result = HistoryComparer.AreEqual(first, second);

        Assert.False(result);
    }

    [Fact]
    public void AreEqual_WhenStacksDiffer_ShouldReturnFalse()
    {
        var first = new StackAndInputHistory();
        var firstStack = new Stack();
        firstStack.Push(123);
        first.Push(firstStack, "1 2 3");

        var second = new StackAndInputHistory();
        var secondStack = new Stack();
        secondStack.Push(456);
        second.Push(secondStack, "1 2 3");

        var result = HistoryComparer.AreEqual(first, second);

        Assert.False(result);
    }

    [Fact]
    public void AreEqual_WhenInputsDiffer_ShouldReturnFalse()
    {
        var first = new StackAndInputHistory();
        var firstStack = new Stack();
        firstStack.Push(123);
        first.Push(firstStack, "1 2 3");

        var second = new StackAndInputHistory();
        var secondStack = new Stack();
        secondStack.Push(123);
        second.Push(secondStack, "4 5 6");

        var result = HistoryComparer.AreEqual(first, second);

        Assert.False(result);
    }

    [Fact]
    public void AreEqual_WhenBothAreEmpty_ShouldReturnTrue()
    {
        var first = new StackAndInputHistory();
        var second = new StackAndInputHistory();

        var result = HistoryComparer.AreEqual(first, second);

        Assert.True(result);
    }

    [Fact]
    public void AreEqual_WithMultipleEntries_ShouldReturnTrue()
    {
        var first = CreateHistoryWithMultipleEntries();
        var second = CreateHistoryWithMultipleEntries();

        var result = HistoryComparer.AreEqual(first, second);

        Assert.True(result);
    }
}


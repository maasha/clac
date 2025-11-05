namespace Clac.Core.Tests;

using Xunit;
using Clac.Core;

public class RpnStackTests
{
    [Fact]
    public void PeekWithEmptyStack_ShouldReturnError()
    {
        var stack = new RpnStack();
        var result = stack.Peek();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void PeekWithNonEmptyStack_ShouldReturnLastElement()
    {
        var stack = new RpnStack();
        stack.Push(1);
        var result = stack.Peek();
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Push_ShouldPushANumberOntoTheStack()
    {
        var stack = new RpnStack();
        stack.Push(1);
        Assert.Equal(1, stack.Peek());
    }

    [Fact]
    public void Clear_ShouldClearTheStack()
    {
        var stack = new RpnStack();
        stack.Push(1);
        stack.Clear();
        var result = stack.Peek();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void PopWithEmptyStack_ShouldReturnError()
    {
        var stack = new RpnStack();
        var result = stack.Pop();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void PopWithNonEmptyStack_ShouldReturnLastElement()
    {
        var stack = new RpnStack();
        stack.Push(1);
        var result = stack.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Count_ShouldReturnTheNumberOfElementsOnTheStack()
    {
        var stack = new RpnStack();
        stack.Push(1);
        stack.Push(2);
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void ToArray_ShouldReturnTheStackAsAnArray()
    {
        var stack = new RpnStack();
        stack.Push(1);
        stack.Push(2);
        Assert.Equal([1, 2], stack.ToArray());
    }

    [Fact]
    public void Swap_WithLessThanTwoElements_ShouldReturnError()
    {
        var stack = new RpnStack();
        stack.Push(1);
        var result = stack.Swap();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack has less than two elements", result.Error.Message);
    }

    [Fact]
    public void Sum_WithEmptyStack_ShouldReturnError()
    {
        var stack = new RpnStack();
        var result = stack.Sum();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void Sum_WithNonEmptyStack_ShouldReturnSumOfElements()
    {
        var stack = new RpnStack();
        stack.Push(1);
        stack.Push(2);
        var result = stack.Sum();
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
    }

    [Fact]
    public void Sqrt_WithEmptyStack_ShouldReturnError()
    {
        var stack = new RpnStack();
        var result = stack.Sqrt();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void Sqrt_WithNonEmptyStack_ShouldReturnSquareRootOfLastElement()
    {
        var stack = new RpnStack();
        stack.Push(4);
        var result = stack.Sqrt();
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Value);
    }

    [Fact]
    public void Sqrt_WithNegativeNumber_ShouldReturnError()
    {
        var stack = new RpnStack();
        stack.Push(-1);
        var result = stack.Sqrt();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Invalid: negative square root", result.Error.Message);
    }
}
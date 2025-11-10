namespace Clac.Core.Tests;

using Xunit;
using Clac.Core;

public class RpnStackTests
{
    private readonly RpnStack _stack;

    public RpnStackTests()
    {
        _stack = new RpnStack();
    }

    [Fact]
    public void PeekWithEmptyStack_ShouldReturnError()
    {
        var result = _stack.Peek();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void PeekWithNonEmptyStack_ShouldReturnLastElement()
    {
        _stack.Push(1);
        var result = _stack.Peek();
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Push_ShouldPushANumberOntoTheStack()
    {
        _stack.Push(1);
        Assert.Equal(1, _stack.Peek().Value);
    }

    [Fact]
    public void Clear_ShouldClearTheStack()
    {
        _stack.Push(1);
        _stack.Clear();
        var result = _stack.Peek();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void PopWithEmptyStack_ShouldReturnError()
    {
        var result = _stack.Pop();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void PopWithNonEmptyStack_ShouldReturnLastElement()
    {
        _stack.Push(1);
        var result = _stack.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Count_ShouldReturnTheNumberOfElementsOnTheStack()
    {
        _stack.Push(1);
        _stack.Push(2);
        Assert.Equal(2, _stack.Count);
    }

    [Fact]
    public void ToArray_ShouldReturnTheStackAsAnArray()
    {
        _stack.Push(1);
        _stack.Push(2);
        Assert.Equal([1, 2], _stack.ToArray());
    }

    [Fact]
    public void Swap_WithLessThanTwoElements_ShouldReturnError()
    {
        _stack.Push(1);
        var result = _stack.Swap();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack has less than two elements", result.Error.Message);
    }

    [Fact]
    public void Sum_WithEmptyStack_ShouldReturnError()
    {
        var result = _stack.Sum();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void Sum_WithNonEmptyStack_ShouldReturnSumOfElements()
    {
        _stack.Push(1);
        _stack.Push(2);
        var result = _stack.Sum();
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
    }

    [Fact]
    public void Sqrt_WithEmptyStack_ShouldReturnError()
    {
        var result = _stack.Sqrt();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void Sqrt_WithNonEmptyStack_ShouldReturnSquareRootOfLastElement()
    {
        _stack.Push(4);
        var result = _stack.Sqrt();
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Value);
    }

    [Fact]
    public void Sqrt_WithNegativeNumber_ShouldReturnError()
    {
        _stack.Push(-1);
        var result = _stack.Sqrt();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Invalid: negative square root", result.Error.Message);
    }

    [Fact]
    public void Pow_WithLessThanTwoElements_ShouldReturnError()
    {
        _stack.Push(2);
        var result = _stack.Pow();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack has less than two elements", result.Error.Message);
    }

    [Fact]
    public void Pow_WithTwoElements_ShouldReturnPowerOfLastTwoElements()
    {
        _stack.Push(2);
        _stack.Push(3);
        var result = _stack.Pow();
        Assert.True(result.IsSuccessful);
        Assert.Equal(8, result.Value);
    }

    [Fact]
    public void Reciprocal_WithNonEmptyStack_ShouldReturnReciprocalOfLastElement()
    {
        _stack.Push(4);
        var result = _stack.Reciprocal();
        Assert.True(result.IsSuccessful);
        Assert.Equal(0.25, result.Value);
    }

    [Fact]
    public void Reciprocal_WithEmptyStack_ShouldReturnError()
    {
        var result = _stack.Reciprocal();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack is empty", result.Error.Message);
    }

    [Fact]
    public void Reciprocal_WithZero_ShouldReturnError()
    {
        _stack.Push(0);
        var result = _stack.Reciprocal();
        Assert.False(result.IsSuccessful);
        Assert.Contains("Division by zero", result.Error.Message);
    }
}
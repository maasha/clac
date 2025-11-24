using Clac.Core.Operations;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Tests.Operations;

public class DivideOperatorTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var divideOperator = new DivideOperator();
        Assert.Equal("Divide", divideOperator.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var divideOperator = new DivideOperator();
        Assert.Equal("Divides two numbers", divideOperator.Description);
    }

    [Fact]
    public void MinimumStackSize_ShouldBeCorrect()
    {
        var divideOperator = new DivideOperator();
        Assert.Equal(2, divideOperator.MinimumStackSize);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnFalse()
    {
        var divideOperator = new DivideOperator();
        Assert.False(divideOperator.ValidateStackSize(1).IsSuccessful);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnErrorMessage()
    {
        var divideOperator = new DivideOperator();
        var result = divideOperator.ValidateStackSize(1);
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void ValidateStackSize_WithSufficientStackSize_ShouldReturnTrue()
    {
        var divideOperator = new DivideOperator();
        Assert.True(divideOperator.ValidateStackSize(2).IsSuccessful);
    }

    [Fact]
    public void Evaluate_WithInsufficientStackSize_ShouldReturnError()
    {
        var divideOperator = new DivideOperator();
        var result = divideOperator.Evaluate(new Stack());
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public void Evaluate_WithInsufficientStackSize_ShouldReturnErrorMessage()
    {
        var divideOperator = new DivideOperator();
        var result = divideOperator.Evaluate(new Stack());
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void Evaluate_WithInSufficientStackSize_ShouldNotChangeStack()
    {
        var stack = new Stack();
        stack.Push(123);
        var divideOperator = new DivideOperator();
        var result = divideOperator.Evaluate(stack);
        Assert.False(result.IsSuccessful);
        Assert.Equal(1, stack.Count);
        Assert.Equal(123, stack.Peek().Value);
    }

    [Fact]
    public void Evaluate_WithSufficientStackSize_ShouldReturnCorrectResult()
    {
        var stack = new Stack();
        stack.Push(6);
        stack.Push(2);
        var divideOperator = new DivideOperator();
        var result = divideOperator.Evaluate(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
    }

    [Fact]
    public void Evaluate_WithSufficientStackSize_ShouldPopTwoNumbersAndPushOneNumber()
    {
        var stack = new Stack();
        stack.Push(6);
        stack.Push(2);
        var divideOperator = new DivideOperator();
        var result = divideOperator.Evaluate(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, stack.Count);
        Assert.Equal(3, stack.Peek().Value);
    }

    [Fact]
    public void Evaluate_WithSufficientStackSizeAndZero_ShouldReturnError()
    {
        var stack = new Stack();
        stack.Push(6);
        stack.Push(0);
        var divideOperator = new DivideOperator();
        var result = divideOperator.Evaluate(stack);
        Assert.False(result.IsSuccessful);
        Assert.Contains(DivisionByZero, result.Error.Message);
    }
}
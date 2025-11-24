using Clac.Core.Operations;
using static Clac.Core.ErrorMessages;
using Clac.Core.Rpn;

namespace Clac.Core.Tests.Operations;

public class AddOperatorTests
{
    [Fact]

    public void Name_ShouldBeCorrect()
    {
        var addOperator = new AddOperator();
        Assert.Equal("Add", addOperator.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var addOperator = new AddOperator();
        Assert.Equal("Adds two numbers", addOperator.Description);
    }

    [Fact]
    public void MinimumStackSize_ShouldBeCorrect()
    {
        var addOperator = new AddOperator();
        Assert.Equal(2, addOperator.MinimumStackSize);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnFalse()
    {
        var addOperator = new AddOperator();
        Assert.False(addOperator.ValidateStackSize(1).IsSuccessful);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnErrorMessage()
    {
        var addOperator = new AddOperator();
        var result = addOperator.ValidateStackSize(1);
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void ValidateStackSize_WithSufficientStackSize_ShouldReturnTrue()
    {
        var addOperator = new AddOperator();
        Assert.True(addOperator.ValidateStackSize(2).IsSuccessful);
    }

    [Fact]
    public void Evaluate_WithInsufficientStackSize_ShouldReturnError()
    {
        var addOperator = new AddOperator();
        var result = addOperator.Evaluate(new Stack());
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public void Evaluate_WithInsufficientStackSize_ShouldReturnErrorMessage()
    {
        var addOperator = new AddOperator();
        var result = addOperator.Evaluate(new Stack());
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void Evaluate_WithInSufficientStackSize_ShouldNotChangeStack()
    {
        var stack = new Stack();
        stack.Push(123);
        var addOperator = new AddOperator();
        var result = addOperator.Evaluate(stack);
        Assert.False(result.IsSuccessful);
        Assert.Equal(1, stack.Count);
        Assert.Equal(123, stack.Peek().Value);
    }

    [Fact]
    public void Evaluate_WithSufficientStackSize_ShouldReturnCorrectResult()
    {
        var stack = new Stack();
        stack.Push(123);
        stack.Push(456);
        var addOperator = new AddOperator();
        var result = addOperator.Evaluate(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(579, result.Value);
    }

    [Fact]
    public void Evaluate_WithSufficientStackSize_ShouldPopTwoNumbersAndPushOneNumber()
    {
        var stack = new Stack();
        stack.Push(123);
        stack.Push(456);
        var addOperator = new AddOperator();
        var result = addOperator.Evaluate(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, stack.Count);
        Assert.Equal(579, stack.Peek().Value);
    }
}
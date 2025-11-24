using Clac.Core.Operations;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

public class MultiplyOperatorTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var multiplyOperator = new MultiplyOperator();
        Assert.Equal("Multiply", multiplyOperator.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var multiplyOperator = new MultiplyOperator();
        Assert.Equal("Multiplies two numbers", multiplyOperator.Description);
    }

    [Fact]
    public void MinimumStackSize_ShouldBeCorrect()
    {
        var multiplyOperator = new MultiplyOperator();
        Assert.Equal(2, multiplyOperator.MinimumStackSize);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnFalse()
    {
        var multiplyOperator = new MultiplyOperator();
        Assert.False(multiplyOperator.ValidateStackSize(1).IsSuccessful);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnErrorMessage()
    {
        var multiplyOperator = new MultiplyOperator();
        var result = multiplyOperator.ValidateStackSize(1);
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void ValidateStackSize_WithSufficientStackSize_ShouldReturnTrue()
    {
        var multiplyOperator = new MultiplyOperator();
        Assert.True(multiplyOperator.ValidateStackSize(2).IsSuccessful);
    }

    [Fact]
    public void Evaluate_WithInsufficientStackSize_ShouldReturnError()
    {
        var multiplyOperator = new MultiplyOperator();
        var result = multiplyOperator.Evaluate(new Stack());
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public void Evaluate_WithInsufficientStackSize_ShouldReturnErrorMessage()
    {
        var multiplyOperator = new MultiplyOperator();
        var result = multiplyOperator.Evaluate(new Stack());
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void Evaluate_WithInSufficientStackSize_ShouldNotChangeStack()
    {
        var stack = new Stack();
        stack.Push(123);
        var multiplyOperator = new MultiplyOperator();
        var result = multiplyOperator.Evaluate(stack);
        Assert.False(result.IsSuccessful);
        Assert.Equal(1, stack.Count);
        Assert.Equal(123, stack.Peek().Value);
    }

    [Fact]
    public void Evaluate_WithSufficientStackSize_ShouldReturnCorrectResult()
    {
        var stack = new Stack();
        stack.Push(2);
        stack.Push(3);
        var multiplyOperator = new MultiplyOperator();
        var result = multiplyOperator.Evaluate(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(6, result.Value);
    }

    [Fact]
    public void Evaluate_WithSufficientStackSize_ShouldPopTwoNumbersAndPushOneNumber()
    {
        var stack = new Stack();
        stack.Push(2);
        stack.Push(3);
        var multiplyOperator = new MultiplyOperator();
        var result = multiplyOperator.Evaluate(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, stack.Count);
        Assert.Equal(6, stack.Peek().Value);
    }
}
using Clac.Core.Operations;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

public class SubtractOperatorTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var subtractOperator = new SubtractOperator();
        Assert.Equal("Subtract", subtractOperator.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var subtractOperator = new SubtractOperator();
        Assert.Equal("Subtracts two numbers", subtractOperator.Description);
    }

    [Fact]
    public void MinimumStackSize_ShouldBeCorrect()
    {
        var subtractOperator = new SubtractOperator();
        Assert.Equal(2, subtractOperator.MinimumStackSize);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnFalse()
    {
        var subtractOperator = new SubtractOperator();
        Assert.False(subtractOperator.ValidateStackSize(1).IsSuccessful);
    }

    [Fact]
    public void ValidateStackSize_WithInsufficientStackSize_ShouldReturnErrorMessage()
    {
        var subtractOperator = new SubtractOperator();
        var result = subtractOperator.ValidateStackSize(1);
        Assert.False(result.IsSuccessful);
        Assert.Contains(StackHasLessThanTwoNumbers, result.Error.Message);
    }

    [Fact]
    public void ValidateStackSize_WithSufficientStackSize_ShouldReturnTrue()
    {
        var subtractOperator = new SubtractOperator();
        Assert.True(subtractOperator.ValidateStackSize(2).IsSuccessful);
    }

    [Fact]
    public void Evaluate_WithInsufficientStackSize_ShouldReturnError()
    {
        var subtractOperator = new SubtractOperator();
        var result = subtractOperator.Evaluate(new Stack());
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
        var subtractOperator = new SubtractOperator();
        var result = subtractOperator.Evaluate(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, stack.Count);
        Assert.Equal(-333, stack.Peek().Value);
    }
}
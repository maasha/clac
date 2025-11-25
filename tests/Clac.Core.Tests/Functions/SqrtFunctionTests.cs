using Clac.Core.Functions;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Tests.Functions;

public class SqrtFunctionTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var function = new SqrtFunction();
        Assert.Equal("Sqrt", function.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var function = new SqrtFunction();
        Assert.Equal("Calculates the square root of the last number on the stack", function.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var function = new SqrtFunction();
        var stack = new Stack();
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithNegativeNumberOnStack_ShouldReturnError()
    {
        var function = new SqrtFunction();
        var stack = new Stack();
        stack.Push(-1);
        var result = function.Execute(stack);
        Assert.False(result.IsSuccessful);
        Assert.Contains(result.Error.Message, InvalidNegativeSquareRoot);
    }

    [Fact]
    public void Execute_WithPositiveNumberOnStack_ShouldCalculateTheSquareRootOfTheLastNumber()
    {
        var function = new SqrtFunction();
        var stack = new Stack();
        stack.Push(9);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
        Assert.Equal(1, stack.Count);
    }
}
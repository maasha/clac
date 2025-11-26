using Clac.Core.Functions;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Tests.Functions;

public class RecipFunctionTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var function = new RecipFunction();
        Assert.Equal("Recip", function.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var function = new RecipFunction();
        Assert.Equal("Calculates the reciprocal value of the last number on the stack", function.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var function = new RecipFunction();
        var stack = new Stack();
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithZeroOnStack_ShouldReturnError()
    {
        var function = new RecipFunction();
        var stack = new Stack();
        stack.Push(0);
        var result = function.Execute(stack);
        Assert.False(result.IsSuccessful);
        Assert.Contains(DivisionByZero, result.Error.Message);
    }

    [Fact]
    public void Execute_WithNonEmptyStack_ShouldCalculateTheReciprocalValueOfTheLastNumber()
    {
        var function = new RecipFunction();
        var stack = new Stack();
        stack.Push(4);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal([0.25], stack.ToArray());
    }
}
using Clac.Core.Functions;
using Clac.Core.Rpn;

namespace Clac.Core.Tests.Functions;

public class ClearFunctionTests
{
    [Fact]
    public void Name_ShouldBeCorrect()
    {
        var function = new ClearFunction();
        Assert.Equal("Clear", function.Name);
    }

    [Fact]
    public void Description_ShouldBeCorrect()
    {
        var function = new ClearFunction();
        Assert.Equal("Clears the stack", function.Description);
    }

    [Fact]
    public void Execute_WithEmptyStack_ShouldDoNothing()
    {
        var function = new ClearFunction();
        var stack = new Stack();
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void Execute_WithNonEmptyStack_ShouldClearTheStack()
    {
        var function = new ClearFunction();
        var stack = new Stack();
        stack.Push(1);
        stack.Push(2);
        var result = function.Execute(stack);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, stack.Count);
    }
}
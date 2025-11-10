namespace Clac.Core.Tests;

using Xunit;
using Clac.Core;
using Xunit.Sdk;

public class RpnProcessorTests
{
    private readonly RpnProcessor _processor;

    public RpnProcessorTests()
    {
        _processor = new RpnProcessor();
    }

    [Fact]
    public void Process_EmptyTokenList_ShouldNotUpdateStack()
    {
        var tokens = new List<Token>();
        var stackLength = _processor.Stack.Count;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains("No result on stack", result.Error.Message);
        Assert.Equal(stackLength, _processor.Stack.Count);
    }

    [Fact]
    public void Process_NumberToken_ShouldPushOntoStack()
    {
        var tokens = RpnParser.Parse("1 2 3").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, _processor.Stack.Count);
        Assert.Equal([1, 2, 3], _processor.Stack.ToArray());
    }

    [Fact]
    public void Process_OperatorTokenWithLessThanTwoNumbers_ShouldReturnError()
    {
        var tokens = RpnParser.Parse("1 +").Value;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack has less than two numbers", result.Error.Message);
    }

    [Fact]
    public void Process_OperatorTokenWithLessThanTwoNumbers_ShouldPreserveStackState()
    {
        _processor.Process(RpnParser.Parse("5 3 +").Value);
        Assert.Single(_processor.Stack.ToArray());
        Assert.Equal(8, _processor.Stack.Peek().Value);

        var tokens = RpnParser.Parse("+").Value;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack has less than two numbers", result.Error.Message);
        Assert.Single(_processor.Stack.ToArray());
        Assert.Equal(8, _processor.Stack.Peek().Value);
    }


    [Fact]
    public void Process_OperatorToken_ShouldPopTwoNumbersAndPushResult()
    {
        var tokens = RpnParser.Parse("1 2 +").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(3, _processor.Stack.Peek().Value);
    }

    [Fact]
    public void Process_SimpleAddition_ShouldReturnCorrectResult()
    {
        var tokens = RpnParser.Parse("2 3 +").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void Process_ComplexExpression_ShouldReturnCorrectResult()
    {
        var tokens = RpnParser.Parse("5 3 - 2 *").Value;
        var result = _processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(4, result.Value);
    }

    [Fact]
    public void Process_DivisionByZero_ShouldReturnError()
    {
        var tokens = RpnParser.Parse("5 0 /").Value;
        var result = _processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.IsType<DivideByZeroException>(result.Error);
    }

    [Fact]
    public void Process_ConsecutiveCalls_ShouldMaintainStack()
    {
        _processor.Process(RpnParser.Parse("1 2").Value);
        Assert.Equal(2, _processor.Stack.Count);

        var result = _processor.Process(RpnParser.Parse("+").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
    }

    [Fact]
    public void Process_ClearCommand_ShouldClearStack()
    {
        _processor.Process(RpnParser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(RpnParser.Parse("clear()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, _processor.Stack.Count);
    }

    [Fact]
    public void Process_PopCommand_ShouldPopLastElementFromStack()
    {
        _processor.Process(RpnParser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(RpnParser.Parse("pop()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, _processor.Stack.Count);
        Assert.Equal(3, result.Value);
        Assert.Equal([1, 2], _processor.Stack.ToArray());
    }

    [Fact]
    public void Process_SwapCommand_ShouldSwapLastTwoElementsOfStack()
    {
        _processor.Process(RpnParser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(RpnParser.Parse("swap()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, _processor.Stack.Count);
        Assert.Equal([1, 3, 2], _processor.Stack.ToArray());
    }

    [Fact]
    public void Process_SumCommand_ShouldClearStackAndPushSum()
    {
        _processor.Process(RpnParser.Parse("1 2 3").Value);
        Assert.Equal(3, _processor.Stack.Count);
        var result = _processor.Process(RpnParser.Parse("sum()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(6, _processor.Stack.Peek().Value);
        Assert.Equal(6, result.Value);
    }

    [Fact]
    public void Process_SqrtCommand_ShouldCalculateSquareRootOfLastElementAndPushResult()
    {
        _processor.Process(RpnParser.Parse("4").Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(4, _processor.Stack.Peek().Value);
        var result = _processor.Process(RpnParser.Parse("sqrt()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(2, _processor.Stack.Peek().Value);
    }

    [Fact]
    public void Process_SqrtCommand_WithNegativeNumber_ShouldReturnError()
    {
        _processor.Process(RpnParser.Parse("-1").Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(-1, _processor.Stack.Peek().Value);
        var result = _processor.Process(RpnParser.Parse("sqrt()").Value);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Invalid: negative square root", result.Error.Message);
    }

    [Fact]
    public void Process_PowCommand_ShouldCalculatePowerOfLastTwoElementsAndPushResult()
    {
        _processor.Process(RpnParser.Parse("2 3").Value);
        Assert.Equal(2, _processor.Stack.Count);
        Assert.Equal(2, _processor.Stack.ToArray()[0]);
        Assert.Equal(3, _processor.Stack.ToArray()[1]);
        var result = _processor.Process(RpnParser.Parse("pow()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(8, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(8, _processor.Stack.Peek().Value);
    }

    [Fact]
    public void Process_ReciprocalCommand_ShouldCalculateReciprocalOfLastElementAndPushResult()
    {
        _processor.Process(RpnParser.Parse("4").Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(4, _processor.Stack.Peek().Value);
        var result = _processor.Process(RpnParser.Parse("reciprocal()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0.25, result.Value);
        Assert.Equal(1, _processor.Stack.Count);
        Assert.Equal(0.25, _processor.Stack.Peek().Value);
    }
}
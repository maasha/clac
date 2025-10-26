namespace Clac.Core.Tests;

using Xunit;
using Clac.Core;

public class RpnProcessorTests
{
    [Fact]
    public void Process_EmptyTokenList_ShouldNotUpdateStack()
    {
        var processor = new RpnProcessor();
        var tokens = new List<Token>();
        var stackLength = processor.Stack.Count;
        var result = processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains("No result on stack", result.Error.Message);
        Assert.Equal(stackLength, processor.Stack.Count);
    }

    [Fact]
    public void Process_NumberToken_ShouldPushOntoStack()
    {
        var processor = new RpnProcessor();
        var tokens = RpnParser.Parse("1 2 3").Value;
        var result = processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, processor.Stack.Count);
        Assert.Equal([1, 2, 3], processor.Stack.ToArray());
    }

    [Fact]
    public void Process_OperatorTokenWithLessThanTwoNumbers_ShouldReturnError()
    {
        var processor = new RpnProcessor();
        var tokens = RpnParser.Parse("1 +").Value;
        var result = processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.Contains("Stack has less than two numbers", result.Error.Message);
    }


    [Fact]
    public void Process_OperatorToken_ShouldPopTwoNumbersAndPushResult()
    {
        var processor = new RpnProcessor();
        var tokens = RpnParser.Parse("1 2 +").Value;
        var result = processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(1, processor.Stack.Count);
        Assert.Equal(3, processor.Stack.Peek().Value);
    }

    [Fact]
    public void Process_SimpleAddition_ShouldReturnCorrectResult()
    {
        var processor = new RpnProcessor();
        var tokens = RpnParser.Parse("2 3 +").Value;
        var result = processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void Process_ComplexExpression_ShouldReturnCorrectResult()
    {
        var processor = new RpnProcessor();
        var tokens = RpnParser.Parse("5 3 - 2 *").Value; // (5-3)*2 = 4
        var result = processor.Process(tokens);
        Assert.True(result.IsSuccessful);
        Assert.Equal(4, result.Value);
    }

    [Fact]
    public void Process_DivisionByZero_ShouldReturnError()
    {
        var processor = new RpnProcessor();
        var tokens = RpnParser.Parse("5 0 /").Value;
        var result = processor.Process(tokens);
        Assert.False(result.IsSuccessful);
        Assert.IsType<DivideByZeroException>(result.Error);
    }

    [Fact]
    public void Process_ConsecutiveCalls_ShouldMaintainStack()
    {
        var processor = new RpnProcessor();
        processor.Process(RpnParser.Parse("1 2").Value);
        Assert.Equal(2, processor.Stack.Count);

        var result = processor.Process(RpnParser.Parse("+").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, result.Value);
        Assert.Equal(1, processor.Stack.Count);
    }

    [Fact]
    public void Process_ClearCommand_ShouldClearStack()
    {
        var processor = new RpnProcessor();
        processor.Process(RpnParser.Parse("1 2 3").Value);
        Assert.Equal(3, processor.Stack.Count);
        var result = processor.Process(RpnParser.Parse("clear()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, processor.Stack.Count);
    }

    [Fact]
    public void Process_PopCommand_ShouldPopLastElementFromStack()
    {
        var processor = new RpnProcessor();
        processor.Process(RpnParser.Parse("1 2 3").Value);
        Assert.Equal(3, processor.Stack.Count);
        var result = processor.Process(RpnParser.Parse("pop()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, processor.Stack.Count);
        Assert.Equal(3, result.Value);
        Assert.Equal([1, 2], processor.Stack.ToArray());
    }

    [Fact]
    public void Process_SwapCommand_ShouldSwapLastTwoElementsOfStack()
    {
        var processor = new RpnProcessor();
        processor.Process(RpnParser.Parse("1 2 3").Value);
        Assert.Equal(3, processor.Stack.Count);
        var result = processor.Process(RpnParser.Parse("swap()").Value);
        Assert.True(result.IsSuccessful);
        Assert.Equal(3, processor.Stack.Count);
        Assert.Equal([1, 3, 2], processor.Stack.ToArray());
    }
}
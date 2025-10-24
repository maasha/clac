namespace Clac.Tests;

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
        Assert.True(result.IsSuccessful);
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
}
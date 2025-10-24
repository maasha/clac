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
}
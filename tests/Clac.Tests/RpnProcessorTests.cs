namespace Clac.Tests;

using Xunit;
using Clac.Core;

public class RpnProcessorTests
{
    [Fact]
    public void TestDummy()
    {
        Assert.True(true);
    }

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
}
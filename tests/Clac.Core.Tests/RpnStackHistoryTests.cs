using Clac.Core;

public class RpnStackHistoryTests
{
    [Fact]
    public void SaveStackSnapShot_WithEmptyStack_ShouldNotSaveSnapshot()
    {
        var stack = new RpnStack();
        var history = new RpnStackHistory();
        history.SaveStackSnapShot(stack);
        Assert.Equal(0, history.Size);
    }

    [Fact]
    public void SaveStackSnapShot_WithEmptyStack_ShouldReturnError()
    {
        var stack = new RpnStack();
        var history = new RpnStackHistory();
        var result = history.SaveStackSnapShot(stack);
        Assert.False(result.IsSuccessful);
        Assert.Contains(ErrorMessages.StackEmpty, result.Error.Message);
    }

    [Fact]
    public void SaveStackSnapShot_WithNonEmptyStack_ShouldSaveSnapshot()
    {
        var stack = new RpnStack();
        stack.Push(1);
        var history = new RpnStackHistory();
        history.SaveStackSnapShot(stack);
        Assert.Equal(1, history.Size);
    }
}
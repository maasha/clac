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
}
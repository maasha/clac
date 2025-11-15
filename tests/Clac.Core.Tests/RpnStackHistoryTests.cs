using Clac.Core;

public class RpnStackHistoryTests
{
    private readonly RpnStackHistory _history;
    private readonly RpnStack _stack;
    public RpnStackHistoryTests()
    {
        _history = new RpnStackHistory();
        _stack = new RpnStack();
    }

    [Fact]
    public void SaveStackSnapShot_WithEmptyStack_ShouldNotSaveSnapshot()
    {
        _history.SaveStackSnapShot(_stack);
        Assert.Equal(0, _history.Size);
    }

    [Fact]
    public void SaveStackSnapShot_WithEmptyStack_ShouldReturnError()
    {
        var result = _history.SaveStackSnapShot(_stack);
        Assert.False(result.IsSuccessful);
        Assert.Contains(ErrorMessages.HistoryStackIsEmpty, result.Error.Message);
    }

    [Fact]
    public void SaveStackSnapShot_WithNonEmptyStack_ShouldSaveSnapshot()
    {
        _stack.Push(1);
        _history.SaveStackSnapShot(_stack);
        Assert.Equal(1, _history.Size);
    }

    [Fact]
    public void PopStackSnapShot_WithEmptyHistory_ShouldReturnError()
    {
        var result = _history.PopStackSnapShot();
        Assert.False(result.IsSuccessful);
        Assert.Contains(ErrorMessages.HistoryStackIsEmpty, result.Error.Message);
    }

    [Fact]
    public void PopStackSnapShot_WithNonEmptyHistory_ShouldPopSnapshot()
    {
        _stack.Push(1);
        _history.SaveStackSnapShot(_stack);
        var result = _history.PopStackSnapShot();
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, _history.Size);
    }
}
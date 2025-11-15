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
        Assert.Equal(0, _history.Count);
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
        _stack.Push(123);
        _history.SaveStackSnapShot(_stack);
        Assert.Equal(1, _history.Count);
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
        _stack.Push(123);
        _history.SaveStackSnapShot(_stack);
        var result = _history.PopStackSnapShot();
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, _history.Count);
    }

    [Fact]
    public void PopStackSnapShot_WithNonEmptyHistory_ShouldReturnSnapshot()
    {
        _stack.Push(123);
        _history.SaveStackSnapShot(_stack);
        var result = _history.PopStackSnapShot();
        Assert.True(result.IsSuccessful);
        Assert.Equal(_stack.ToArray(), result.Value.ToArray());
    }


    [Fact]
    public void PopStackSnapShot_WithTwoSnapshots_ShouldReturnSecondSnapshot()
    {
        _stack.Push(123);
        _history.SaveStackSnapShot(_stack);
        _stack.Push(456);
        _history.SaveStackSnapShot(_stack);
        var result = _history.PopStackSnapShot();
        Assert.True(result.IsSuccessful);
        Assert.Equal(_stack.ToArray(), result.Value.ToArray());
    }

    [Fact]
    public void PopStackSnapShotTwice_WithTwoSnapshots_ShouldReturnFirstSnapshot()
    {
        _stack.Push(123);
        _history.SaveStackSnapShot(_stack);
        _stack.Push(456);
        _history.SaveStackSnapShot(_stack);
        _history.PopStackSnapShot();
        var result = _history.PopStackSnapShot();
        Assert.True(result.IsSuccessful);
        Assert.Equal([123], result.Value.ToArray());
    }

    [Fact]
    public void SaveStackSnapShot_ShouldCloneStackBeforeSaving()
    {
        _stack.Push(1);
        _stack.Push(2);
        _stack.Push(3);
        _history.SaveStackSnapShot(_stack);

        _stack.Push(4);

        var result = _history.PopStackSnapShot();
        Assert.True(result.IsSuccessful);
        Assert.Equal([1, 2, 3], result.Value.ToArray());
    }
}
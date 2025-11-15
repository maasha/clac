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
    public void Push_WithEmptyStack_ShouldNotSaveSnapshot()
    {
        _history.Push(_stack);
        Assert.Equal(0, _history.Count);
    }

    [Fact]
    public void Push_WithEmptyStack_ShouldReturnError()
    {
        var result = _history.Push(_stack);
        Assert.False(result.IsSuccessful);
        Assert.Contains(ErrorMessages.HistoryStackIsEmpty, result.Error.Message);
    }

    [Fact]
    public void Push_WithNonEmptyStack_ShouldSaveSnapshot()
    {
        _stack.Push(123);
        _history.Push(_stack);
        Assert.Equal(1, _history.Count);
    }

    [Fact]
    public void Pop_WithEmptyHistory_ShouldReturnError()
    {
        var result = _history.Pop();
        Assert.False(result.IsSuccessful);
        Assert.Contains(ErrorMessages.HistoryStackIsEmpty, result.Error.Message);
    }

    [Fact]
    public void Pop_WithNonEmptyHistory_ShouldPopSnapshot()
    {
        _stack.Push(123);
        _history.Push(_stack);
        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal(0, _history.Count);
    }

    [Fact]
    public void Pop_WithNonEmptyHistory_ShouldReturnSnapshot()
    {
        _stack.Push(123);
        _history.Push(_stack);
        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal(_stack.ToArray(), result.Value.ToArray());
    }


    [Fact]
    public void Pop_WithTwoSnapshots_ShouldReturnSecondSnapshot()
    {
        _stack.Push(123);
        _history.Push(_stack);
        _stack.Push(456);
        _history.Push(_stack);
        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal(_stack.ToArray(), result.Value.ToArray());
    }

    [Fact]
    public void PopTwice_WithTwoSnapshots_ShouldReturnFirstSnapshot()
    {
        _stack.Push(123);
        _history.Push(_stack);
        _stack.Push(456);
        _history.Push(_stack);
        _history.Pop();
        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal([123], result.Value.ToArray());
    }

    [Fact]
    public void Push_ShouldCloneStackBeforeSaving()
    {
        _stack.Push(1);
        _stack.Push(2);
        _stack.Push(3);
        _history.Push(_stack);

        _stack.Push(4);

        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal([1, 2, 3], result.Value.ToArray());
    }

    [Fact]
    public void Push_WillNotExceedMaxHistorySize()
    {
        for (int i = 0; i < 101; i++)
        {
            _stack.Push(i);
            _history.Push(_stack);
        }
        Assert.Equal(100, _history.Count);
    }

    [Fact]
    public void Push_WithMaxHistorySize_ShouldRemoveOldestSnapshot()
    {
        const int maxHistorySize = 100;
        const int snapshotsToSave = maxHistorySize + 1;

        for (int i = 0; i < snapshotsToSave; i++)
        {
            _stack.Push(i);
            _history.Push(_stack);
        }

        Assert.Equal(maxHistorySize, _history.Count);

        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal(maxHistorySize, result.Value.ToArray().Last());
    }

    [Fact]
    public void CanUndo_WithNoHistory_ShouldReturnFalse()
    {
        Assert.False(_history.CanUndo());
    }

    [Fact]
    public void CanUndo_WithHistory_ShouldReturnTrue()
    {
        _stack.Push(123);
        _history.Push(_stack);
        Assert.True(_history.CanUndo());
    }
}
using Clac.Core;
using static Clac.Core.ErrorMessages;

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
    public void Pop_WithEmptyHistory_ShouldReturnError()
    {
        var result = _history.Pop();
        Assert.False(result.IsSuccessful);
        Assert.Contains(HistoryStackIsEmpty, result.Error.Message);
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
}
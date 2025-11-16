using Clac.Core;
using static Clac.Core.ErrorMessages;

public class RpnInputHistoryTests
{
    private readonly RpnInputHistory _history;

    public RpnInputHistoryTests()
    {
        _history = new RpnInputHistory();
    }

    [Fact]
    public void Push_WithEmptyInput_ShouldNotAddInputToHistory()
    {
        _history.Push("");
        Assert.Equal(0, _history.Count);
    }

    [Fact]
    public void Pop_WithEmptyHistory_ShouldReturnError()
    {
        var result = _history.Pop();
        Assert.False(result.IsSuccessful);
        Assert.Contains(HistoryInputIsEmpty, result.Error.Message);
    }
}
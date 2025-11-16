using Clac.Core;
using static Clac.Core.ErrorMessages;

// TODO use static error message imports all over.

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
    public void Push_WithNonEmptyInput_ShouldAddInputToHistory()
    {
        _history.Push("1 2 3");
        Assert.Equal(1, _history.Count);
    }

    [Fact]
    public void Pop_WithEmptyHistory_ShouldReturnError()
    {
        var result = _history.Pop();
        Assert.False(result.IsSuccessful);
        Assert.Contains(HistoryInputIsEmpty, result.Error.Message);
    }

    [Fact]
    public void Pop_WithOneInput_ShouldReturnInput()
    {
        _history.Push("1 2 3");
        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal("1 2 3", result.Value);
    }

    [Fact]
    public void Pop_WithTwoInputs_ShouldReturnLastInput()
    {
        _history.Push("1 2 3");
        _history.Push("4 5 6");
        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal("4 5 6", result.Value);
    }

    [Fact]
    public void PopTwice_WithTwoInputs_ShouldReturnFirstInput()
    {
        _history.Push("1 2 3");
        _history.Push("4 5 6");
        _history.Pop();
        var result = _history.Pop();
        Assert.True(result.IsSuccessful);
        Assert.Equal("1 2 3", result.Value);
    }

    [Fact]
    public void Push_WillNotExceedMaxHistorySize()
    {
        for (int i = 0; i < 101; i++)
        {
            _history.Push(i.ToString());
        }
        Assert.Equal(100, _history.Count);
    }
}
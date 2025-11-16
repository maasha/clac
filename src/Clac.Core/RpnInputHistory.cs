using DotNext;
using static Clac.Core.ErrorMessages;

namespace Clac.Core;

public class RpnInputHistory
{
    private readonly History<string> _history;

    public RpnInputHistory()
    {
        _history = new History<string>(validateFunc: input => !string.IsNullOrWhiteSpace(input));
    }

    public int Count => _history.Count;

    public Result<bool> Push(string input)
    {
        return _history.Push(input);
    }

    public Result<string> Pop()
    {
        var result = _history.Pop();
        if (!result.IsSuccessful)
            return new Result<string>(new InvalidOperationException(HistoryInputIsEmpty));
        return result;
    }

    public bool CanUndo => _history.CanUndo;
}
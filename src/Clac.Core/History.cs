using DotNext;
using static Clac.Core.ErrorMessages;

namespace Clac.Core;

public class History<T>
{
    private readonly List<T> _history = [];

    public int Count => _history.Count;

    public Result<bool> Push(T item)
    {
        _history.Add(item);
        return new Result<bool>(true);
    }

    public Result<T> Pop()
    {
        if (_history.Count == 0)
            return new Result<T>(new InvalidOperationException(HistoryIsEmpty));

        var value = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        return new Result<T>(value);
    }
}


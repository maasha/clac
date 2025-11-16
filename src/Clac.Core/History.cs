using DotNext;
using static Clac.Core.ErrorMessages;

namespace Clac.Core;

public class History<T>
{
    private readonly int _maxHistorySize = 100;
    private readonly List<T> _history = [];
    private readonly Func<T, T>? _cloneFunc;

    public History(Func<T, T>? cloneFunc = null)
    {
        _cloneFunc = cloneFunc;
    }

    public int Count => _history.Count;

    public Result<bool> Push(T item)
    {
        var itemToAdd = _cloneFunc != null ? _cloneFunc(item) : item;
        _history.Add(itemToAdd);
        EnforceMaxHistorySize();
        return new Result<bool>(true);
    }

    private void EnforceMaxHistorySize()
    {
        if (_history.Count > _maxHistorySize)
            _history.RemoveAt(0);
    }

    public Result<T> Pop()
    {
        if (_history.Count == 0)
            return new Result<T>(new InvalidOperationException(HistoryIsEmpty));

        var value = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        return new Result<T>(value);
    }

    public bool CanUndo()
    {
        return _history.Count > 0;
    }
}


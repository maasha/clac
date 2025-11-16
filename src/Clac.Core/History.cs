using DotNext;
using static Clac.Core.ErrorMessages;

namespace Clac.Core;

public class History<T>
{
    private readonly int _maxHistorySize = 100;
    private readonly List<T> _history = [];
    private readonly Func<T, T>? _cloneFunc;
    private readonly Func<T, bool>? _validateFunc;

    public History(Func<T, T>? cloneFunc = null, Func<T, bool>? validateFunc = null)
    {
        _cloneFunc = cloneFunc;
        _validateFunc = validateFunc;
    }

    public int Count => _history.Count;

    public Result<bool> Push(T item)
    {
        if (_validateFunc != null && !_validateFunc(item))
            return new Result<bool>(new InvalidOperationException(ValidationFailed));

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


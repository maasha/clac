using DotNext;
using static Clac.Core.ErrorMessages;

namespace Clac.Core;

public class History<T>
{
    private readonly int _maxHistorySize;
    private readonly List<T> _history = [];
    private readonly Func<T, T>? _cloneFunc;
    private readonly Func<T, bool>? _validateFunc;

    public History(Func<T, T>? cloneFunc = null, Func<T, bool>? validateFunc = null, int maxHistorySize = 100)
    {
        _cloneFunc = cloneFunc;
        _validateFunc = validateFunc;
        _maxHistorySize = maxHistorySize;
    }

    public int Count => _history.Count;

    public Result<bool> Push(T item)
    {
        if (!IsValid(item))
            return CreateValidationError();

        var itemToAdd = CloneIfNeeded(item);
        _history.Add(itemToAdd);
        EnforceMaxHistorySize();
        return new Result<bool>(true);
    }

    private bool IsValid(T item)
    {
        return _validateFunc == null || _validateFunc(item);
    }

    private Result<bool> CreateValidationError()
    {
        return new Result<bool>(new InvalidOperationException(ValidationFailed));
    }

    private T CloneIfNeeded(T item)
    {
        return _cloneFunc != null ? _cloneFunc(item) : item;
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

    public bool CanUndo => _history.Count > 0;
}


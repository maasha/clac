using static Clac.Core.ErrorMessages;
using DotNext;

public class RpnInputHistory
{
    private readonly List<string> _history = [];
    public int Count => _history.Count;

    private readonly int _maxHistorySize = 100;

    public Result<bool> Push(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new Result<bool>(false);

        _history.Add(input);
        EnforceMaxHistorySize();
        return new Result<bool>(true);
    }

    public Result<string> Pop()
    {
        if (_history.Count == 0)
            return new Result<string>(new InvalidOperationException(HistoryInputIsEmpty));
        var value = _history[^1];
        _history.RemoveAt(_history.Count - 1);
        return new Result<string>(value);
    }
    private void EnforceMaxHistorySize()
    {
        if (_history.Count > _maxHistorySize)
            _history.RemoveAt(0);
    }
}
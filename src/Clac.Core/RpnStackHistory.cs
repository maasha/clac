using DotNext;
namespace Clac.Core;

public class RpnStackHistory
{
    private readonly int _maxHistorySize = 100;
    private readonly List<RpnStack> _historyStack = [];
    public int Count => _historyStack.Count;
    public Result<bool> SaveStackSnapShot(RpnStack stack)
    {
        if (stack.Count == 0)
            return new Result<bool>(new InvalidOperationException(ErrorMessages.HistoryStackIsEmpty));
        AddClonedStackToHistory(stack);
        EnforceMaxHistorySize();
        return new Result<bool>(true);
    }

    public Result<RpnStack> PopStackSnapShot()
    {
        if (_historyStack.Count == 0)
            return new Result<RpnStack>(new InvalidOperationException(ErrorMessages.HistoryStackIsEmpty));
        var value = _historyStack[^1];
        _historyStack.RemoveAt(_historyStack.Count - 1);
        return new Result<RpnStack>(value);
    }

    public bool CanUndo()
    {
        return _historyStack.Count > 0;
    }

    private void EnforceMaxHistorySize()
    {
        if (_historyStack.Count > _maxHistorySize)
            _historyStack.RemoveAt(0);
    }

    private void AddClonedStackToHistory(RpnStack stack)
    {
        var clonedStack = CloneStack(stack);
        _historyStack.Add(clonedStack);
    }

    private static RpnStack CloneStack(RpnStack stack)
    {
        var clonedStack = new RpnStack();
        foreach (var value in stack.ToArray())
            clonedStack.Push(value);
        return clonedStack;
    }
}

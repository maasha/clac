using DotNext;
using static Clac.Core.ErrorMessages;
namespace Clac.Core;

public class RpnStackHistory
{
    private readonly History<RpnStack> _history;

    public RpnStackHistory()
    {
        _history = new History<RpnStack>(cloneFunc: CloneStack);
    }

    public int Count => _history.Count;

    public Result<bool> Push(RpnStack stack)
    {
        return _history.Push(stack);
    }

    public Result<RpnStack> Pop()
    {
        var result = _history.Pop();
        if (!result.IsSuccessful)
            return new Result<RpnStack>(new InvalidOperationException(HistoryStackIsEmpty));
        return result;
    }

    public bool CanUndo()
    {
        return _history.CanUndo;
    }

    private static RpnStack CloneStack(RpnStack stack)
    {
        var clonedStack = new RpnStack();
        foreach (var value in stack.ToArray())
            clonedStack.Push(value);
        return clonedStack;
    }
}

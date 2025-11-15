using DotNext;
namespace Clac.Core;

public class RpnStackHistory
{
    private readonly List<RpnStack> _historyStack = [];
    public int Count => _historyStack.Count;
    public Result<bool> SaveStackSnapShot(RpnStack stack)
    {
        if (stack.Count == 0)
            return new Result<bool>(new InvalidOperationException(ErrorMessages.HistoryStackIsEmpty));
        var clonedStack = new RpnStack();
        foreach (var value in stack.ToArray())
        {
            clonedStack.Push(value);
        }
        _historyStack.Add(clonedStack);
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
}

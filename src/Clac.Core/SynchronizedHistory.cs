using DotNext;

namespace Clac.Core;

public class SynchronizedHistory
{
    private readonly History<RpnStack> _stackHistory;
    private readonly History<string> _inputHistory;

    public SynchronizedHistory()
    {
        _stackHistory = new History<RpnStack>(cloneFunc: CloneStack);
        _inputHistory = new History<string>(validateFunc: input => !string.IsNullOrWhiteSpace(input));
    }

    public Result<bool> Push(RpnStack stack, string input)
    {
        var stackResult = _stackHistory.Push(stack);
        if (!stackResult.IsSuccessful)
            return stackResult;

        var inputResult = _inputHistory.Push(input);
        if (!inputResult.IsSuccessful)
        {
            _stackHistory.Pop();
            return inputResult;
        }

        return new Result<bool>(true);
    }

    public Result<(RpnStack stack, string input)> Pop()
    {
        var stackResult = _stackHistory.Pop();
        if (!stackResult.IsSuccessful)
            return new Result<(RpnStack, string)>(stackResult.Error);

        var inputResult = _inputHistory.Pop();
        if (!inputResult.IsSuccessful)
        {
            _stackHistory.Push(stackResult.Value);
            return new Result<(RpnStack, string)>(inputResult.Error);
        }

        return new Result<(RpnStack, string)>((stackResult.Value, inputResult.Value));
    }

    public bool CanUndo => _stackHistory.CanUndo;

    private static RpnStack CloneStack(RpnStack stack)
    {
        var clonedStack = new RpnStack();
        foreach (var value in stack.ToArray())
            clonedStack.Push(value);
        return clonedStack;
    }
}

using DotNext;
using Clac.Core.Rpn;

namespace Clac.Core.History;

public class StackAndInputHistory
{
    public bool CanUndo => _stackHistory.CanUndo;
    public bool IsEmpty => _stackHistory.Count == 0;
    public History<Stack> StackHistory => _stackHistory;
    public History<string> InputHistory => _inputHistory;

    private readonly History<Stack> _stackHistory;
    private readonly History<string> _inputHistory;

    public StackAndInputHistory()
    {
        _stackHistory = new History<Stack>(cloneFunc: CloneStack);
        _inputHistory = new History<string>(validateFunc: input => !string.IsNullOrWhiteSpace(input));
    }

    public Result<bool> Push(Stack stack, string input)
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

    public Result<(Stack stack, string input)> Pop()
    {
        var stackResult = _stackHistory.Pop();
        if (!stackResult.IsSuccessful)
            return new Result<(Stack, string)>(stackResult.Error);

        var inputResult = _inputHistory.Pop();
        if (!inputResult.IsSuccessful)
        {
            _stackHistory.Push(stackResult.Value);
            return new Result<(Stack, string)>(inputResult.Error);
        }

        return new Result<(Stack, string)>((stackResult.Value, inputResult.Value));
    }

    private static Stack CloneStack(Stack stack)
    {
        var clonedStack = new Stack();
        foreach (var value in stack.ToArray())
            clonedStack.Push(value);
        return clonedStack;
    }
}


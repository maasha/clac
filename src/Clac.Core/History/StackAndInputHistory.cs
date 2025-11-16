using DotNext;
using Clac.Core.Rpn;

namespace Clac.Core.History;

public class StackAndInputHistory
{
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

    public bool CanUndo => _stackHistory.CanUndo;

    private static Stack CloneStack(Stack stack)
    {
        var clonedStack = new Stack();
        foreach (var value in stack.ToArray())
            clonedStack.Push(value);
        return clonedStack;
    }
}


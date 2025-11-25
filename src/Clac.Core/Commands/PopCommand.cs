using Clac.Core.Rpn;
using DotNext;

namespace Clac.Core.Commands;

public class PopCommand : IStackCommand
{
    public string Symbol => "pop()";

    public string Name => "Pop";

    public string Description => "Removes the last number from the stack";

    public Result<double> Execute(Stack stack)
    {
        var result = stack.Pop();
        if (!result.IsSuccessful)
            return new Result<double>(0); /// Ignore error.
        return result.Value;
    }
}
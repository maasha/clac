using Clac.Core.Rpn;
using DotNext;

namespace Clac.Core.Functions;

public class PopFunction : IFunction
{
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
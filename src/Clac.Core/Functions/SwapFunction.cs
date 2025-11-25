using Clac.Core.Rpn;
using DotNext;

namespace Clac.Core.Functions;

public class SwapFunction : IFunction
{
    public string Name => "Swap";

    public string Description => "Swaps the last two numbers on the stack";

    public Result<double> Execute(Stack stack)
    {
        var result = stack.Swap();
        if (!result.IsSuccessful)
            return new Result<double>(0); /// Ignore error.
        return result.Value;
    }
}
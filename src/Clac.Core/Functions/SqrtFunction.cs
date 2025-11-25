using Clac.Core.Rpn;
using DotNext;

namespace Clac.Core.Functions;

public class SqrtFunction : IFunction
{
    public string Name => "Sqrt";

    public string Description => "Calculates the square root of the last number on the stack";

    public Result<double> Execute(Stack stack)
    {
        var result = stack.Sqrt();
        if (!result.IsSuccessful)
            return new Result<double>(0); /// Ignore error.
        return result.Value;
    }
}
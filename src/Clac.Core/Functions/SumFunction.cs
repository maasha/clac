using Clac.Core.Rpn;
using DotNext;

namespace Clac.Core.Functions;

public class SumFunction : IFunction
{
    public string Name => "Sum";

    public string Description => "Sums all the numbers on the stack";

    public Result<double> Execute(Stack stack)
    {
        var sum = stack.Sum();
        if (!sum.IsSuccessful)
            return new Result<double>(0); /// Ignore error.
        return sum.Value;
    }
}
using Clac.Core.Rpn;
using DotNext;
using static Clac.Core.ErrorMessages;
namespace Clac.Core.Functions;

public class PowFunction : IFunction
{
    public string Name => "Pow";

    public string Description => "Calculates the power of the last two numbers on the stack";

    public Result<double> Execute(Stack stack)
    {
        var result = stack.Pow();
        if (!result.IsSuccessful)
            return new Result<double>(0); /// Ignore error.
        return result.Value;
    }
}
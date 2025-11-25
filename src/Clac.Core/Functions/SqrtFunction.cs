using Clac.Core.Rpn;
using DotNext;
using static Clac.Core.ErrorMessages;
namespace Clac.Core.Functions;

public class SqrtFunction : IFunction
{
    public string Name => "Sqrt";

    public string Description => "Calculates the square root of the last number on the stack";

    public Result<double> Execute(Stack stack)
    {
        var result = stack.Sqrt();
        if (!result.IsSuccessful)
        {
            if (result.Error is InvalidOperationException && result.Error.Message == InvalidNegativeSquareRoot)
                return result;
            else
                return new Result<double>(0); /// Ignore error.
        }
        return result.Value;
    }
}
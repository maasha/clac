using Clac.Core.Rpn;
using DotNext;
using static Clac.Core.ErrorMessages;
namespace Clac.Core.Functions;

public class RecipFunction : IFunction
{
    public string Name => "Recip";

    public string Description => "Calculates the reciprocal value of the last number on the stack";

    public Result<double> Execute(Stack stack)
    {
        var result = stack.Recip();
        if (!result.IsSuccessful)
        {
            if (result.Error is InvalidOperationException && result.Error.Message == DivisionByZero)
                return result;
            else
                return new Result<double>(0); /// Ignore error.
        }
        return result.Value;
    }
}
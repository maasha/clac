using DotNext;
using Clac.Core.Enums;

namespace Clac.Core;

public class RpnEvaluator
{
    public static Result<double> Evaluate(double number1, double number2, OperatorSymbol op)
    {
        return op switch
        {
            OperatorSymbol.Add => number1 + number2,
            OperatorSymbol.Subtract => number1 - number2,
            OperatorSymbol.Multiply => number1 * number2,
            OperatorSymbol.Divide when number2 != 0 => number1 / number2,
            OperatorSymbol.Divide => new Result<double>(new DivideByZeroException(ErrorMessages.DivisionByZero)),
            _ => new Result<double>(new InvalidOperationException(ErrorMessages.UnknownOperator(op))),
        };
    }
}
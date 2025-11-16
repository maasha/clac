using DotNext;
using Clac.Core.Enums;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Rpn;

public class Evaluator
{
    public static Result<double> Evaluate(double number1, double number2, OperatorSymbol op)
    {
        return op switch
        {
            OperatorSymbol.Add => number1 + number2,
            OperatorSymbol.Subtract => number1 - number2,
            OperatorSymbol.Multiply => number1 * number2,
            OperatorSymbol.Divide when number2 != 0 => number1 / number2,
            OperatorSymbol.Divide => new Result<double>(new DivideByZeroException(DivisionByZero)),
            _ => new Result<double>(new InvalidOperationException(UnknownOperator(op))),
        };
    }
}
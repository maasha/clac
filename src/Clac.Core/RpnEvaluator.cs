namespace Clac.Core;

using DotNext;

/// <summary>
/// Class for evaluating reverse-Polish notation expressions.
/// </summary>
public class RpnEvaluator
{
    /// <summary>
    /// Evaluates a reverse-Polish notation expression consisting of two numbers and an operator.
    /// </summary>
    /// <param name="number1">The first operand.</param>
    /// <param name="number2">The second operand.</param>
    /// <param name="op">The operator symbol (Add, Subtract, Multiply, Divide).</param> operator, e.g., "+", "-", "*", "/".</param>
    /// <returns>A Result containing the evaluation result or an error if the operator is invalid.</returns>
    public static Result<double> Evaluate(double number1, double number2, OperatorSymbol op)
    {
        return op switch
        {
            OperatorSymbol.Add => number1 + number2,
            OperatorSymbol.Subtract => number1 - number2,
            OperatorSymbol.Multiply => number1 * number2,
            OperatorSymbol.Divide when number2 != 0 => number1 / number2,
            OperatorSymbol.Divide => new Result<double>(new DivideByZeroException("Division by zero")),
            _ => new Result<double>(new InvalidOperationException($"Unknown operator: {op}")),
        };
    }
}
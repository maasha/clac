namespace Clac.Core;

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
    /// <param name="operatorSymbol">The operator, e.g., "+", "-", "*", "/".</param>
    /// <returns>The result of the evaluation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the operator is invalid.</exception>
    public double Evaluate(double number1, double number2, OperatorSymbol op)
    {
        switch (op)
        {
            case OperatorSymbol.Add:
                return number1 + number2;
            case OperatorSymbol.Subtract:
                return number1 - number2;
            case OperatorSymbol.Multiply:
                return number1 * number2;
            case OperatorSymbol.Divide:
                return number1 / number2;
            default:
                throw new InvalidOperationException($"Invalid operator: {op}");
        }
    }
}
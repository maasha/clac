using DotNext;
using Clac.Core.Enums;

namespace Clac.Core;

public static class Operator
{
    /// <summary>
    /// Gets the OperatorSymbol for the given string symbol.
    /// </summary>
    /// <param name="symbol">The string representation of the operator.</param>
    /// <returns>A Result containing the OperatorSymbol if valid, or an error if invalid.</returns>
    public static Result<OperatorSymbol> GetOperatorSymbol(string symbol)
    {
        return symbol switch
        {
            "+" => new Result<OperatorSymbol>(OperatorSymbol.Add),
            "-" => new Result<OperatorSymbol>(OperatorSymbol.Subtract),
            "*" => new Result<OperatorSymbol>(OperatorSymbol.Multiply),
            "/" => new Result<OperatorSymbol>(OperatorSymbol.Divide),
            _ => new Result<OperatorSymbol>(new InvalidOperationException($"Invalid operator symbol: '{symbol}'"))
        };
    }

    /// <summary>
    /// Checks if the given symbol is a valid operator symbol.
    /// </summary>
    /// <param name="symbol">The symbol to check.</param>
    /// <returns>True if the symbol is a valid operator symbol, false otherwise.</returns>
    public static bool IsValidOperator(string symbol)
    {
        return symbol switch
        {
            "+" => true,
            "-" => true,
            "*" => true,
            "/" => true,
            _ => false
        };
    }
}


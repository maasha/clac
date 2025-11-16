using DotNext;
using Clac.Core.Enums;

namespace Clac.Core.Operations;

public static class Operator
{
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


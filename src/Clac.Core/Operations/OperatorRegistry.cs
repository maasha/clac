using Clac.Core.Enums;
using DotNext;

namespace Clac.Core.Operations;

public class OperatorRegistry
{
    private readonly Dictionary<OperatorSymbol, IOperator> _operators = [];

    public void Register(IOperator op)
    {
        _operators[op.Symbol] = op;
    }

    public Result<IOperator> GetOperator(OperatorSymbol symbol)
    {
        if (!_operators.TryGetValue(symbol, out var op))
            return new Result<IOperator>(new InvalidOperationException($"Operator '{symbol}' not found"));
        return new Result<IOperator>(op);
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
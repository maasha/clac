using Clac.Core.Enums;
using DotNext;

namespace Clac.Core.Operations;

public class OperatorRegistry
{
    private readonly Dictionary<string, IOperator> _operators = [];

    public void Register(IOperator op)
    {
        _operators[op.Symbol] = op;
    }

    public Result<IOperator> GetOperator(string symbol)
    {
        if (!_operators.TryGetValue(symbol, out var op))
            return new Result<IOperator>(new InvalidOperationException($"Operator '{symbol}' not found"));
        return new Result<IOperator>(op);
    }

    public Result<bool> IsValidOperator(string symbol)
    {
        if (!_operators.ContainsKey(symbol))
            return new Result<bool>(new InvalidOperationException($"Operator '{symbol}' not found"));
        return new Result<bool>(true);
    }
}
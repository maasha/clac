using DotNext;
using Clac.Core.Rpn;
using Clac.Core.Enums;

namespace Clac.Core.Operations;

public interface IOperator
{
    OperatorSymbol Symbol { get; }
    string Name { get; }
    string Description { get; }
    int MinimumStackSize { get; }

    Result<bool> ValidateStackSize(int stackSize);
    Result<double> Evaluate(Stack stack);
}
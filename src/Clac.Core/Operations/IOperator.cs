using DotNext;
using Clac.Core.Rpn;

namespace Clac.Core.Operations;

public interface IOperator
{
    string Name { get; }
    string Description { get; }
    int MinimumStackSize { get; }

    Result<bool> ValidateStackSize(int stackSize);
    Result<double> Evaluate(Stack stack);
}
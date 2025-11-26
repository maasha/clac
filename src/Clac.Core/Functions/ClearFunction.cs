using Clac.Core.Rpn;
using DotNext;

namespace Clac.Core.Functions;

public class ClearFunction : IFunction
{
    public string Name => "Clear";

    public string Description => "Clears the stack";

    public Result<double> Execute(Stack stack)
    {
        stack.Clear();
        return new Result<double>(0);
    }
}
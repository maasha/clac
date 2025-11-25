using DotNext;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Operations;

public class AddOperator : IOperator
{
    public string Symbol => "+";
    public string Name => "Add";
    public string Description => "Adds two numbers";
    public int MinimumStackSize => 2;

    public Result<bool> ValidateStackSize(int stackSize)
    {
        if (stackSize < MinimumStackSize)
            return new Result<bool>(new InvalidOperationException(StackHasLessThanTwoNumbers));
        return new Result<bool>(true);
    }

    public Result<double> Evaluate(Stack stack)
    {
        if (!ValidateStackSize(stack.Count).IsSuccessful)
            return new Result<double>(new InvalidOperationException(StackHasLessThanTwoNumbers));

        double result = EvaluateStack(stack);

        stack.Push(result);
        return result;
    }

    private static double EvaluateStack(Stack stack)
    {
        var number1 = stack.Pop();
        var number2 = stack.Pop();
        var result = number1.Value + number2.Value;
        return result;
    }
}
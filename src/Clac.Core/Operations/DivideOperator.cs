using Clac.Core.Rpn;
using DotNext;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Operations;

public class DivideOperator : IOperator
{
    public string Symbol => "/";
    public string Name => "Divide";
    public string Description => "Divides two numbers";
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

        var result = EvaluateStack(stack);

        if (!result.IsSuccessful)
            return result;

        stack.Push(result.Value);
        return result;
    }

    private static Result<double> EvaluateStack(Stack stack)
    {
        var number1 = stack.Pop();
        var number2 = stack.Pop();
        if (number1.Value == 0)
        {
            stack.Push(number2.Value);
            stack.Push(number1.Value);
            return new Result<double>(new DivideByZeroException(DivisionByZero));
        }
        var result = number2.Value / number1.Value;
        return new Result<double>(result);
    }
}
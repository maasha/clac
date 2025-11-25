using DotNext;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Rpn;

public class Stack
{
    private readonly List<double> _stack = [];

    public double[] ToArray() => [.. _stack];
    public int Count => _stack.Count;

    public Result<double> Peek()
    {
        if (_stack.Count == 0)
            return new Result<double>(new InvalidOperationException(StackEmpty));

        return new Result<double>(_stack[^1]);
    }

    public void Push(double value)
    {
        _stack.Add(value);
    }

    public void Clear()
    {
        _stack.Clear();
    }

    public Result<double> Pop()
    {
        if (_stack.Count == 0)
            return new Result<double>(new InvalidOperationException(StackEmpty));

        var value = _stack[^1];
        _stack.RemoveAt(_stack.Count - 1);
        return new Result<double>(value);
    }

    public Result<double> Swap()
    {
        if (_stack.Count < 2)
            return new Result<double>(new InvalidOperationException(StackHasLessThanTwoNumbers));

        var last = _stack[^1];
        var secondLast = _stack[^2];
        _stack[^1] = secondLast;
        _stack[^2] = last;
        return new Result<double>(secondLast);
    }

    public Result<double> Sum()
    {
        if (_stack.Count == 0)
            return new Result<double>(new InvalidOperationException(StackEmpty));

        var sum = _stack.Sum();
        _stack.Clear();
        Push(sum);
        return new Result<double>(sum);
    }

    public Result<double> Sqrt()
    {
        if (_stack.Count == 0)
            return new Result<double>(new InvalidOperationException(StackEmpty));

        var result = Pop();
        if (!result.IsSuccessful)
            return new Result<double>(result.Error);

        if (result.Value < 0)
            return new Result<double>(new InvalidOperationException(InvalidNegativeSquareRoot));

        var sqrt = Math.Sqrt(result.Value);
        Push(sqrt);
        return new Result<double>(sqrt);
    }

    public Result<double> Pow()
    {
        if (_stack.Count < 2)
            return new Result<double>(new InvalidOperationException(StackHasLessThanTwoNumbers));

        var result = PopTwo();
        if (!result.IsSuccessful)
            return new Result<double>(result.Error);

        var (exponent, baseValue) = result.Value;
        var pow = Math.Pow(baseValue, exponent);
        Push(pow);
        return new Result<double>(pow);
    }

    public Result<double> Reciprocal()
    {
        if (_stack.Count == 0)
            return new Result<double>(new InvalidOperationException(StackEmpty));

        if (_stack[^1] == 0)
            return new Result<double>(new InvalidOperationException(DivisionByZero));

        return new Result<double>(1.0 / _stack[^1]);
    }

    private Result<(double first, double second)> PopTwo()
    {
        var first = Pop();
        if (!first.IsSuccessful)
            return new Result<(double, double)>(first.Error);

        var second = Pop();
        if (!second.IsSuccessful)
            return new Result<(double, double)>(second.Error);

        return new Result<(double, double)>((first.Value, second.Value));
    }
}

using DotNext;

namespace Clac.Core;

public class RpnStack
{
    private readonly List<double> _stack = [];

    public double[] ToArray() => [.. _stack];

    public Result<double> Peek()
    {
        if (_stack.Count == 0)
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

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
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

        var value = _stack[^1];
        _stack.RemoveAt(_stack.Count - 1);
        return new Result<double>(value);
    }

    public Result<double> Swap()
    {
        if (_stack.Count < 2)
        {
            return new Result<double>(new InvalidOperationException("Stack has less than two elements"));
        }

        var last = _stack[^1];
        var secondLast = _stack[^2];
        _stack[^1] = secondLast;
        _stack[^2] = last;
        return new Result<double>(secondLast);
    }

    public Result<double> Sum()
    {
        if (_stack.Count == 0)
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

        return new Result<double>(_stack.Sum());
    }

    public Result<double> Sqrt()
    {
        if (_stack.Count == 0)
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

        if (_stack[^1] < 0)
        {
            return new Result<double>(new InvalidOperationException("Invalid: negative square root"));
        }

        return new Result<double>(Math.Sqrt(_stack[^1]));
    }

    public Result<double> Pow()
    {
        if (_stack.Count < 2)
        {
            return new Result<double>(new InvalidOperationException("Stack has less than two elements"));
        }

        var exponent = _stack[^1];
        var baseValue = _stack[^2];
        return new Result<double>(Math.Pow(baseValue, exponent));
    }

    public Result<double> Reciprocal()
    {
        if (_stack.Count == 0)
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

        if (_stack[^1] == 0)
        {
            return new Result<double>(new InvalidOperationException("Division by zero"));
        }

        return new Result<double>(1.0 / _stack[^1]);
    }

    public int Count => _stack.Count;
}

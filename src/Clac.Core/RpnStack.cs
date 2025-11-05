
using DotNext;

namespace Clac.Core;

/// <summary>
/// Class for maintaining the stack in the calculator.
/// 
/// The stack is a last-in, first-out (LIFO) data structure that only contains
/// numbers.
/// </summary>
public class RpnStack
{
    /// <summary>
    /// The stack of numbers.
    /// </summary>
    private readonly List<double> _stack = [];

    /// <summary>
    /// Returns the stack as an array.
    /// </summary>
    /// <returns>The stack as an array.</returns>
    public double[] ToArray() => [.. _stack];

    /// <summary>
    /// Returns the last element on the stack without removing it.
    /// </summary>
    /// <returns>The last element on the stack.</returns>
    /// <remarks>Returns a failed result with an error if the stack is empty.</remarks>
    public Result<double> Peek()
    {
        if (_stack.Count == 0)
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

        return new Result<double>(_stack[^1]);
    }

    /// <summary>
    /// Pushes a number onto the stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    public void Push(double value)
    {
        _stack.Add(value);
    }

    /// <summary> 
    /// Clears the stack.
    /// </summary>
    public void Clear()
    {
        _stack.Clear();
    }

    /// <summary>
    /// Removes and returns the last element from the stack.
    /// </summary>
    /// <returns>The last element on the stack.</returns>
    /// <remarks>Returns a failed result with an error if the stack is empty.</remarks>
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

    /// <summary>
    /// Swaps the last two elements of the stack.
    /// </summary>
    /// <returns>The the new last element on the stack.</returns>
    /// <remarks>Returns a failed result with an error if the stack has less than two elements.</remarks>
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

    /// <summary>
    /// Sums all the elements on the stack.
    /// </summary>
    /// <returns>The sum of the elements on the stack.</returns>
    /// <remarks>Returns a failed result with an error if the stack is empty.</remarks>
    public Result<double> Sum()
    {
        if (_stack.Count == 0)
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

        return new Result<double>(_stack.Sum());
    }

    /// <summary>
    /// Calculates the square root of the last element on the stack.
    /// </summary>
    /// <returns>The square root of the last element on the stack.</returns>
    /// <remarks>Returns a failed result with an error if the stack is empty.</remarks>
    public Result<double> Sqrt()
    {
        if (_stack.Count == 0)
        {
            return new Result<double>(new InvalidOperationException("Stack is empty"));
        }

        if (_stack[^1] < 0)
        {
            return new Result<double>(new InvalidOperationException("Square root of a negative number is not supported"));
        }

        return new Result<double>(Math.Sqrt(_stack[^1]));
    }

    /// <summary>
    /// Returns the number of elements on the stack.
    /// </summary>
    /// <returns>The number of elements on the stack.</returns>
    public int Count => _stack.Count;
}
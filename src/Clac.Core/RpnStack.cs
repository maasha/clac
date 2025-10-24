
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
    /// Returns the last element on the stack without removing it.
    /// </summary>
    /// <returns>The last element on the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
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
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
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
    /// Returns the number of elements on the stack.
    /// </summary>
    /// <returns>The number of elements on the stack.</returns>
    public int Count => _stack.Count;
}
using DotNext;

namespace Clac.Core;

/// <summary>
/// Class for processing the parsed input, performing evaluations and updating
/// the stack. The processor maintains state and that multiple calls to Process
/// accumulate values on the stack.
/// </summary>
public class RpnProcessor
{
    private readonly RpnStack _stack = new();

    /// <summary>
    /// Gets a clone of the RPN stack.
    /// </summary>
    public RpnStack Stack => CloneStack();


    /// <summary>
    /// Processes the list of tokens, performing evaluations and updating the stack.
    /// 
    /// For each token, the processor will:
    /// - If the token is a number, push it onto the stack.
    /// - If the token is an operator, pop the last two numbers from the stack,
    ///   perform the evaluation, and push the result back onto the stack.
    /// - If the token is a command, run that command and return success with 0
    ///   as the result. The command is executed and the stack is cleared.
    /// </summary>
    /// <param name="tokens">The list of tokens to process.</param>
    /// <returns>The result of the evaluation.</returns>
    /// <remarks>Returns a failed result with an error if pop fails.</remarks>
    public Result<double> Process(List<Token> tokens)
    {
        bool commandExecuted = false;
        double commandResult = 0;

        foreach (var token in tokens)
        {
            if (token is Token.NumberToken numberToken)
            {
                _stack.Push(numberToken.Value);
            }
            else if (token is Token.OperatorToken operatorToken)
            {
                var numberToken1 = _stack.Pop();
                var numberToken2 = _stack.Pop();

                if (!numberToken1.IsSuccessful || !numberToken2.IsSuccessful)
                {
                    return new Result<double>(new InvalidOperationException("Stack has less than two numbers"));
                }

                var result = RpnEvaluator.Evaluate(numberToken2.Value, numberToken1.Value, operatorToken.Symbol);

                if (!result.IsSuccessful)
                {
                    return result;
                }

                _stack.Push(result.Value);
            }
            else if (token is Token.CommandToken commandToken)
            {
                if (commandToken.Command == "clear")
                {
                    _stack.Clear();
                    commandExecuted = true;
                }
                else if (commandToken.Command == "pop")
                {
                    var result = _stack.Pop();
                    if (!result.IsSuccessful)
                    {
                        return result;
                    }
                    commandResult = result.Value;
                    commandExecuted = true;
                }
            }
        }

        // If a command was executed, return success with the command result
        if (commandExecuted)
        {
            return new Result<double>(commandResult);
        }

        var finalResult = _stack.Peek();
        return finalResult.IsSuccessful
            ? finalResult
            : new Result<double>(new InvalidOperationException("No result on stack"));
    }

    /// <summary>
    /// Clones the RPN stack to prevent external modification of the stack.
    /// </summary>
    /// <returns>A clone of the RPN stack.</returns>
    private RpnStack CloneStack()
    {
        var copy = new RpnStack();

        foreach (var value in _stack.ToArray())
        {
            copy.Push(value);
        }

        return copy;
    }
}
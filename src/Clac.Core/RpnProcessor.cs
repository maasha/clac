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
    ///   as the result. The command is executed and the input is cleared.
    /// </summary>
    /// <param name="tokens">The list of tokens to process.</param>
    /// <returns>The result of the evaluation.</returns>
    /// <remarks>Returns a failed result with an error if evaluation fails.</remarks>
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
                var result = ProcessOperator(operatorToken);
                if (!result.IsSuccessful)
                {
                    return result;
                }
            }
            else if (token is Token.CommandToken commandToken)
            {
                var commandProcessResult = ProcessCommand(commandToken.Command);
                if (commandProcessResult.HasValue)
                {
                    if (!commandProcessResult.Value.IsSuccessful)
                    {
                        return commandProcessResult.Value;
                    }
                    commandExecuted = true;
                    commandResult = commandProcessResult.Value.Value;
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
    /// Processes a command token and returns the result if the command was recognized.
    /// </summary>
    /// <param name="command">The command to process.</param>
    /// <returns>The result of the command execution, or null if the command is not recognized.</returns>
    /// <remarks>Returns a failed result with an error if the command execution fails critically.</remarks>
    private Result<double>? ProcessCommand(string command)
    {
        if (command == "clear")
        {
            _stack.Clear();
            return new Result<double>(0);
        }
        else if (command == "pop")
        {
            var result = _stack.Pop();
            return result.IsSuccessful ? result : new Result<double>(0);
        }
        else if (command == "swap")
        {
            var result = _stack.Swap();
            return result.IsSuccessful ? result : new Result<double>(0);
        }
        else if (command == "sum")
        {
            var result = _stack.Sum();
            if (result.IsSuccessful)
            {
                _stack.Clear();
                _stack.Push(result.Value);
            }
            return result.IsSuccessful ? result : new Result<double>(0);
        }
        else if (command == "sqrt")
        {
            var result = _stack.Sqrt();

            if (!result.IsSuccessful)
            {
                if (result.Error.Message.Contains("Stack is empty"))
                {
                    return new Result<double>(0);
                }
                else
                {
                    return result;
                }
            }
            else
            {
                _stack.Pop();
                _stack.Push(result.Value);
                return result;
            }
        }
        else if (command == "pow")
        {
            var result = _stack.Pow();

            if (!result.IsSuccessful)
            {
                if (result.Error.Message.Contains("Stack is empty") || result.Error.Message.Contains("Stack has less than two elements"))
                {
                    return new Result<double>(0);
                }
                else
                {
                    return result;
                }
            }
            else
            {
                _stack.Pop();
                _stack.Pop();
                _stack.Push(result.Value);
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Processes an operator token by popping two values, evaluating, and pushing the result.
    /// </summary>
    /// <param name="operatorToken">The operator token to process.</param>
    /// <returns>The result of the evaluation.</returns>
    /// <remarks>Returns a failed result with an error if the stack has less than two numbers.</remarks>
    /// <remarks>Returns a failed result with an error if the evaluation fails.</remarks>
    /// <remarks>Pushes the result back onto the stack.</remarks>
    private Result<double> ProcessOperator(Token.OperatorToken operatorToken)
    {
        if (_stack.Count < 2)
        {
            return new Result<double>(new InvalidOperationException("Stack has less than two numbers"));
        }

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
        return result;
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
using DotNext;

namespace Clac.Core;

/// <summary>
/// Class for processing the parsed input, performing evaluations and updating
/// the stack.
/// </summary>
public class RpnProcessor
{
    private readonly RpnStack _stack = new();

    /// <summary>
    /// Gets the RPN stack.
    /// </summary>
    public RpnStack Stack => _stack;

    /// <summary>
    /// Processes the list of tokens, performing evaluations and updating the stack.
    /// 
    /// For each token, the processor will:
    /// - If the token is a number, push it onto the stack.
    /// - If the token is an operator, pop the last two numbers from the stack,
    ///   perform the evaluation, and push the result back onto the stack.
    /// </summary>
    /// <param name="tokens">The list of tokens to process.</param>
    /// <returns>The result of the evaluation.</returns>
    /// <remarks>Returns a failed result with an error if pop fails.</remarks>
    public Result<double> Process(List<Token> tokens)
    {
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

                var result = RpnEvaluator.Evaluate(numberToken1.Value, numberToken2.Value, operatorToken.Symbol);

                if (!result.IsSuccessful)
                {
                    return result;
                }

                _stack.Push(result.Value);
            }
        }

        return new Result<double>(0);
    }
}
namespace Clac.Core;

/// <summary>
/// Represents a token in the RPN calculator, which can be either a number or an operator.
/// </summary>
public abstract record Token
{
    private Token() { }

    /// <summary>
    /// Represents a number token with its value.
    /// </summary>
    public sealed record NumberToken(double Value) : Token;

    /// <summary>
    /// Represents an operator token with its symbol.
    /// </summary>
    public sealed record OperatorToken(OperatorSymbol Symbol) : Token;

    /// <summary>
    /// Creates a new number token with the specified value.
    /// </summary>
    public static Token CreateNumber(double value) => new NumberToken(value);

    /// <summary>
    /// Creates a new operator token with the specified symbol.
    /// </summary>
    public static Token CreateOperator(OperatorSymbol symbol) => new OperatorToken(symbol);
}


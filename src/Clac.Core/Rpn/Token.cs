namespace Clac.Core.Rpn;

using Clac.Core.Enums;

public abstract record Token
{
    private Token() { }

    public sealed record NumberToken(double Value) : Token;

    public sealed record OperatorToken(OperatorSymbol Symbol) : Token;

    public static Token CreateNumber(double value) => new NumberToken(value);

    public static Token CreateOperator(OperatorSymbol symbol) => new OperatorToken(symbol);

    public sealed record CommandToken(CommandSymbol Command) : Token;

    public static Token CreateCommand(CommandSymbol command) => new CommandToken(command);
}

namespace Clac.Core.Rpn;

public abstract record Token
{
    private Token() { }

    public sealed record NumberToken(double Value) : Token;

    public sealed record OperatorToken(string Symbol) : Token;

    public static Token CreateNumber(double Value) => new NumberToken(Value);

    public static Token CreateOperator(string Symbol) => new OperatorToken(Symbol);

    public sealed record FunctionToken(string FunctionName) : Token;

    public static Token CreateFunction(string FunctionName) => new FunctionToken(FunctionName);
}

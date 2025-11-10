using DotNext;
using Clac.Core.Enums;

namespace Clac.Core;

public static class Command
{
    public static Result<CommandSymbol> GetCommandSymbol(string command)
    {
        return command switch
        {
            "clear" => new Result<CommandSymbol>(CommandSymbol.Clear),
            "pop" => new Result<CommandSymbol>(CommandSymbol.Pop),
            "swap" => new Result<CommandSymbol>(CommandSymbol.Swap),
            "sum" => new Result<CommandSymbol>(CommandSymbol.Sum),
            "sqrt" => new Result<CommandSymbol>(CommandSymbol.Sqrt),
            "pow" => new Result<CommandSymbol>(CommandSymbol.Pow),
            "reciprocal" => new Result<CommandSymbol>(CommandSymbol.Reciprocal),
            _ => new Result<CommandSymbol>(new InvalidOperationException($"Invalid command: '{command}'"))
        };
    }

    public static bool IsValidCommand(string command)
    {
        return command switch
        {
            "clear" => true,
            "pop" => true,
            "swap" => true,
            "sum" => true,
            "sqrt" => true,
            "pow" => true,
            "reciprocal" => true,
            _ => false
        };
    }

    public static string GetCommandString(CommandSymbol symbol)
    {
        return symbol switch
        {
            CommandSymbol.Clear => "clear",
            CommandSymbol.Pop => "pop",
            CommandSymbol.Swap => "swap",
            CommandSymbol.Sum => "sum",
            CommandSymbol.Sqrt => "sqrt",
            CommandSymbol.Pow => "pow",
            CommandSymbol.Reciprocal => "reciprocal",
            _ => throw new InvalidOperationException($"Unknown command symbol: {symbol}")
        };
    }
}


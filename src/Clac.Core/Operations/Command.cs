using DotNext;
using Clac.Core.Enums;

namespace Clac.Core.Operations;

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
            "recip" => new Result<CommandSymbol>(CommandSymbol.Recip),
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
            "recip" => true,
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
            CommandSymbol.Recip => "recip",
            _ => throw new InvalidOperationException($"Unknown command symbol: {symbol}")
        };
    }
}


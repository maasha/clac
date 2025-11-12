using System.Globalization;
using DotNext;

namespace Clac.Core;

public class RpnParser
{
    public static Result<List<Token>> Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new Result<List<Token>>([]);

        var inputItems = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var validationResult = ValidateInput(inputItems);

        if (!validationResult.IsSuccessful)
            return new Result<List<Token>>(validationResult.Error);

        return CreateTokensFromItems(validationResult.Value);
    }

    private static Result<List<Token>> CreateTokensFromItems(string[] items)
    {
        var tokens = new List<Token>();

        foreach (var item in items)
        {
            var tokenResult = CreateTokenFromItem(item);
            if (!tokenResult.IsSuccessful)
                return new Result<List<Token>>(tokenResult.Error);
            tokens.Add(tokenResult.Value);
        }

        return new Result<List<Token>>(tokens);
    }

    private static Result<Token> CreateTokenFromItem(string item)
    {
        var tokenResult = CreateTokenFromString(item);
        if (!tokenResult.IsSuccessful)
            return new Result<Token>(tokenResult.Error);
        return tokenResult;
    }

    private static Result<Token> CreateTokenFromString(string item)
    {
        if (double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            return new Result<Token>(Token.CreateNumber(number));

        if (IsCommand(item))
            return CreateCommandToken(item);

        return CreateOperatorToken(item);
    }

    private static Result<string[]> ValidateInput(string[] input)
    {
        var errors = CollectInvalidItems(input);

        if (errors.Count > 0)
        {
            var errorMessage = FormatInvalidInputMessage(errors);
            return new Result<string[]>(new Exception(errorMessage));
        }

        return new Result<string[]>(input);
    }

    private static List<string> CollectInvalidItems(string[] input)
    {
        var errors = new List<string>();

        foreach (var item in input)
        {
            if (IsInvalidItem(item))
                errors.Add(item);
        }

        return errors;
    }

    private static bool IsInvalidItem(string item)
    {
        bool isNumber = IsNumber(item);
        bool isOperator = Operator.IsValidOperator(item);
        bool isCommand = IsCommand(item);

        return !isNumber && !isOperator && !isCommand;
    }

    private static bool IsCommand(string item)
    {
        if (!item.EndsWith("()"))
            return false;

        var commandName = ExtractCommandName(item);
        return Command.IsValidCommand(commandName);
    }

    private static bool IsNumber(string item)
    {
        return double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }

    private static Result<Token> CreateCommandToken(string item)
    {
        var commandString = ExtractCommandName(item);
        var commandResult = Command.GetCommandSymbol(commandString);
        if (!commandResult.IsSuccessful)
            return new Result<Token>(commandResult.Error);
        return new Result<Token>(Token.CreateCommand(commandResult.Value));
    }

    private static Result<Token> CreateOperatorToken(string item)
    {
        var operatorResult = Operator.GetOperatorSymbol(item);
        if (!operatorResult.IsSuccessful)
            return new Result<Token>(operatorResult.Error);
        return new Result<Token>(Token.CreateOperator(operatorResult.Value));
    }

    private static string ExtractCommandName(string commandWithParentheses)
    {
        // Remove "()" suffix and lowercase command name
        return commandWithParentheses[..^2].ToLowerInvariant();
    }

    private static string FormatInvalidInputMessage(List<string> errors)
    {
        return "Invalid input: " + string.Join(" ", errors);
    }
}
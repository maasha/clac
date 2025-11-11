using System.Globalization;
using DotNext;
using Clac.Core.Enums;

namespace Clac.Core;

public class RpnParser
{
    private static readonly string[] ValidCommands = ["clear()", "pop()", "swap()", "sum()", "sqrt()", "pow()", "reciprocal()"];

    public static Result<List<Token>> Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return ParseEmptyInput();

        var inputItems = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var validationResult = ValidateInput(inputItems);

        if (!validationResult.IsSuccessful)
            return ValidationError(validationResult.Error);

        return CreateTokensFromItems(validationResult.Value);
    }

    private static Result<List<Token>> CreateTokensFromItems(string[] items)
    {
        var tokens = new List<Token>();

        foreach (var item in items)
        {
            var tokenResult = CreateTokenFromItem(item);
            if (!tokenResult.IsSuccessful)
                return TokenParsingError(tokenResult.Error);
            tokens.Add(tokenResult.Value);
        }

        return new Result<List<Token>>(tokens);
    }

    private static Result<Token> CreateTokenFromItem(string item)
    {
        var tokenResult = CreateTokenFromString(item);
        if (!tokenResult.IsSuccessful)
            return TokenCreationError(tokenResult.Error);
        return tokenResult;
    }

    private static Result<List<Token>> ValidationError(Exception error)
    {
        return new Result<List<Token>>(error);
    }

    private static Result<List<Token>> TokenParsingError(Exception error)
    {
        return new Result<List<Token>>(error);
    }

    private static Result<List<Token>> ParseEmptyInput()
    {
        return new Result<List<Token>>([]);
    }

    private static Result<Token> TokenCreationError(Exception error)
    {
        return new Result<Token>(error);
    }

    private static Result<string[]> InputValidationError(Exception error)
    {
        return new Result<string[]>(error);
    }

    private static Result<Token> CreateTokenFromString(string item)
    {
        if (double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            return new Result<Token>(Token.CreateNumber(number));

        if (ValidCommands.Contains(item))
        {
            var commandString = ExtractCommandName(item);
            var commandResult = Command.GetCommandSymbol(commandString);
            if (!commandResult.IsSuccessful)
                return TokenCreationError(commandResult.Error);
            return new Result<Token>(Token.CreateCommand(commandResult.Value));
        }

        var operatorResult = Operator.GetOperatorSymbol(item);
        if (!operatorResult.IsSuccessful)
            return TokenCreationError(operatorResult.Error);
        return new Result<Token>(Token.CreateOperator(operatorResult.Value));
    }

    private static Result<string[]> ValidateInput(string[] input)
    {
        var errors = CollectInvalidItems(input);

        if (errors.Count > 0)
        {
            var errorMessage = FormatInvalidInputMessage(errors);
            return InputValidationError(new Exception(errorMessage));
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
        bool isCommand = ValidCommands.Contains(item);

        return !isNumber && !isOperator && !isCommand;
    }

    private static bool IsNumber(string item)
    {
        return double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }

    private static string ExtractCommandName(string commandWithParentheses)
    {
        return commandWithParentheses[..^2]; // Remove "()" suffix
    }

    private static string FormatInvalidInputMessage(List<string> errors)
    {
        return "Invalid input: " + string.Join(" ", errors);
    }
}
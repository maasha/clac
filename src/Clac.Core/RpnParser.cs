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
        {
            return ParseEmptyInput();
        }

        var inputItems = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var validationResult = ValidateInput(inputItems);

        if (!validationResult.IsSuccessful)
        {
            return ValidationError(validationResult.Error);
        }

        var tokens = new List<Token>();

        foreach (var item in validationResult.Value)
        {
            var tokenResult = CreateTokenFromString(item);
            if (!tokenResult.IsSuccessful)
            {
                return TokenParsingError(tokenResult.Error);
            }
            tokens.Add(tokenResult.Value);
        }

        return ParseSuccess(tokens);
    }

    private static Result<List<Token>> ValidationError(Exception error)
    {
        return new Result<List<Token>>(error);
    }

    private static Result<List<Token>> TokenParsingError(Exception error)
    {
        return new Result<List<Token>>(error);
    }

    private static Result<List<Token>> ParseSuccess(List<Token> tokens)
    {
        return new Result<List<Token>>(tokens);
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
        {
            return new Result<Token>(Token.CreateNumber(number));
        }

        if (ValidCommands.Contains(item))
        {
            var commandString = item[..^2];
            var commandResult = Command.GetCommandSymbol(commandString);
            if (!commandResult.IsSuccessful)
            {
                return TokenCreationError(commandResult.Error);
            }
            return new Result<Token>(Token.CreateCommand(commandResult.Value));
        }

        var operatorResult = Operator.GetOperatorSymbol(item);
        if (!operatorResult.IsSuccessful)
        {
            return TokenCreationError(operatorResult.Error);
        }
        return new Result<Token>(Token.CreateOperator(operatorResult.Value));
    }

    private static Result<string[]> ValidateInput(string[] input)
    {
        var errors = new List<string>();

        foreach (var item in input)
        {
            bool isNumber = double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool isOperator = Operator.IsValidOperator(item);
            bool isCommand = ValidCommands.Contains(item);

            if (!isNumber && !isOperator && !isCommand)
            {
                errors.Add(item);
            }
        }

        if (errors.Count > 0)
        {
            var errorMessage = "Invalid input: " + string.Join(" ", errors);
            return InputValidationError(new Exception(errorMessage));
        }

        return new Result<string[]>(input);
    }
}
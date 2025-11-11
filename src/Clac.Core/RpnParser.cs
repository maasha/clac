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
            return new Result<List<Token>>([]);
        }

        var inputItems = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var validationResult = ValidateInput(inputItems);

        if (!validationResult.IsSuccessful)
        {
            return new Result<List<Token>>(validationResult.Error);
        }

        var tokens = new List<Token>();

        foreach (var item in validationResult.Value)
        {
            var tokenResult = CreateTokenFromString(item);
            if (!tokenResult.IsSuccessful)
            {
                return new Result<List<Token>>(tokenResult.Error);
            }
            tokens.Add(tokenResult.Value);
        }

        return new Result<List<Token>>(tokens);
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
                return new Result<Token>(commandResult.Error);
            }
            return new Result<Token>(Token.CreateCommand(commandResult.Value));
        }

        var operatorResult = Operator.GetOperatorSymbol(item);
        if (!operatorResult.IsSuccessful)
        {
            return new Result<Token>(operatorResult.Error);
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
            return new Result<string[]>(new Exception(errorMessage));
        }

        return new Result<string[]>(input);
    }
}
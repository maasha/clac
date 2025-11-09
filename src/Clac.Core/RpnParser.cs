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
            if (double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                tokens.Add(Token.CreateNumber(number));
            }
            else if (ValidCommands.Contains(item))
            {
                tokens.Add(Token.CreateCommand(item[..^2]));
            }
            else
            {
                var operatorResult = Operator.GetOperatorSymbol(item);
                if (!operatorResult.IsSuccessful)
                {
                    return new Result<List<Token>>(operatorResult.Error);
                }
                tokens.Add(Token.CreateOperator(operatorResult.Value));
            }
        }

        return new Result<List<Token>>(tokens);
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
using System.Globalization;
using DotNext;
using Clac.Core.Enums;

namespace Clac.Core;

/// <summary>
/// Class for parsing the calculator input.
/// </summary>
public class RpnParser
{
    /// <summary>
    /// Parses the input string into a list of tokens.
    /// 
    /// Note that validation takes place before parsing to allow the user to 
    /// correct the input.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A list of tokens.</returns>
    /// <remarks>Returns a failed result with an error if the input contains invalid tokens.</remarks>
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
            else if (item is "clear()" or "pop()" or "swap()" or "sum()" or "sqrt()")
            {
                tokens.Add(Token.CreateCommand(item[..^2]));
            }
            else
            {
                var operatorSymbol = item switch
                {
                    "+" => OperatorSymbol.Add,
                    "-" => OperatorSymbol.Subtract,
                    "*" => OperatorSymbol.Multiply,
                    "/" => OperatorSymbol.Divide,
                    _ => throw new InvalidOperationException($"Unreachable: validation failed for '{item}'")
                };
                tokens.Add(Token.CreateOperator(operatorSymbol));
            }
        }

        return new Result<List<Token>>(tokens);
    }

    /// <summary>
    /// Validates the input and returns a list of errors with all input items
    /// that are not a number, an operator symbol, or a command.
    /// </summary>
    /// <param name="input">The input array of strings to validate.</param>
    /// <returns>A result containing the validated input array or an error if the input is invalid.</returns>
    /// <remarks>Returns a failed result with an error message if the input contains invalid tokens.</remarks>
    private static Result<string[]> ValidateInput(string[] input)
    {
        var errors = new List<string>();

        foreach (var item in input)
        {
            bool isNumber = double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            bool isOperator = item is "+" or "-" or "*" or "/";
            bool isCommand = item is "clear()" or "pop()" or "swap()" or "sum()" or "sqrt()";

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
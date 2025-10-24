using DotNext;

namespace Clac.Core;

/// <summary>
/// Class for parsing the calculator input.
/// </summary>
public class RpnParser
{
    /// <summary>
    /// Parses the input string into a list of tokens.
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
            if (double.TryParse(item, out var number))
            {
                tokens.Add(Token.CreateNumber(number));
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
    /// that are not a number or an operator symbol.
    /// </summary>
    /// <param name="input">The input array of strings to validate.</param>
    /// <returns>A result containing the validated input array or an error if the input is invalid.</returns>
    /// <remarks>Returns a failed result with an error message if the input contains invalid tokens.</remarks>
    private static Result<string[]> ValidateInput(string[] input)
    {
        var errors = new List<string>();

        foreach (var item in input)
        {
            bool isNumber = double.TryParse(item, out _);
            bool isOperator = item is "+" or "-" or "*" or "/";

            if (!isNumber && !isOperator)
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
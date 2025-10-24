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
    /// <exception cref="ArgumentException">Thrown when the input is invalid.</exception>
    public static Result<List<TokenType>> Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new Result<List<TokenType>>([.. Array.Empty<TokenType>()]);
        }

        var inputItems = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var validationResult = ValidateInput(inputItems);
        if (!validationResult.IsSuccessful)
        {
            return new Result<List<TokenType>>(validationResult.Error);
        }

        var tokenTypes = new List<TokenType>();

        foreach (var item in validationResult.Value)
        {
            if (double.TryParse(item, out _))
            {
                tokenTypes.Add(TokenType.Number);
            }
            else
            {
                tokenTypes.Add(TokenType.Operator);
            }
        }

        return new Result<List<TokenType>>(tokenTypes);
    }

    /// <summary>
    /// Validates the input and returns a list of errors with all input items
    /// that are not a number or an operator symbol.
    /// </summary>
    /// <param name="input">The input array of strings to validate.</param>
    /// <returns>A result containing the validated input array or an error if the input is invalid.</returns>
    /// <exception cref="ArgumentException">Thrown when the input is invalid.</exception>
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
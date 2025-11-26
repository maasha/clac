using System.Globalization;
using DotNext;
using Clac.Core.Operations;
using Clac.Core.Functions;

namespace Clac.Core.Rpn;

public class Parser(OperatorRegistry operatorRegistry, FunctionRegistry functionRegistry)
{
    private readonly OperatorRegistry _operatorRegistry = operatorRegistry;

    public Result<List<Token>> Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new Result<List<Token>>([]);

        var inputItems = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var validationResult = ValidateInput(inputItems);

        if (!validationResult.IsSuccessful)
            return new Result<List<Token>>(validationResult.Error);

        return CreateTokensFromItems(validationResult.Value);
    }

    private Result<List<Token>> CreateTokensFromItems(string[] items)
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

    private Result<Token> CreateTokenFromItem(string item)
    {
        var tokenResult = CreateTokenFromString(item);
        if (!tokenResult.IsSuccessful)
            return new Result<Token>(tokenResult.Error);
        return tokenResult;
    }

    private Result<Token> CreateTokenFromString(string item)
    {
        if (double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            return new Result<Token>(Token.CreateNumber(number));

        if (IsFunction(item))
            return CreateFunctionToken(item);

        return CreateOperatorToken(item);
    }

    private Result<string[]> ValidateInput(string[] input)
    {
        var errors = CollectInvalidItems(input);

        if (errors.Count > 0)
        {
            var errorMessage = FormatInvalidInputMessage(errors);
            return new Result<string[]>(new Exception(errorMessage));
        }

        return new Result<string[]>(input);
    }

    private List<string> CollectInvalidItems(string[] input)
    {
        var errors = new List<string>();

        foreach (var item in input)
        {
            if (IsInvalidItem(item))
                errors.Add(item);
        }

        return errors;
    }

    private bool IsInvalidItem(string item)
    {
        bool isNumber = IsNumber(item);
        bool isOperator = _operatorRegistry.GetOperator(item).IsSuccessful;
        bool isFunction = IsFunction(item);

        return !isNumber && !isOperator && !isFunction;
    }

    private bool IsFunction(string item)
    {
        if (!item.EndsWith("()"))
            return false;

        var functionName = ExtractFunctionName(item);
        var result = functionRegistry.IsValidFunction(functionName);
        if (!result.IsSuccessful)
            return false;
        return true;
    }

    private static bool IsNumber(string item)
    {
        return double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }

    private Result<Token> CreateFunctionToken(string item)
    {
        var functionString = ExtractFunctionName(item);
        var result = functionRegistry.GetFunction(functionString);
        if (!result.IsSuccessful)
            return new Result<Token>(result.Error);
        return new Result<Token>(Token.CreateFunction(result.Value.Name));
    }

    private Result<Token> CreateOperatorToken(string item)
    {
        var operatorResult = _operatorRegistry.GetOperator(item);
        if (!operatorResult.IsSuccessful)
            return new Result<Token>(operatorResult.Error);
        return new Result<Token>(Token.CreateOperator(operatorResult.Value.Symbol));
    }

    private static string ExtractFunctionName(string functionWithParentheses)
    {
        // Remove "()" suffix and lowercase function name
        return functionWithParentheses[..^2].ToLowerInvariant();
    }

    private static string FormatInvalidInputMessage(List<string> errors)
    {
        return "Invalid input: " + string.Join(" ", errors);
    }
}
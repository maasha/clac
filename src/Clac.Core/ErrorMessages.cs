using Clac.Core.Enums;

namespace Clac.Core;

public static class ErrorMessages
{
    public const string StackEmpty = "Stack is empty";
    public const string StackHasLessThanTwoNumbers = "Stack has less than two numbers";
    public const string DivisionByZero = "Division by zero";
    public const string InvalidNegativeSquareRoot = "Invalid: negative square root";
    public const string NoResultOnStack = "No result on stack";
    public const string HistoryStackIsEmpty = "History stack is empty";
    public const string HistoryInputIsEmpty = "History input is empty";

    public const string HistoryIsEmpty = "History is empty";
    public const string ValidationFailed = "Validation failed";

    public const string SavingFailed = "Saving failed";
    public const string LoadingFailed = "Loading failed";

    public static string UnknownOperator(OperatorSymbol op) => $"Unknown operator: {op}";
}


using DotNext;
using Clac.Core.Enums;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Rpn;

public class Processor
{
    private const int MinimumStackSizeForBinaryOperation = 2;
    private readonly Stack _stack = new();
    private readonly Dictionary<CommandSymbol, Func<Result<double>?>> _commandHandlers;

    public Processor()
    {
        _commandHandlers = new Dictionary<CommandSymbol, Func<Result<double>?>>
        {
            { CommandSymbol.Clear, HandleClear },
            { CommandSymbol.Pop, HandlePop },
            { CommandSymbol.Swap, HandleSwap },
            { CommandSymbol.Sum, HandleSum },
            { CommandSymbol.Sqrt, HandleSqrt },
            { CommandSymbol.Pow, HandlePow },
            { CommandSymbol.Reciprocal, HandleReciprocal }
        };
    }

    public Stack Stack => CloneStack();

    public Result<double> Process(List<Token> tokens)
    {
        var processResult = ProcessTokens(tokens);
        if (!processResult.IsSuccessful)
            return new Result<double>(processResult.Error);
        return GetFinalResult(processResult.Value.commandExecuted, processResult.Value.commandResult);
    }

    private Result<(bool commandExecuted, double commandResult)> ProcessTokens(List<Token> tokens)
    {
        bool commandExecuted = false;
        double commandResult = 0;

        foreach (var token in tokens)
        {
            var tokenResult = ProcessSingleToken(token);
            if (!tokenResult.IsSuccessful)
                return new Result<(bool commandExecuted, double commandResult)>(tokenResult.Error);

            if (tokenResult.Value.HasValue)
            {
                commandExecuted = true;
                commandResult = tokenResult.Value.Value.commandResult;
            }
        }

        return new Result<(bool commandExecuted, double commandResult)>((commandExecuted, commandResult));
    }

    private Result<(bool commandExecuted, double commandResult)?> ProcessSingleToken(Token token)
    {
        if (token is Token.NumberToken numberToken)
            return ProcessNumberToken(numberToken);

        if (token is Token.OperatorToken operatorToken)
            return ProcessOperatorToken(operatorToken);

        if (token is Token.CommandToken commandToken)
            return ProcessCommandToken(commandToken);

        return NoCommandExecuted();
    }

    private Result<(bool commandExecuted, double commandResult)?> ProcessNumberToken(Token.NumberToken numberToken)
    {
        _stack.Push(numberToken.Value);
        return NoCommandExecuted();
    }

    private Result<(bool commandExecuted, double commandResult)?> ProcessOperatorToken(Token.OperatorToken operatorToken)
    {
        var operatorResult = ProcessOperator(operatorToken);
        return ConvertOperatorResultToTokenResult(operatorResult);
    }

    private Result<(bool commandExecuted, double commandResult)?> ConvertOperatorResultToTokenResult(Result<double> operatorResult)
    {
        return operatorResult.IsSuccessful
            ? NoCommandExecuted()
            : new Result<(bool commandExecuted, double commandResult)?>(operatorResult.Error);
    }

    private Result<(bool commandExecuted, double commandResult)?> NoCommandExecuted()
    {
        (bool commandExecuted, double commandResult)? nullValue = null;
        return new Result<(bool commandExecuted, double commandResult)?>(nullValue);
    }

    private Result<double> GetFinalResult(bool commandExecuted, double commandResult)
    {
        if (commandExecuted)
            return new Result<double>(commandResult);

        var finalResult = _stack.Peek();
        return finalResult.IsSuccessful
            ? finalResult
            : new Result<double>(new InvalidOperationException(NoResultOnStack));
    }

    private Result<(bool commandExecuted, double commandResult)?> ProcessCommandToken(Token.CommandToken commandToken)
    {
        var commandResult = ProcessCommand(commandToken.Command);
        if (commandResult == null)
            return NoCommandExecuted();

        return commandResult.Value.IsSuccessful
            ? new Result<(bool commandExecuted, double commandResult)?>((true, commandResult.Value.Value))
            : new Result<(bool commandExecuted, double commandResult)?>(commandResult.Value.Error);
    }

    private Result<double>? ProcessCommand(CommandSymbol command)
    {
        if (_commandHandlers.TryGetValue(command, out var handler))
            return handler();
        return null;
    }

    /// This was introduced to silence the errors that would just cause
    /// confusion in the calculator such as clicking pop() on an empty stack.
    private static Result<double> SuccessWithZero()
    {
        return new Result<double>(0);
    }

    private Result<double>? HandleClear()
    {
        _stack.Clear();
        return SuccessWithZero();
    }

    private Result<double>? HandlePop()
    {
        var result = _stack.Pop();
        return result.IsSuccessful ? result : SuccessWithZero();
    }

    private Result<double>? HandleSwap()
    {
        var result = _stack.Swap();
        return result.IsSuccessful ? result : SuccessWithZero();
    }

    private Result<double>? HandleSum()
    {
        var result = _stack.Sum();
        if (result.IsSuccessful)
        {
            _stack.Clear();
            _stack.Push(result.Value);
            return result;
        }
        return SuccessWithZero();
    }

    private Result<double>? HandleSqrt()
    {
        return HandleStackOperation(
            () => _stack.Sqrt(),
            popCount: 1,
            shouldIgnoreError: IsStackEmptyError);
    }

    private Result<double>? HandlePow()
    {
        return HandleStackOperation(
            () => _stack.Pow(),
            popCount: 2,
            shouldIgnoreError: error => IsStackEmptyError(error) || IsStackInsufficientNumbersError(error));
    }

    private Result<double>? HandleReciprocal()
    {
        return HandleStackOperation(
            () => _stack.Reciprocal(),
            popCount: 1,
            shouldIgnoreError: IsStackEmptyError);
    }

    private bool IsStackEmptyError(Exception error)
    {
        return error is InvalidOperationException && error.Message == StackEmpty;
    }

    private bool IsStackInsufficientNumbersError(Exception error)
    {
        return error is InvalidOperationException && error.Message == StackHasLessThanTwoNumbers;
    }

    private Result<double>? HandleStackOperation(
        Func<Result<double>> operation,
        int popCount,
        Func<Exception, bool> shouldIgnoreError)
    {
        var result = operation();

        if (!result.IsSuccessful)
        {
            if (shouldIgnoreError(result.Error))
                return SuccessWithZero();
            return result;
        }

        for (int i = 0; i < popCount; i++)
        {
            _stack.Pop();
        }
        _stack.Push(result.Value);
        return result;
    }

    private Result<double> ProcessOperator(Token.OperatorToken operatorToken)
    {
        if (_stack.Count < MinimumStackSizeForBinaryOperation)
            return new Result<double>(new InvalidOperationException(StackHasLessThanTwoNumbers));

        var number1 = _stack.Pop();
        var number2 = _stack.Pop();

        var result = Evaluator.Evaluate(number2.Value, number1.Value, operatorToken.Symbol);

        if (!result.IsSuccessful)
            return result;

        _stack.Push(result.Value);
        return result;
    }

    private Stack CloneStack()
    {
        var copy = new Stack();

        foreach (var value in _stack.ToArray())
        {
            copy.Push(value);
        }

        return copy;
    }

    public void RestoreStack(Stack stack)
    {
        _stack.Clear();
        foreach (var value in stack.ToArray())
            _stack.Push(value);
    }
}

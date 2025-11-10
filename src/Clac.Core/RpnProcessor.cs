using DotNext;
using Clac.Core.Enums;

namespace Clac.Core;

public class RpnProcessor
{
    private const string ErrorStackEmpty = "Stack is empty";
    private const string ErrorStackHasLessThanTwoElements = "Stack has less than two elements";

    private readonly RpnStack _stack = new();
    private readonly Dictionary<CommandSymbol, Func<Result<double>?>> _commandHandlers;

    public RpnProcessor()
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

    public RpnStack Stack => CloneStack();

    public Result<double> Process(List<Token> tokens)
    {
        var processResult = ProcessTokens(tokens);
        if (!processResult.IsSuccessful)
            return ProcessingError(processResult.Error);
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
                return TokenProcessingError(tokenResult.Error);

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
            : ErrorResult(operatorResult.Error);
    }

    private Result<(bool commandExecuted, double commandResult)?> NoCommandExecuted()
    {
        (bool commandExecuted, double commandResult)? nullValue = null;
        return new Result<(bool commandExecuted, double commandResult)?>(nullValue);
    }

    private Result<(bool commandExecuted, double commandResult)?> ErrorResult(Exception error)
    {
        return new Result<(bool commandExecuted, double commandResult)?>(error);
    }

    private Result<(bool commandExecuted, double commandResult)> TokenProcessingError(Exception error)
    {
        return new Result<(bool commandExecuted, double commandResult)>(error);
    }

    private Result<double> ProcessingError(Exception error)
    {
        return new Result<double>(error);
    }

    private Result<double> NoResultOnStackError()
    {
        return new Result<double>(new InvalidOperationException("No result on stack"));
    }

    private Result<double> GetFinalResult(bool commandExecuted, double commandResult)
    {
        if (commandExecuted)
            return new Result<double>(commandResult);

        var finalResult = _stack.Peek();
        return finalResult.IsSuccessful
            ? finalResult
            : NoResultOnStackError();
    }

    private Result<(bool commandExecuted, double commandResult)?> ProcessCommandToken(Token.CommandToken commandToken)
    {
        var commandProcessResult = ProcessCommand(commandToken.Command);
        if (commandProcessResult.HasValue)
        {
            if (!commandProcessResult.Value.IsSuccessful)
                return ErrorResult(commandProcessResult.Value.Error);
            return new Result<(bool commandExecuted, double commandResult)?>((true, commandProcessResult.Value.Value));
        }
        (bool commandExecuted, double commandResult)? nullValue = null;
        return new Result<(bool commandExecuted, double commandResult)?>(nullValue);
    }

    private Result<double>? ProcessCommand(CommandSymbol command)
    {
        if (_commandHandlers.TryGetValue(command, out var handler))
            return handler();
        return null;
    }

    private Result<double> IgnoreError()
    {
        return new Result<double>(0);
    }

    private Result<double>? HandleClear()
    {
        _stack.Clear();
        return IgnoreError();
    }

    private Result<double>? HandlePop()
    {
        var result = _stack.Pop();
        return result.IsSuccessful ? result : IgnoreError();
    }

    private Result<double>? HandleSwap()
    {
        var result = _stack.Swap();
        return result.IsSuccessful ? result : IgnoreError();
    }

    private Result<double>? HandleSum()
    {
        var result = _stack.Sum();
        if (result.IsSuccessful)
        {
            _stack.Clear();
            _stack.Push(result.Value);
        }
        return result.IsSuccessful ? result : IgnoreError();
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
            shouldIgnoreError: error => IsStackEmptyError(error) || IsStackInsufficientElementsError(error));
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
        return error is InvalidOperationException && error.Message == ErrorStackEmpty;
    }

    private bool IsStackInsufficientElementsError(Exception error)
    {
        return error is InvalidOperationException && error.Message == ErrorStackHasLessThanTwoElements;
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
                return IgnoreError();
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
        if (_stack.Count < 2)
            return new Result<double>(new InvalidOperationException("Stack has less than two numbers"));

        var numberToken1 = _stack.Pop();
        var numberToken2 = _stack.Pop();

        var result = RpnEvaluator.Evaluate(numberToken2.Value, numberToken1.Value, operatorToken.Symbol);

        if (!result.IsSuccessful)
            return result;

        _stack.Push(result.Value);
        return result;
    }

    private RpnStack CloneStack()
    {
        var copy = new RpnStack();

        foreach (var value in _stack.ToArray())
        {
            copy.Push(value);
        }

        return copy;
    }
}

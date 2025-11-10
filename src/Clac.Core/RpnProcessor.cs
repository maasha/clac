using DotNext;

namespace Clac.Core;

public class RpnProcessor
{
    private readonly RpnStack _stack = new();
    private readonly Dictionary<string, Func<Result<double>?>> _commandHandlers;

    public RpnProcessor()
    {
        _commandHandlers = new Dictionary<string, Func<Result<double>?>>
        {
            { "clear", HandleClear },
            { "pop", HandlePop },
            { "swap", HandleSwap },
            { "sum", HandleSum },
            { "sqrt", HandleSqrt },
            { "pow", HandlePow },
            { "reciprocal", HandleReciprocal }
        };
    }

    public RpnStack Stack => CloneStack();

    public Result<double> Process(List<Token> tokens)
    {
        bool commandExecuted = false;
        double commandResult = 0;

        foreach (var token in tokens)
        {
            if (token is Token.NumberToken numberToken)
            {
                _stack.Push(numberToken.Value);
            }
            else if (token is Token.OperatorToken operatorToken)
            {
                var result = ProcessOperator(operatorToken);
                if (!result.IsSuccessful)
                    return result;
            }
            else if (token is Token.CommandToken commandToken)
            {
                var commandProcessResult = ProcessCommand(commandToken.Command);
                if (commandProcessResult.HasValue)
                {
                    if (!commandProcessResult.Value.IsSuccessful)
                        return commandProcessResult.Value;
                    commandExecuted = true;
                    commandResult = commandProcessResult.Value.Value;
                }
            }
        }

        if (commandExecuted)
            return new Result<double>(commandResult);

        var finalResult = _stack.Peek();
        return finalResult.IsSuccessful
            ? finalResult
            : new Result<double>(new InvalidOperationException("No result on stack"));
    }

    private Result<double>? ProcessCommand(string command)
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
        var result = _stack.Sqrt();

        if (!result.IsSuccessful)
        {
            if (result.Error.Message.Contains("Stack is empty"))
                return IgnoreError();
            return result;
        }

        _stack.Pop();
        _stack.Push(result.Value);
        return result;
    }

    private Result<double>? HandlePow()
    {
        var result = _stack.Pow();

        if (!result.IsSuccessful)
        {
            if (result.Error.Message.Contains("Stack is empty") || result.Error.Message.Contains("Stack has less than two elements"))
                return IgnoreError();
            return result;
        }

        _stack.Pop();
        _stack.Pop();
        _stack.Push(result.Value);
        return result;
    }

    private Result<double>? HandleReciprocal()
    {
        var result = _stack.Reciprocal();

        if (!result.IsSuccessful)
        {
            if (result.Error.Message.Contains("Stack is empty"))
                return IgnoreError();
            return result;
        }

        _stack.Pop();
        _stack.Push(result.Value);
        return result;
    }

    private Result<double> ProcessOperator(Token.OperatorToken operatorToken)
    {
        if (_stack.Count < 2)
        {
            return new Result<double>(new InvalidOperationException("Stack has less than two numbers"));
        }

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

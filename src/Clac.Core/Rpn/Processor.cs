using DotNext;
using Clac.Core.Enums;
using static Clac.Core.ErrorMessages;
using Clac.Core.Operations;
using Clac.Core.Functions;

namespace Clac.Core.Rpn;

public class Processor
{
    private readonly OperatorRegistry _operatorRegistry;
    private readonly FunctionRegistry _functionRegistry;
    private readonly Stack _stack = new();
    private readonly Parser _parser;


    public OperatorRegistry OperatorRegistry => _operatorRegistry;
    public FunctionRegistry FunctionRegistry => _functionRegistry;
    public Parser Parser => _parser;

    public Processor(OperatorRegistry? operatorRegistry = null, FunctionRegistry? functionRegistry = null)
    {
        _operatorRegistry = operatorRegistry ?? new DefaultOperatorRegistry();
        _functionRegistry = functionRegistry ?? new DefaultFunctionRegistry();
        _parser = new Parser(_operatorRegistry);
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

    private static Result<(bool commandExecuted, double commandResult)?> ConvertOperatorResultToTokenResult(Result<double> operatorResult)
    {
        return operatorResult.IsSuccessful
            ? NoCommandExecuted()
            : new Result<(bool commandExecuted, double commandResult)?>(operatorResult.Error);
    }

    private static Result<(bool commandExecuted, double commandResult)?> NoCommandExecuted()
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
        var result = _functionRegistry.GetFunction(command.ToString());
        if (!result.IsSuccessful)
            return new Result<double>(result.Error);

        return result.Value.Execute(_stack);
    }

    private Result<double> ProcessOperator(Token.OperatorToken operatorToken)
    {
        var opResult = _operatorRegistry.GetOperator(operatorToken.Symbol);
        if (!opResult.IsSuccessful)
            return new Result<double>(opResult.Error);

        var op = opResult.Value;

        var stackSizeResult = op.ValidateStackSize(_stack.Count);
        if (!stackSizeResult.IsSuccessful)
            return new Result<double>(stackSizeResult.Error);

        return op.Evaluate(_stack);
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

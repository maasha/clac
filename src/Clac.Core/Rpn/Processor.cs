using DotNext;
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
        _parser = new Parser(_operatorRegistry, _functionRegistry);
    }

    public Stack Stack => CloneStack();

    public Result<double> Process(List<Token> tokens)
    {
        var processResult = ProcessTokens(tokens);
        if (!processResult.IsSuccessful)
            return new Result<double>(processResult.Error);
        return GetFinalResult(processResult.Value.functionExecuted, processResult.Value.functionResult);
    }

    private Result<(bool functionExecuted, double functionResult)> ProcessTokens(List<Token> tokens)
    {
        bool functionExecuted = false;
        double functionResult = 0;

        foreach (var token in tokens)
        {
            var tokenResult = ProcessSingleToken(token);
            if (!tokenResult.IsSuccessful)
                return new Result<(bool functionExecuted, double functionResult)>(tokenResult.Error);

            if (tokenResult.Value.HasValue)
            {
                functionExecuted = true;
                functionResult = tokenResult.Value.Value.functionResult;
            }
        }

        return new Result<(bool functionExecuted, double functionResult)>((functionExecuted, functionResult));
    }

    private Result<(bool functionExecuted, double functionResult)?> ProcessSingleToken(Token token)
    {
        if (token is Token.NumberToken numberToken)
            return ProcessNumberToken(numberToken);

        if (token is Token.OperatorToken operatorToken)
            return ProcessOperatorToken(operatorToken);

        if (token is Token.FunctionToken functionToken)
            return ProcessFunctionToken(functionToken);

        return NoFunctionExecuted();
    }

    private Result<(bool functionExecuted, double functionResult)?> ProcessNumberToken(Token.NumberToken numberToken)
    {
        _stack.Push(numberToken.Value);
        return NoFunctionExecuted();
    }

    private Result<(bool functionExecuted, double functionResult)?> ProcessOperatorToken(Token.OperatorToken operatorToken)
    {
        var operatorResult = ProcessOperator(operatorToken);
        return ConvertOperatorResultToTokenResult(operatorResult);
    }

    private static Result<(bool functionExecuted, double functionResult)?> ConvertOperatorResultToTokenResult(Result<double> operatorResult)
    {
        return operatorResult.IsSuccessful
            ? NoFunctionExecuted()
            : new Result<(bool functionExecuted, double functionResult)?>(operatorResult.Error);
    }

    private static Result<(bool functionExecuted, double functionResult)?> NoFunctionExecuted()
    {
        (bool functionExecuted, double functionResult)? nullValue = null;
        return new Result<(bool functionExecuted, double functionResult)?>(nullValue);
    }

    private Result<double> GetFinalResult(bool functionExecuted, double functionResult)
    {
        if (functionExecuted)
            return new Result<double>(functionResult);

        var finalResult = _stack.Peek();
        return finalResult.IsSuccessful
            ? finalResult
            : new Result<double>(new InvalidOperationException(NoResultOnStack));
    }

    private Result<(bool functionExecuted, double functionResult)?> ProcessFunctionToken(Token.FunctionToken functionToken)
    {
        var result = ProcessFunction(functionToken);
        if (result == null)
            return NoFunctionExecuted();

        return result.Value.IsSuccessful
            ? new Result<(bool functionExecuted, double functionResult)?>((true, result.Value.Value))
            : new Result<(bool functionExecuted, double functionResult)?>(result.Value.Error);
    }

    private Result<double>? ProcessFunction(Token.FunctionToken functionToken)
    {
        var result = _functionRegistry.GetFunction(functionToken.FunctionName);
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
            copy.Push(value);

        return copy;
    }

    public void RestoreStack(Stack stack)
    {
        _stack.Clear();
        foreach (var value in stack.ToArray())
            _stack.Push(value);
    }
}

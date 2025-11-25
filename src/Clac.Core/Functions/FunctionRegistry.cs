using DotNext;
namespace Clac.Core.Functions;

public class FunctionRegistry
{
    private readonly Dictionary<string, IFunction> _functions = [];

    public void Register(IFunction function)
    {
        _functions[function.Name.ToLowerInvariant()] = function;
    }

    public Result<IFunction> GetFunction(string functionName)
    {
        if (!_functions.TryGetValue(functionName.ToLowerInvariant(), out var function))
            return new Result<IFunction>(FunctionNotFound(functionName));
        return new Result<IFunction>(function);
    }

    public Result<bool> IsValidFunction(string functionName)
    {
        if (!_functions.ContainsKey(functionName.ToLowerInvariant()))
            return new Result<bool>(FunctionNotFound(functionName));
        return new Result<bool>(true);
    }

    private static InvalidOperationException FunctionNotFound(string functionName)
    {
        return new InvalidOperationException($"Function '{functionName}' not found");
    }
}
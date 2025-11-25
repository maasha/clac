namespace Clac.Core.Functions;

public class DefaultFunctionRegistry : FunctionRegistry
{
    public DefaultFunctionRegistry()
    {
        Register(new PopFunction());
        Register(new SwapFunction());
    }
}
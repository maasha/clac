namespace Clac.Core.Functions;

public class DefaultFunctionRegistry : FunctionRegistry
{
    public DefaultFunctionRegistry()
    {
        Register(new PopFunction());
        Register(new SwapFunction());
        Register(new SumFunction());
        Register(new SqrtFunction());
        Register(new PowFunction());
        Register(new RecipFunction());
        Register(new ClearFunction());
    }
}
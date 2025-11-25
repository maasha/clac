using Clac.Core.Operations;

namespace Clac.Core.Operations;

public class DefaultOperatorRegistry : OperatorRegistry
{
    public DefaultOperatorRegistry()
    {
        Register(new AddOperator());
        Register(new SubtractOperator());
        Register(new MultiplyOperator());
        Register(new DivideOperator());
    }
}
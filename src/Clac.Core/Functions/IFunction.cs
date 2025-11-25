using DotNext;
using Clac.Core.Rpn;
namespace Clac.Core.Functions;

public interface IFunction
{
    string Name { get; }
    string Description { get; }
    Result<double> Execute(Stack stack);
}
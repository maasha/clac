using DotNext;
using Clac.Core.Rpn;
namespace Clac.Core.Commands;

public interface IStackCommand : ICommand
{
    Result<double> Execute(Stack stack);
}
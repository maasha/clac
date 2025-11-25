namespace Clac.Core.Commands;

public interface ICommand
{
    string Symbol { get; }
    string Name { get; }
    string Description { get; }
}
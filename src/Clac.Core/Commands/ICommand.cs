namespace Clac.Core.Commands;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
}
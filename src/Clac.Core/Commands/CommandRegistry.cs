using DotNext;
namespace Clac.Core.Commands;

public class CommandRegistry
{
    private readonly Dictionary<string, ICommand> _commands = [];

    public void Register(ICommand command)
    {
        _commands[command.Name] = command;
    }

    public Result<ICommand> GetCommand(string name)
    {
        if (!_commands.TryGetValue(name, out var command))
            return new Result<ICommand>(new InvalidOperationException($"Command '{name}' not found"));
        return new Result<ICommand>(command);
    }
}
using DotNext;
namespace Clac.Core.Commands;

public class CommandRegistry
{
    private readonly Dictionary<string, ICommand> _commands = [];

    public void Register(ICommand command)
    {
        _commands[command.Name.ToLowerInvariant()] = command;
    }

    public Result<ICommand> GetCommand(string commandName)
    {
        if (!_commands.TryGetValue(commandName.ToLowerInvariant(), out var command))
            return new Result<ICommand>(CommandNotFound(commandName));
        return new Result<ICommand>(command);
    }

    public Result<bool> IsValidCommand(string commandName)
    {
        if (!_commands.ContainsKey(commandName.ToLowerInvariant()))
            return new Result<bool>(CommandNotFound(commandName));
        return new Result<bool>(true);
    }

    private static InvalidOperationException CommandNotFound(string commandName)
    {
        return new InvalidOperationException($"Command '{commandName}' not found");
    }
}
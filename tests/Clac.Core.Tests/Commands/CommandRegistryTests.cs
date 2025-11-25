using Clac.Core.Commands;
namespace Clac.Core.Tests.Commands;

public class CommandRegistryTests
{
    [Fact]
    public void Register_ShouldAddCommandToRegistry()
    {
        var commandRegistry = new CommandRegistry();
        var command = new PopCommand();
        commandRegistry.Register(command);
        Assert.Equal(command.Name, commandRegistry.GetCommand(command.Name).Value.Name);
    }
}
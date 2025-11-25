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

    [Fact]
    public void GetCommand_WithUnRegisteredCommand_ShouldReturnError()
    {
        var commandRegistry = new CommandRegistry();
        var result = commandRegistry.GetCommand("clear");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Command 'clear' not found", result.Error.Message);
    }

    [Fact]
    public void IsValidCommand_WithUnRegisteredCommand_ShouldReturnFalse()
    {
        var commandRegistry = new CommandRegistry();
        var result = commandRegistry.IsValidCommand("clear");
        Assert.False(result.IsSuccessful);
        Assert.Contains("Command 'clear' not found", result.Error.Message);
    }

    [Fact]
    public void IsValidCommand_WithRegisteredCommand_ShouldReturnTrue()
    {
        var commandRegistry = new CommandRegistry();
        var command = new PopCommand();
        commandRegistry.Register(command);
        var result = commandRegistry.IsValidCommand(command.Name);
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void IsValidCommand_WithRegisteredCommand_ShouldReturnTrue_CaseInsensitive()
    {
        var commandRegistry = new CommandRegistry();
        var command = new PopCommand();
        commandRegistry.Register(command);
        var result = commandRegistry.IsValidCommand(command.Name.ToUpperInvariant());
        Assert.True(result.IsSuccessful);
    }
}
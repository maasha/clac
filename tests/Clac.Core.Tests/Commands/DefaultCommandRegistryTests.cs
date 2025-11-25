using Clac.Core.Commands;
namespace Clac.Core.Tests.Commands;

public class DefaultCommandRegistryTests
{
    [Theory]
    [InlineData("pop")]
    public void CreateDefaultCommandRegistry_ShouldHaveDefaultCommands(string commandName)
    {
        var registry = new DefaultCommandRegistry();
        var result = registry.IsValidCommand(commandName);
        Assert.True(result.IsSuccessful);
    }
}
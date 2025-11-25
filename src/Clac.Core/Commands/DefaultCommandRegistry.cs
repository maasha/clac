namespace Clac.Core.Commands;

public class DefaultCommandRegistry : CommandRegistry
{
    public DefaultCommandRegistry()
    {
        Register(new PopCommand());
    }
}
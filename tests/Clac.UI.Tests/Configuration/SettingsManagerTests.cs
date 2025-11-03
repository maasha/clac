using Xunit;
using Clac.UI.Configuration;

namespace Clac.UI.Tests.Configuration;

public class SettingsManagerTests
{
    [Fact]
    public void SettingsManager_ShouldProvideDefaultUISettings()
    {
        var settings = SettingsManager.UI;

        Assert.NotNull(settings);
        Assert.Equal(6, settings.DisplayLines);
    }
}


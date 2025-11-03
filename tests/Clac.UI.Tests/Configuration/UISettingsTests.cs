using Xunit;
using Clac.UI.Configuration;

namespace Clac.UI.Tests.Configuration;

public class UISettingsTests
{
    [Fact]
    public void UISettings_ShouldHaveDefaultDisplayLines()
    {
        // Arrange & Act
        var settings = new UISettings();

        // Assert
        Assert.Equal(4, settings.DisplayLines);
    }
}
using Xunit;
using Clac.UI.Configuration;

namespace Clac.UI.Tests.Configuration;

public class UISettingsTests
{
    [Fact]
    public void UISettings_ShouldHaveDefaultDisplayLines()
    {
        var settings = new UISettings();

        Assert.Equal(6, settings.DisplayLines);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultLineHeight()
    {
        var settings = new UISettings();

        Assert.Equal(30, settings.LineHeight);
    }
}
using Xunit;
using Clac.UI.Configuration;

namespace Clac.UI.Tests.Configuration;

public class UISettingsTests
{
    [Fact]
    public void UISettings_ShouldHaveDefaultWindowHeight()
    {
        var settings = new UISettings();

        Assert.Equal(400, settings.WindowHeight);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultWindowWidth()
    {
        var settings = new UISettings();

        Assert.Equal(400, settings.WindowWidth);
    }

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

    [Fact]
    public void UISettings_ShouldHaveDefaultBorderThickness()
    {
        var settings = new UISettings();

        Assert.Equal(2, settings.BorderThickness);
    }
}
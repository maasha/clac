using Xunit;
using Clac.UI.Configuration;

namespace Clac.UI.Tests.Configuration;

public class UISettingsTests
{
    private readonly UISettings _settings;

    public UISettingsTests()
    {
        _settings = new UISettings();
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultWindowHeight()
    {
        Assert.Equal(450, _settings.WindowHeight);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultWindowWidth()
    {
        Assert.Equal(450, _settings.WindowWidth);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultDisplayLines()
    {
        Assert.Equal(6, _settings.DisplayLines);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultLineHeight()
    {
        Assert.Equal(30, _settings.LineHeight);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultBorderThickness()
    {
        Assert.Equal(2, _settings.BorderThickness);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultWindowMinWidth()
    {
        Assert.Equal(350, _settings.WindowMinWidth);
    }

    [Fact]
    public void UISettings_ShouldHaveDefaultWindowMinHeight()
    {
        Assert.Equal(450, _settings.WindowMinHeight);
    }
}
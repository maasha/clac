namespace Clac.UI.Tests.Services;

using System;
using Xunit;
using Clac.UI.Services;
using Clac.UI.Configuration;

public class DisplayServiceTests
{
    [Fact]
    public void DisplayItems_ShouldHaveMinimumDisplayLines_WhenStackIsEmpty()
    {
        int expectedLines = SettingsManager.UI.DisplayLines;
        var service = new DisplayService(SettingsManager.UI);
        var stack = new string[] { };

        var items = service.CreateDisplayItems(stack);

        Assert.Equal(expectedLines, items.Count);
    }

    [Fact]
    public void DisplayItems_ShouldShowAllStackValues_WhenStackExceedsDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int stackSize = displayLines + 4;
        var service = new DisplayService(SettingsManager.UI);
        var stack = new string[stackSize];
        for (int i = 0; i < stackSize; i++)
            stack[i] = (i + 1).ToString();

        var items = service.CreateDisplayItems(stack);

        Assert.Equal(stackSize, items.Count);
    }

    [Fact]
    public void ShowScrollBar_ShouldBeFalse_WhenStackIsWithinDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int valuesToAdd = Math.Max(1, displayLines - 1);
        var service = new DisplayService(SettingsManager.UI);
        var stack = new string[valuesToAdd];
        for (int i = 0; i < valuesToAdd; i++)
            stack[i] = (i + 1).ToString();

        var result = service.ShouldShowScrollBar(stack);

        Assert.False(result);
    }

    [Fact]
    public void ShowScrollBar_ShouldBeTrue_WhenStackExceedsDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int stackSize = displayLines + 1;
        var service = new DisplayService(SettingsManager.UI);
        var stack = new string[stackSize];
        for (int i = 0; i < stackSize; i++)
            stack[i] = (i + 1).ToString();

        var result = service.ShouldShowScrollBar(stack);

        Assert.True(result);
    }
}
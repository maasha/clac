using System;
using Xunit;
using Avalonia.Controls.Primitives;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;

namespace Clac.UI.Tests.ViewModels;

public class CalculatorViewModelDisplayTests
{
    private readonly CalculatorViewModel _vm;

    public CalculatorViewModelDisplayTests()
    {
        _vm = new CalculatorViewModel();
    }

    [Fact]
    public void DisplayItems_ShouldHaveMinimumDisplayLines_WhenStackIsEmpty()
    {
        int expectedLines = SettingsManager.UI.DisplayLines;

        Assert.Equal(expectedLines, _vm.DisplayItems.Count);
    }

    [Fact]
    public void DisplayItems_ShouldShowAllStackValues_WhenStackExceedsDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int stackSize = displayLines + 4; // Ensure we exceed display lines

        // Add more values than display lines
        for (int i = 1; i <= stackSize; i++)
        {
            _vm.CurrentInput = i.ToString();
            _vm.Enter();
        }

        Assert.Equal(stackSize, _vm.DisplayItems.Count);
    }

    [Fact]
    public void ScrollBarVisibility_ShouldBeHidden_WhenStackIsWithinDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int valuesToAdd = Math.Max(1, displayLines - 1); // Add fewer than displayLines

        // Add values that don't exceed display lines
        for (int i = 1; i <= valuesToAdd; i++)
        {
            _vm.CurrentInput = i.ToString();
            _vm.Enter();
        }

        Assert.Equal(ScrollBarVisibility.Hidden, _vm.ScrollBarVisibility);
    }

    [Fact]
    public void ScrollBarVisibility_ShouldBeAuto_WhenStackExceedsDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int stackSize = displayLines + 1; // Exceed display lines by at least 1

        // Add more values than display lines
        for (int i = 1; i <= stackSize; i++)
        {
            _vm.CurrentInput = i.ToString();
            _vm.Enter();
        }

        Assert.Equal(ScrollBarVisibility.Auto, _vm.ScrollBarVisibility);
    }
}


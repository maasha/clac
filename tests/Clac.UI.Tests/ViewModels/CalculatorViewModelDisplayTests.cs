using System;
using Xunit;
using Avalonia.Controls.Primitives;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;

namespace Clac.UI.Tests.ViewModels;

public class CalculatorViewModelDisplayTests
{
    [Fact]
    public void DisplayItems_ShouldHaveMinimumDisplayLines_WhenStackIsEmpty()
    {
        var vm = new CalculatorViewModel();
        int expectedLines = SettingsManager.UI.DisplayLines;

        Assert.Equal(expectedLines, vm.DisplayItems.Count);
    }

    [Fact]
    public void DisplayItems_ShouldShowAllStackValues_WhenStackExceedsDisplayLines()
    {
        var vm = new CalculatorViewModel();
        int displayLines = SettingsManager.UI.DisplayLines;
        int stackSize = displayLines + 4; // Ensure we exceed display lines

        // Add more values than display lines
        for (int i = 1; i <= stackSize; i++)
        {
            vm.CurrentInput = i.ToString();
            vm.Enter();
        }

        Assert.Equal(stackSize, vm.DisplayItems.Count);
    }

    [Fact]
    public void ScrollBarVisibility_ShouldBeHidden_WhenStackIsWithinDisplayLines()
    {
        var vm = new CalculatorViewModel();
        int displayLines = SettingsManager.UI.DisplayLines;
        int valuesToAdd = Math.Max(1, displayLines - 1); // Add fewer than displayLines

        // Add values that don't exceed display lines
        for (int i = 1; i <= valuesToAdd; i++)
        {
            vm.CurrentInput = i.ToString();
            vm.Enter();
        }

        Assert.Equal(ScrollBarVisibility.Hidden, vm.ScrollBarVisibility);
    }

    [Fact]
    public void ScrollBarVisibility_ShouldBeAuto_WhenStackExceedsDisplayLines()
    {
        var vm = new CalculatorViewModel();
        int displayLines = SettingsManager.UI.DisplayLines;
        int stackSize = displayLines + 1; // Exceed display lines by at least 1

        // Add more values than display lines
        for (int i = 1; i <= stackSize; i++)
        {
            vm.CurrentInput = i.ToString();
            vm.Enter();
        }

        Assert.Equal(ScrollBarVisibility.Auto, vm.ScrollBarVisibility);
    }
}


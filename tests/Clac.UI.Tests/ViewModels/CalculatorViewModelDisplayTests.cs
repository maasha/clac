using Xunit;
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
}


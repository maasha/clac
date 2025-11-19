using System;
using Xunit;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;
using Clac.UI.Tests.Spies;
using System.IO.Abstractions;
using Clac.Core.Services;
using System.IO.Abstractions.TestingHelpers;

namespace Clac.UI.Tests.ViewModels;

public class CalculatorViewModelDisplayTests
{
    private readonly CalculatorViewModel _vm;
    private readonly PersistenceSpy persistenceSpy = new(new MockFileSystem());

    public CalculatorViewModelDisplayTests()
    {
        _vm = new CalculatorViewModel(persistenceSpy);
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
    public void ShowScrollBar_ShouldBeFalse_WhenStackIsWithinDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int valuesToAdd = Math.Max(1, displayLines - 1); // Add fewer than displayLines

        // Add values that don't exceed display lines
        for (int i = 1; i <= valuesToAdd; i++)
        {
            _vm.CurrentInput = i.ToString();
            _vm.Enter();
        }

        Assert.False(_vm.ShowScrollBar);
    }

    [Fact]
    public void ShowScrollBar_ShouldBeTrue_WhenStackExceedsDisplayLines()
    {
        int displayLines = SettingsManager.UI.DisplayLines;
        int stackSize = displayLines + 1; // Exceed display lines by at least 1

        // Add more values than display lines
        for (int i = 1; i <= stackSize; i++)
        {
            _vm.CurrentInput = i.ToString();
            _vm.Enter();
        }

        Assert.True(_vm.ShowScrollBar);
    }
}


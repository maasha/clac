namespace Clac.UI.Tests;

using Xunit;
using Clac.UI.ViewModels;

public class CalculatorViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyStackDisplay()
    {
        var viewModel = new CalculatorViewModel();
        Assert.Empty(viewModel.StackDisplay);
    }
}
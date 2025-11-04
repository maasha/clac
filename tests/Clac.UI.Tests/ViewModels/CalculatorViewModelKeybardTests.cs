namespace Clac.UI.Tests.ViewModels;

using Xunit;
using Clac.UI.ViewModels;

public class CalculatorViewModelKeyboardTests
{
    [Fact]
    public void AppendToInput_WithCharacter_ShouldAppendToCurrentInput()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "12";

        viewModel.AppendToInput("3");

        Assert.Equal("123", viewModel.CurrentInput);
    }
}
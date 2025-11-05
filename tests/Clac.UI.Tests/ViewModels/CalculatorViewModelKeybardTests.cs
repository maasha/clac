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

    [Fact]
    public void AppendToInput_WithEmptyInput_ShouldSetCurrentInput()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "";

        viewModel.AppendToInput("3");

        Assert.Equal("3", viewModel.CurrentInput);
    }

    [Fact]
    public void DeleteFromInput_WithInput_ShouldRemoveLastCharacter()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "123";

        viewModel.DeleteFromInput();

        Assert.Equal("12", viewModel.CurrentInput);
    }

    [Fact]
    public void DeleteFromInput_WithEmptyInput_ShouldDoNothing()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "";

        viewModel.DeleteFromInput();

        Assert.Equal("", viewModel.CurrentInput);
    }

    [Fact]
    public void AppendToInput_ShouldRaisePropertyChangedForCurrentInput()
    {
        var viewModel = new CalculatorViewModel();
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(CalculatorViewModel.CurrentInput))
                propertyChangedRaised = true;
        };

        viewModel.AppendToInput("1");

        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void DeleteFromInput_ShouldRaisePropertyChangedForCurrentInput()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "123";
        bool propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(CalculatorViewModel.CurrentInput))
                propertyChangedRaised = true;
        };

        viewModel.DeleteFromInput();

        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void DeleteFromInput_WithOperator_ShouldDeleteOperatorAndPrefixedWhitespace()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "1 +";

        viewModel.DeleteFromInput();

        Assert.Equal("1", viewModel.CurrentInput);
    }
}
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

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyCurrentInput()
    {
        var viewModel = new CalculatorViewModel();
        Assert.Equal("", viewModel.CurrentInput);
    }

    [Fact]
    public void CurrentInput_CanBeSetDirectly()
    {
        var viewModel = new CalculatorViewModel();

        viewModel.CurrentInput = "42";

        Assert.Equal("42", viewModel.CurrentInput);
    }

    [Fact]
    public void Enter_WithNumber_ShouldPushToStackAndClearInput()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "42";

        viewModel.Enter();

        Assert.Single(viewModel.StackDisplay);
        Assert.Equal("42", viewModel.StackDisplay[0]);
        Assert.Equal("", viewModel.CurrentInput);
    }

    [Fact]
    public void Enter_WithInvalidInput_ShouldSetErrorState()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "abc";

        viewModel.Enter();

        Assert.True(viewModel.HasError);
        Assert.Contains("Invalid", viewModel.ErrorMessage);
        Assert.Empty(viewModel.StackDisplay);
    }

    [Fact]
    public void Enter_WithEmptyInput_ShouldDoNothing()
    {
        var viewModel = new CalculatorViewModel();

        viewModel.Enter();

        Assert.False(viewModel.HasError);
        Assert.Empty(viewModel.StackDisplay);
    }
}
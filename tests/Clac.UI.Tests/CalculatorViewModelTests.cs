namespace Clac.UI.Tests;

using Xunit;
using Clac.UI.ViewModels;

public class CalculatorViewModelTests
{
    private readonly CalculatorViewModel _viewModel;

    public CalculatorViewModelTests()
    {
        _viewModel = new CalculatorViewModel();
    }

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyStackDisplay()
    {
        Assert.Empty(_viewModel.StackDisplay);
    }

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyCurrentInput()
    {
        Assert.Equal("", _viewModel.CurrentInput);
    }

    [Fact]
    public void CurrentInput_CanBeSetDirectly()
    {

        _viewModel.CurrentInput = "42";

        Assert.Equal("42", _viewModel.CurrentInput);
    }

    [Fact]
    public void Enter_WithNumber_ShouldPushToStackAndClearInput()
    {
        _viewModel.CurrentInput = "42";

        _viewModel.Enter();

        Assert.Single(_viewModel.StackDisplay);
        Assert.Equal("42", _viewModel.StackDisplay[0]);
        Assert.Equal("", _viewModel.CurrentInput);
    }

    [Fact]
    public void Enter_WithInvalidInput_ShouldSetErrorState()
    {
        _viewModel.CurrentInput = "abc";

        _viewModel.Enter();

        Assert.True(_viewModel.HasError);
        Assert.Contains("Invalid", _viewModel.ErrorMessage);
        Assert.Empty(_viewModel.StackDisplay);
    }

    [Fact]
    public void Enter_WithEmptyInput_ShouldDoNothing()
    {
        _viewModel.Enter();

        Assert.False(_viewModel.HasError);
        Assert.Empty(_viewModel.StackDisplay);
    }
}
namespace Clac.UI.Tests;

using Xunit;
using Clac.UI.ViewModels;

public class CalculatorViewModelTests
{
    private readonly CalculatorViewModel _vm;

    public CalculatorViewModelTests()
    {
        _vm = new CalculatorViewModel();
    }

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyStackDisplay()
    {
        Assert.Empty(_vm.StackDisplay);
    }

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyCurrentInput()
    {
        Assert.Equal("", _vm.CurrentInput);
    }

    [Fact]
    public void CurrentInput_CanBeSetDirectly()
    {

        _vm.CurrentInput = "42";

        Assert.Equal("42", _vm.CurrentInput);
    }

    [Fact]
    public void Enter_WithNumber_ShouldPushToStackAndClearInput()
    {
        _vm.CurrentInput = "42";

        _vm.Enter();

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("42", _vm.StackDisplay[0]);
        Assert.Equal("", _vm.CurrentInput);
    }

    [Fact]
    public void Enter_WithInvalidInput_ShouldSetErrorState()
    {
        _vm.CurrentInput = "abc";

        _vm.Enter();

        Assert.True(_vm.HasError);
        Assert.Contains("Invalid", _vm.ErrorMessage);
        Assert.Empty(_vm.StackDisplay);
    }

    [Fact]
    public void Enter_WithEmptyInput_ShouldDoNothing()
    {
        _vm.Enter();

        Assert.False(_vm.HasError);
        Assert.Empty(_vm.StackDisplay);
    }
}
namespace Clac.UI.Tests.ViewModels;

using Clac.UI.ViewModels;
using Clac.UI.Tests.Spies;
using System.IO.Abstractions;
using Clac.Core.Services;

public class CalculatorViewModelKeyboardTests
{
    private readonly CalculatorViewModel _vm;
    private static IPersistence DummyPersistence() => new PersistenceSpy(new FileSystem());

    public CalculatorViewModelKeyboardTests()
    {
        _vm = new CalculatorViewModel(DummyPersistence());
    }

    [Fact]
    public void AppendToInput_WithCharacter_ShouldAppendToCurrentInput()
    {
        _vm.CurrentInput = "12";

        _vm.AppendToInput("3");

        Assert.Equal("123", _vm.CurrentInput);
    }

    [Fact]
    public void AppendToInput_WithEmptyInput_ShouldSetCurrentInput()
    {
        _vm.CurrentInput = "";

        _vm.AppendToInput("3");

        Assert.Equal("3", _vm.CurrentInput);
    }

    [Fact]
    public void DeleteFromInput_WithInput_ShouldRemoveLastCharacter()
    {
        _vm.CurrentInput = "123";

        _vm.DeleteFromInput();

        Assert.Equal("12", _vm.CurrentInput);
    }

    [Fact]
    public void DeleteFromInput_WithEmptyInput_ShouldDoNothing()
    {
        _vm.CurrentInput = "";

        _vm.DeleteFromInput();

        Assert.Equal("", _vm.CurrentInput);
    }

    [Fact]
    public void AppendToInput_ShouldRaisePropertyChangedForCurrentInput()
    {
        bool propertyChangedRaised = false;
        _vm.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(CalculatorViewModel.CurrentInput))
                propertyChangedRaised = true;
        };

        _vm.AppendToInput("1");

        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void DeleteFromInput_ShouldRaisePropertyChangedForCurrentInput()
    {
        _vm.CurrentInput = "123";
        bool propertyChangedRaised = false;
        _vm.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(CalculatorViewModel.CurrentInput))
                propertyChangedRaised = true;
        };

        _vm.DeleteFromInput();

        Assert.True(propertyChangedRaised);
    }

    [Fact]
    public void DeleteFromInput_WithOperator_ShouldDeleteOperatorAndPrefixedWhitespace()
    {
        _vm.CurrentInput = "1 +";

        _vm.DeleteFromInput();

        Assert.Equal("1", _vm.CurrentInput);
    }
}
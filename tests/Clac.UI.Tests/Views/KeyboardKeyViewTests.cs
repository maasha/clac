namespace Clac.UI.Tests.Views;

using Xunit;
using Clac.UI.Views;
using Clac.UI.Models;
using Clac.UI.Enums;
using Clac.UI.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

public class KeyboardKeyViewTests
{
    [Fact]
    public void Button_ShouldDisplayLabel_WhenKeyboardKeyIsSetAsDataContext()
    {
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "7",
            Value = "7",
            Type = KeyType.Number
        };
        view.DataContext = key;

        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);
        Assert.Equal("7", button.Content);
    }

    [Fact]
    public void ButtonClick_ShouldAppendValueToInput_WhenKeyboardKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "7",
            Value = "7",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("7", viewModel.CurrentInput);
    }

    [Fact]
    public void DeleteKeyClick_ShouldRemoveLastCharacterFromInput_WhenDeleteKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "123";
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "DEL",
            Value = "",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("12", viewModel.CurrentInput);
    }

    [Fact]
    public void PlusKeyClick_ShouldAppendPlusToInput_WhenPlusKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "1";
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "+",
            Value = "+",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("1 +", viewModel.CurrentInput);
    }

    [Fact]
    public void PlusKeyClick_ShouldNotAddSpaceBeforeOperator_WhenInputIsEmpty()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "+",
            Value = "+",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("+", viewModel.CurrentInput);
    }
}


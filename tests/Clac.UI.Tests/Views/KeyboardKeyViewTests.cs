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

        var button = view.FindControl<Button>("Button");
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
}


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
            Value = "del()",
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
    public void DeleteKeyClick_ShouldNotChangeInput_WhenInputIsEmpty()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "DEL",
            Value = "del()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("", viewModel.CurrentInput);
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

    [Fact]
    public void MinusKeyClick_ShouldAppendMinusWithoutSpace_WhenInputIsEmpty()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "-",
            Value = "-",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("-", viewModel.CurrentInput);
    }

    [Fact]
    public void MinusKeyClick_ShouldAppendMinusWithSpace_WhenInputEndsWithNumber()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "5";
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "-",
            Value = "-",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("5 -", viewModel.CurrentInput);
    }

    [Fact]
    public void EnterKeyClick_ShouldProcessInput_WhenEnterKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "5";
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "ENTER",
            Value = "",
            Type = KeyType.Enter
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("", viewModel.CurrentInput);
        Assert.Single(viewModel.StackDisplay);
        Assert.Equal("5", viewModel.StackDisplay[0]);
    }

    [Fact]
    public void ZeroKeyClick_ShouldAppendZeroToInput_WhenZeroKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "0",
            Value = "0",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("0", viewModel.CurrentInput);
    }

    [Fact]
    public void TwoKeyClick_ShouldAppendTwoToInput_WhenTwoKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "2",
            Value = "2",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("2", viewModel.CurrentInput);
    }

    [Fact]
    public void ThreeKeyClick_ShouldAppendThreeToInput_WhenThreeKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "3",
            Value = "3",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("3", viewModel.CurrentInput);
    }

    [Fact]
    public void FourKeyClick_ShouldAppendFourToInput_WhenFourKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "4",
            Value = "4",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("4", viewModel.CurrentInput);
    }

    [Fact]
    public void FiveKeyClick_ShouldAppendFiveToInput_WhenFiveKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "5",
            Value = "5",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("5", viewModel.CurrentInput);
    }

    [Fact]
    public void SixKeyClick_ShouldAppendSixToInput_WhenSixKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "6",
            Value = "6",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("6", viewModel.CurrentInput);
    }

    [Fact]
    public void SevenKeyClick_ShouldAppendSevenToInput_WhenSevenKeyIsClicked()
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
    public void EightKeyClick_ShouldAppendEightToInput_WhenEightKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "8",
            Value = "8",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("8", viewModel.CurrentInput);
    }

    [Fact]
    public void NineKeyClick_ShouldAppendNineToInput_WhenNineKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "9",
            Value = "9",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("9", viewModel.CurrentInput);
    }

    [Fact]
    public void MultiplyKeyClick_ShouldAppendMultiplyToInput_WhenMultiplyKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "1";
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "*",
            Value = "*",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("1 *", viewModel.CurrentInput);
    }

    [Fact]
    public void MultiplyKeyClick_ShouldNotAddSpaceBeforeOperator_WhenInputIsEmpty()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "*",
            Value = "*",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("*", viewModel.CurrentInput);
    }

    [Fact]
    public void DivideKeyClick_ShouldAppendDivideToInput_WhenDivideKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "1";
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "/",
            Value = "/",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("1 /", viewModel.CurrentInput);
    }

    [Fact]
    public void DivideKeyClick_ShouldNotAddSpaceBeforeOperator_WhenInputIsEmpty()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "/",
            Value = "/",
            Type = KeyType.Operator
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal("/", viewModel.CurrentInput);
    }

    [Fact]
    public void DecimalKeyClick_ShouldAppendDecimalPointToInput_WhenDecimalKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = ".",
            Value = ".",
            Type = KeyType.Number
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal(".", viewModel.CurrentInput);
    }

    [Fact]
    public void PopKeyClick_ShouldRemoveTopItemFromStack_WhenPopKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "5";
        viewModel.Enter();
        viewModel.CurrentInput = "3";
        viewModel.Enter();

        Assert.Equal(2, viewModel.StackDisplay.Length);

        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "POP",
            Value = "pop()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(viewModel.StackDisplay);
        Assert.Equal("5", viewModel.StackDisplay[0]);
    }

    [Fact]
    public void PopKeyClick_ShouldDoNothing_WhenStackIsEmpty()
    {
        var viewModel = new CalculatorViewModel();

        Assert.Empty(viewModel.StackDisplay);

        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "POP",
            Value = "pop()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Empty(viewModel.StackDisplay);
        Assert.False(viewModel.HasError);
    }

    [Fact]
    public void SwapKeyClick_ShouldSwapTopTwoItemsOnStack_WhenSwapKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "5";
        viewModel.Enter();
        viewModel.CurrentInput = "3";
        viewModel.Enter();

        Assert.Equal(2, viewModel.StackDisplay.Length);
        Assert.Equal("5", viewModel.StackDisplay[0]);
        Assert.Equal("3", viewModel.StackDisplay[1]);

        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "SWAP",
            Value = "swap()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal(2, viewModel.StackDisplay.Length);
        Assert.Equal("3", viewModel.StackDisplay[0]);
        Assert.Equal("5", viewModel.StackDisplay[1]);
    }

    [Fact]
    public void ClearKeyClick_ShouldClearStack_WhenClearKeyIsClicked()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "5";
        viewModel.Enter();
        viewModel.CurrentInput = "3";
        viewModel.Enter();

        Assert.Equal(2, viewModel.StackDisplay.Length);

        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "CLEAR",
            Value = "clear()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Empty(viewModel.StackDisplay);
    }

    [Fact]
    public void ClearKeyClick_ShouldClearStackWithoutError_WhenInputContainsNumber()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "5";
        viewModel.Enter();
        viewModel.CurrentInput = "3";
        viewModel.Enter();
        viewModel.CurrentInput = "43";

        Assert.Equal(2, viewModel.StackDisplay.Length);
        Assert.False(viewModel.HasError);

        var parent = new UserControl { DataContext = viewModel };
        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "CLEAR",
            Value = "clear()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Empty(viewModel.StackDisplay);
        Assert.False(viewModel.HasError);
    }
}


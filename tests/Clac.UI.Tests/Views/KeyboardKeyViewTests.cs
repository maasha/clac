namespace Clac.UI.Tests.Views;

using Xunit;
using Clac.UI.Views;
using Clac.UI.Models;
using Clac.UI.Enums;
using Clac.UI.ViewModels;
using Clac.Core;
using Avalonia.Controls;
using Avalonia.Interactivity;

public class KeyboardKeyViewTests
{
    private readonly CalculatorViewModel _vm;
    private readonly KeyboardKeyView _view;

    public KeyboardKeyViewTests()
    {
        _vm = new CalculatorViewModel();
        _view = new KeyboardKeyView();
    }

    [Fact]
    public void Button_ShouldDisplayLabel_WhenKeyboardKeyIsSetAsDataContext()
    {
        var key = new KeyboardKey
        {
            Label = "7",
            Value = "7",
            Type = KeyType.Number
        };
        _view.DataContext = key;

        _view.InitializeComponent();

        var button = _view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);
        Assert.Equal("7", button.Content);
    }

    [Fact]
    public void ButtonClick_ShouldAppendValueToInput_WhenKeyboardKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("7", _vm.CurrentInput);
    }

    [Fact]
    public void DeleteKeyClick_ShouldRemoveLastCharacterFromInput_WhenDeleteKeyIsClicked()
    {
        _vm.CurrentInput = "123";
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("12", _vm.CurrentInput);
    }

    [Fact]
    public void DeleteKeyClick_ShouldNotChangeInput_WhenInputIsEmpty()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("", _vm.CurrentInput);
    }

    [Fact]
    public void PlusKeyClick_ShouldAppendPlusToInput_WhenPlusKeyIsClicked()
    {
        _vm.CurrentInput = "1";
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("1 +", _vm.CurrentInput);
    }

    [Fact]
    public void PlusKeyClick_ShouldNotAddSpaceBeforeOperator_WhenInputIsEmpty()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("+", _vm.CurrentInput);
    }

    [Fact]
    public void MinusKeyClick_ShouldAppendMinusWithoutSpace_WhenInputIsEmpty()
    {

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("-", _vm.CurrentInput);
    }

    [Fact]
    public void MinusKeyClick_ShouldAppendMinusWithSpace_WhenInputEndsWithNumber()
    {
        _vm.CurrentInput = "5";
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("5 -", _vm.CurrentInput);
    }

    [Fact]
    public void EnterKeyClick_ShouldProcessInput_WhenEnterKeyIsClicked()
    {
        _vm.CurrentInput = "5";
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("", _vm.CurrentInput);
        Assert.Single(_vm.StackDisplay);
        Assert.Equal("5", _vm.StackDisplay[0]);
    }

    [Fact]
    public void ZeroKeyClick_ShouldAppendZeroToInput_WhenZeroKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("0", _vm.CurrentInput);
    }

    [Fact]
    public void TwoKeyClick_ShouldAppendTwoToInput_WhenTwoKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("2", _vm.CurrentInput);
    }

    [Fact]
    public void ThreeKeyClick_ShouldAppendThreeToInput_WhenThreeKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("3", _vm.CurrentInput);
    }

    [Fact]
    public void FourKeyClick_ShouldAppendFourToInput_WhenFourKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("4", _vm.CurrentInput);
    }

    [Fact]
    public void FiveKeyClick_ShouldAppendFiveToInput_WhenFiveKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("5", _vm.CurrentInput);
    }

    [Fact]
    public void SixKeyClick_ShouldAppendSixToInput_WhenSixKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("6", _vm.CurrentInput);
    }

    [Fact]
    public void SevenKeyClick_ShouldAppendSevenToInput_WhenSevenKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("7", _vm.CurrentInput);
    }

    [Fact]
    public void EightKeyClick_ShouldAppendEightToInput_WhenEightKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("8", _vm.CurrentInput);
    }

    [Fact]
    public void NineKeyClick_ShouldAppendNineToInput_WhenNineKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("9", _vm.CurrentInput);
    }

    [Fact]
    public void MultiplyKeyClick_ShouldAppendMultiplyToInput_WhenMultiplyKeyIsClicked()
    {
        _vm.CurrentInput = "1";
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("1 *", _vm.CurrentInput);
    }

    [Fact]
    public void MultiplyKeyClick_ShouldNotAddSpaceBeforeOperator_WhenInputIsEmpty()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("*", _vm.CurrentInput);
    }

    [Fact]
    public void DivideKeyClick_ShouldAppendDivideToInput_WhenDivideKeyIsClicked()
    {
        _vm.CurrentInput = "1";
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("1 /", _vm.CurrentInput);
    }

    [Fact]
    public void DivideKeyClick_ShouldNotAddSpaceBeforeOperator_WhenInputIsEmpty()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal("/", _vm.CurrentInput);
    }

    [Fact]
    public void DecimalKeyClick_ShouldAppendDecimalPointToInput_WhenDecimalKeyIsClicked()
    {
        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal(".", _vm.CurrentInput);
    }

    [Fact]
    public void PopKeyClick_ShouldRemoveTopItemFromStack_WhenPopKeyIsClicked()
    {
        _vm.CurrentInput = "5";
        _vm.Enter();
        _vm.CurrentInput = "3";
        _vm.Enter();

        Assert.Equal(2, _vm.StackDisplay.Length);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("5", _vm.StackDisplay[0]);
    }

    [Fact]
    public void PopKeyClick_ShouldDoNothing_WhenStackIsEmpty()
    {
        Assert.Empty(_vm.StackDisplay);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Empty(_vm.StackDisplay);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SwapKeyClick_ShouldSwapTopTwoItemsOnStack_WhenSwapKeyIsClicked()
    {

        _vm.CurrentInput = "5";
        _vm.Enter();
        _vm.CurrentInput = "3";
        _vm.Enter();

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.Equal("5", _vm.StackDisplay[0]);
        Assert.Equal("3", _vm.StackDisplay[1]);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.Equal("3", _vm.StackDisplay[0]);
        Assert.Equal("5", _vm.StackDisplay[1]);
    }

    [Fact]
    public void ClearKeyClick_ShouldClearStack_WhenClearKeyIsClicked()
    {

        _vm.CurrentInput = "5";
        _vm.Enter();
        _vm.CurrentInput = "3";
        _vm.Enter();

        Assert.Equal(2, _vm.StackDisplay.Length);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Empty(_vm.StackDisplay);
    }

    [Fact]
    public void ClearKeyClick_ShouldClearStackWithoutError_WhenInputContainsNumber()
    {

        _vm.CurrentInput = "5";
        _vm.Enter();
        _vm.CurrentInput = "3";
        _vm.Enter();
        _vm.CurrentInput = "43";

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.False(_vm.HasError);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
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

        Assert.Empty(_vm.StackDisplay);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SumKeyClick_ShouldSumAllItemsInStackAndReplaceWithSum_WhenSumKeyIsClicked()
    {

        _vm.CurrentInput = "1";
        _vm.Enter();
        _vm.CurrentInput = "2";
        _vm.Enter();
        _vm.CurrentInput = "3";
        _vm.Enter();

        Assert.Equal(3, _vm.StackDisplay.Length);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "Σ",
            Value = "sum()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("6", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SumKeyClick_ShouldDoNothing_WhenStackIsEmpty()
    {


        Assert.Empty(_vm.StackDisplay);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "Σ",
            Value = "sum()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Empty(_vm.StackDisplay);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SumKeyClick_ShouldSumSingleItem_WhenStackHasOneItem()
    {

        _vm.CurrentInput = "5";
        _vm.Enter();

        Assert.Single(_vm.StackDisplay);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "Σ",
            Value = "sum()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("5", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SumKeyClick_ShouldSumStackWithoutError_WhenInputContainsNumber()
    {

        _vm.CurrentInput = "1";
        _vm.Enter();
        _vm.CurrentInput = "2";
        _vm.Enter();
        _vm.CurrentInput = "43";

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.False(_vm.HasError);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "Σ",
            Value = "sum()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("46", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SqrtKeyClick_ShouldCalculateSquareRootOfLastItem_WhenSqrtKeyIsClicked()
    {

        _vm.CurrentInput = "4";
        _vm.Enter();

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("4", _vm.StackDisplay[0]);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "√",
            Value = "sqrt()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("2", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SqrtKeyClick_ShouldDoNothing_WhenStackIsEmpty()
    {


        Assert.Empty(_vm.StackDisplay);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "√",
            Value = "sqrt()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Empty(_vm.StackDisplay);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void SqrtKeyClick_ShouldShowError_WhenLastItemIsNegative()
    {

        _vm.CurrentInput = "-1";
        _vm.Enter();

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("-1", _vm.StackDisplay[0]);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "√",
            Value = "sqrt()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("-1", _vm.StackDisplay[0]);
        Assert.True(_vm.HasError);
        Assert.Contains(ErrorMessages.InvalidNegativeSquareRoot, _vm.ErrorMessage);
    }

    [Fact]
    public void SqrtKeyClick_ShouldCalculateSquareRootWithoutError_WhenInputContainsNumber()
    {

        _vm.CurrentInput = "9";
        _vm.Enter();
        _vm.CurrentInput = "16";

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("9", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "√",
            Value = "sqrt()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.Equal("9", _vm.StackDisplay[0]);
        Assert.Equal("4", _vm.StackDisplay[1]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void PowKeyClick_ShouldCalculatePowerOfLastTwoNumbers_WhenPowKeyIsClicked()
    {

        _vm.CurrentInput = "2";
        _vm.Enter();
        _vm.CurrentInput = "3";
        _vm.Enter();

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.Equal("2", _vm.StackDisplay[0]);
        Assert.Equal("3", _vm.StackDisplay[1]);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "xʸ",
            Value = "pow()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("8", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void PowKeyClick_ShouldDoNothing_WhenStackHasLessThanTwoNumbers()
    {

        _vm.CurrentInput = "2";
        _vm.Enter();

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("2", _vm.StackDisplay[0]);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "xʸ",
            Value = "pow()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("2", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void PowKeyClick_ShouldCalculatePowerWithoutError_WhenInputContainsNumber()
    {

        _vm.CurrentInput = "2";
        _vm.Enter();
        _vm.CurrentInput = "3";

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("2", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "xʸ",
            Value = "pow()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("8", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void ReciprocalKeyClick_ShouldCalculateReciprocalOfLastItem_WhenReciprocalKeyIsClicked()
    {

        _vm.CurrentInput = "4";
        _vm.Enter();

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("4", _vm.StackDisplay[0]);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "1/x",
            Value = "reciprocal()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("0.25", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void ReciprocalKeyClick_ShouldDoNothing_WhenStackIsEmpty()
    {


        Assert.Empty(_vm.StackDisplay);
        Assert.False(_vm.HasError);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "1/x",
            Value = "reciprocal()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Empty(_vm.StackDisplay);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void ReciprocalKeyClick_ShouldShowError_WhenLastItemIsZero()
    {

        _vm.CurrentInput = "0";
        _vm.Enter();

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("0", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "1/x",
            Value = "reciprocal()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("0", _vm.StackDisplay[0]);
        Assert.True(_vm.HasError);
        Assert.Contains(ErrorMessages.DivisionByZero, _vm.ErrorMessage);
    }

    [Fact]
    public void ReciprocalKeyClick_ShouldCalculateReciprocalWithoutError_WhenInputContainsNumber()
    {

        _vm.CurrentInput = "4";
        _vm.Enter();
        _vm.CurrentInput = "8";

        Assert.Single(_vm.StackDisplay);
        Assert.Equal("4", _vm.StackDisplay[0]);
        Assert.False(_vm.HasError);

        var parent = new UserControl { DataContext = _vm };
        var view = _view;
        var key = new KeyboardKey
        {
            Label = "1/x",
            Value = "reciprocal()",
            Type = KeyType.Command
        };
        view.DataContext = key;
        parent.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.Equal("4", _vm.StackDisplay[0]);
        Assert.Equal("0.125", _vm.StackDisplay[1]);
        Assert.False(_vm.HasError);
    }

    [Fact]
    public void ButtonClick_ShouldNotCrash_WhenParentChainExceedsMaxDepth()
    {
        var viewModel = new CalculatorViewModel();
        viewModel.CurrentInput = "123";

        var topParent = new UserControl { DataContext = viewModel };
        var current = topParent;

        for (int i = 0; i < 100; i++)
        {
            var next = new UserControl();
            current.Content = next;
            current = next;
        }

        var view = new KeyboardKeyView();
        var key = new KeyboardKey
        {
            Label = "7",
            Value = "7",
            Type = KeyType.Number
        };
        view.DataContext = key;
        current.Content = view;
        view.InitializeComponent();

        var button = view.FindControl<Button>("KeyButton");
        Assert.NotNull(button);

        var originalInput = viewModel.CurrentInput;

        var exception = Record.Exception(() => button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)));

        Assert.Null(exception);
    }
}


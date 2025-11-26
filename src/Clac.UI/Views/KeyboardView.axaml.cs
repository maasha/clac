using Avalonia.Controls;
using Clac.UI.Enums;
using Clac.UI.Models;
using Clac.UI.ViewModels;

namespace Clac.UI.Views;

public partial class KeyboardView : UserControl
{
    public KeyboardView()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;

        SqrtKeyView.DataContext = new KeyboardKey { Label = "√", Value = "sqrt()", Type = KeyType.Command };
        PowKeyView.DataContext = new KeyboardKey { Label = "xʸ", Value = "pow()", Type = KeyType.Command };
        recipKeyView.DataContext = new KeyboardKey { Label = "1/x", Value = "recip()", Type = KeyType.Command };
        DivideKeyView.DataContext = new KeyboardKey { Label = "/", Value = "/", Type = KeyType.Operator };
        DeleteKeyView.DataContext = new KeyboardKey { Label = "DEL", Value = "del()", Type = KeyType.Command };

        Key7View.DataContext = new KeyboardKey { Label = "7", Value = "7", Type = KeyType.Number };
        Key8View.DataContext = new KeyboardKey { Label = "8", Value = "8", Type = KeyType.Number };
        Key9View.DataContext = new KeyboardKey { Label = "9", Value = "9", Type = KeyType.Number };
        MultiplyKeyView.DataContext = new KeyboardKey { Label = "*", Value = "*", Type = KeyType.Operator };
        ClearKeyView.DataContext = new KeyboardKey { Label = "CLEAR", Value = "clear()", Type = KeyType.Command };

        Key4View.DataContext = new KeyboardKey { Label = "4", Value = "4", Type = KeyType.Number };
        Key5View.DataContext = new KeyboardKey { Label = "5", Value = "5", Type = KeyType.Number };
        Key6View.DataContext = new KeyboardKey { Label = "6", Value = "6", Type = KeyType.Number };
        MinusKeyView.DataContext = new KeyboardKey { Label = "-", Value = "-", Type = KeyType.Operator };
        UndoKeyView.DataContext = new KeyboardKey { Label = "UNDO", Value = "undo()", Type = KeyType.Command };

        Key1View.DataContext = new KeyboardKey { Label = "1", Value = "1", Type = KeyType.Number };
        Key2View.DataContext = new KeyboardKey { Label = "2", Value = "2", Type = KeyType.Number };
        Key3View.DataContext = new KeyboardKey { Label = "3", Value = "3", Type = KeyType.Number };
        PlusKeyView.DataContext = new KeyboardKey { Label = "+", Value = "+", Type = KeyType.Operator };
        PopKeyView.DataContext = new KeyboardKey { Label = "POP", Value = "pop()", Type = KeyType.Command };

        Key0View.DataContext = new KeyboardKey { Label = "0", Value = "0", Type = KeyType.Number };
        DecimalKeyView.DataContext = new KeyboardKey { Label = ".", Value = ".", Type = KeyType.Number };
        SumKeyView.DataContext = new KeyboardKey { Label = "Σ", Value = "sum()", Type = KeyType.Command };
        SwapKeyView.DataContext = new KeyboardKey { Label = "SWAP", Value = "swap()", Type = KeyType.Command };

        EnterKeyView.DataContext = new KeyboardKey { Label = "ENTER", Value = "", Type = KeyType.Enter };
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is CalculatorViewModel viewModel)
        {
            SetViewModelOnAllKeys(viewModel);
        }
    }

    private void SetViewModelOnAllKeys(CalculatorViewModel viewModel)
    {
        SqrtKeyView.ViewModel = viewModel;
        PowKeyView.ViewModel = viewModel;
        recipKeyView.ViewModel = viewModel;
        DivideKeyView.ViewModel = viewModel;
        DeleteKeyView.ViewModel = viewModel;
        Key7View.ViewModel = viewModel;
        Key8View.ViewModel = viewModel;
        Key9View.ViewModel = viewModel;
        MultiplyKeyView.ViewModel = viewModel;
        ClearKeyView.ViewModel = viewModel;
        Key4View.ViewModel = viewModel;
        Key5View.ViewModel = viewModel;
        Key6View.ViewModel = viewModel;
        MinusKeyView.ViewModel = viewModel;
        UndoKeyView.ViewModel = viewModel;
        Key1View.ViewModel = viewModel;
        Key2View.ViewModel = viewModel;
        Key3View.ViewModel = viewModel;
        PlusKeyView.ViewModel = viewModel;
        PopKeyView.ViewModel = viewModel;
        Key0View.ViewModel = viewModel;
        DecimalKeyView.ViewModel = viewModel;
        SumKeyView.ViewModel = viewModel;
        SwapKeyView.ViewModel = viewModel;
        EnterKeyView.ViewModel = viewModel;
    }
}

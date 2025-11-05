using Avalonia.Controls;
using Clac.UI.Enums;
using Clac.UI.Models;

namespace Clac.UI.Views;

/// <summary>
/// View for the keyboard. This is where the user can press the buttons to enter
/// the input in the form of numbers and operators.
/// </summary>
public partial class KeyboardView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the KeyboardView class.
    /// </summary>
    public KeyboardView()
    {
        InitializeComponent();

        // Row 1: √, xʸ, 1/x, /, DEL
        SqrtKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Function };
        PowerKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Function };
        ReciprocalKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Function };
        DivideKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Operator };
        DeleteKeyView.DataContext = new KeyboardKey { Label = "DEL", Value = "", Type = KeyType.Command };

        // Row 2: 7, 8, 9, x, CLEAR
        Key7View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        Key8View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        Key9View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        MultiplyKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Operator };
        ClearKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Command };

        // Row 3: 4, 5, 6, -, UNDO
        Key4View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        Key5View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        Key6View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        MinusKeyView.DataContext = new KeyboardKey { Label = "-", Value = "-", Type = KeyType.Operator };
        UndoKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Command };

        // Row 4: 1, 2, 3, +, POP
        Key1View.DataContext = new KeyboardKey { Label = "1", Value = "1", Type = KeyType.Number };
        Key2View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        Key3View.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        PlusKeyView.DataContext = new KeyboardKey { Label = "+", Value = "+", Type = KeyType.Operator };
        PopKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Command };

        // Row 5: 0 (spans two columns), ., Σ, SWAP
        Key0View.DataContext = new KeyboardKey { Label = "0", Value = "0", Type = KeyType.Number };
        DecimalKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Number };
        SumKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Function };
        SwapKeyView.DataContext = new KeyboardKey { Label = "D", Value = "", Type = KeyType.Command };

        // Row 6: ENTER (spans all columns)
        EnterKeyView.DataContext = new KeyboardKey { Label = "ENTER", Value = "", Type = KeyType.Enter };
    }
}
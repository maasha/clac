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

        // Create the "1" key
        var key1 = new KeyboardKey
        {
            Label = "1",
            Value = "1",
            Type = KeyType.Number
        };

        // Set DataContext after InitializeComponent
        Key1View.DataContext = key1;

        // Verify it was set
        if (Key1View.DataContext is KeyboardKey key)
        {
            System.Diagnostics.Debug.WriteLine($"Key1View DataContext set: Label={key.Label}");
        }
    }
}
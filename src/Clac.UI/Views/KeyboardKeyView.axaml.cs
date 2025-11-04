using Avalonia.Controls;
using Avalonia.Input;
using Clac.UI.ViewModels;

namespace Clac.UI.Views;

public partial class KeyboardKeyView : UserControl
{
    public KeyboardKeyView()
    {
        InitializeComponent();
    }

    private void OnKeyPressed(object? sender, PointerPressedEventArgs e)
    {
        // Get the KeyboardKey from our DataContext
        if (this.DataContext is not Clac.UI.Models.KeyboardKey key)
            return;

        // Get the CalculatorViewModel from parent's DataContext
        var parent = this.Parent;
        while (parent != null && parent.DataContext is not CalculatorViewModel)
        {
            parent = parent.Parent;
        }

        if (parent?.DataContext is CalculatorViewModel viewModel)
        {
            viewModel.AppendToInput(key.Value);
        }

        // Visual feedback for press
        if (KeyBorder != null)
        {
            KeyBorder.Background = Avalonia.Media.Brushes.DarkGray;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (KeyBorder != null)
        {
            KeyBorder.Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#363636"));
        }
    }
}
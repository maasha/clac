using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Clac.UI.ViewModels;
using Clac.UI.Models;

namespace Clac.UI.Views;

public partial class KeyboardKeyView : UserControl
{
    public KeyboardKeyView()
    {
        InitializeComponent();
    }

    private void OnKeyButtonClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not KeyboardKey key)
            return;

        var viewModel = FindCalculatorViewModel();
        if (viewModel != null)
        {
            viewModel.AppendToInput(key.Value);
        }
    }

    private CalculatorViewModel? FindCalculatorViewModel()
    {
        var current = Parent;
        while (current != null)
        {
            if (current.DataContext is CalculatorViewModel viewModel)
                return viewModel;
            current = current.Parent;
        }
        return null;
    }
}
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Clac.UI.ViewModels;
using Clac.UI.Models;
using Clac.UI.Enums;

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
            if (key.Type == KeyType.Command)
            {
                if (key.Value == "del()")
                {
                    viewModel.DeleteFromInput();
                }
                else
                {
                    var prefix = string.IsNullOrWhiteSpace(viewModel.CurrentInput) ? "" : " ";
                    viewModel.AppendToInput(prefix + key.Value);
                    viewModel.Enter();
                }
            }
            else if (key.Type == KeyType.Enter)
            {
                viewModel.Enter();
            }
            else if (key.Type == KeyType.Operator)
            {
                var prefix = string.IsNullOrWhiteSpace(viewModel.CurrentInput) ? "" : " ";
                viewModel.AppendToInput(prefix + key.Value);
            }
            else
            {
                viewModel.AppendToInput(key.Value);
            }
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

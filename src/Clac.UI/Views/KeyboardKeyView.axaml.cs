using Avalonia.Controls;
using Avalonia.Interactivity;
using Clac.UI.ViewModels;
using Clac.UI.Models;
using Clac.UI.Enums;

namespace Clac.UI.Views;

public partial class KeyboardKeyView : UserControl
{
    private const string DeleteCommand = "del()";

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
                HandleCommandKey(key, viewModel);
            }
            else if (key.Type == KeyType.Enter)
            {
                viewModel.Enter();
            }
            else if (key.Type == KeyType.Operator)
            {
                HandleOperatorKey(key, viewModel);
            }
            else
            {
                viewModel.AppendToInput(key.Value);
            }
        }
    }

    private void HandleCommandKey(KeyboardKey key, CalculatorViewModel viewModel)
    {
        if (key.Value == DeleteCommand)
        {
            viewModel.DeleteFromInput();
        }
        else
        {
            var prefix = GetOperatorPrefix(viewModel);
            viewModel.AppendToInput(prefix + key.Value);
            viewModel.Enter();
        }
    }

    private void HandleOperatorKey(KeyboardKey key, CalculatorViewModel viewModel)
    {
        var prefix = GetOperatorPrefix(viewModel);
        viewModel.AppendToInput(prefix + key.Value);
    }

    private static string GetOperatorPrefix(CalculatorViewModel viewModel)
    {
        return string.IsNullOrWhiteSpace(viewModel.CurrentInput) ? "" : " ";
    }

    private CalculatorViewModel? FindCalculatorViewModel()
    {
        const int maxDepth = 50;
        var current = Parent;
        int depth = 0;

        while (current != null && depth < maxDepth)
        {
            if (current.DataContext is CalculatorViewModel viewModel)
                return viewModel;
            current = current.Parent;
            depth++;
        }

        return null;
    }
}

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
            switch (key.Type)
            {
                case KeyType.Command:
                    HandleCommandKey(key, viewModel);
                    break;
                case KeyType.Enter:
                    viewModel.Enter();
                    break;
                case KeyType.Operator:
                    HandleOperatorKey(key, viewModel);
                    break;
                default:
                    viewModel.AppendToInput(key.Value);
                    break;
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

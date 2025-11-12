using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Clac.UI.ViewModels;
using Clac.UI.Models;
using Clac.UI.Enums;

namespace Clac.UI.Views;

public partial class KeyboardKeyView : UserControl
{
    private const string DeleteCommand = "del()";

    private static readonly Dictionary<KeyType, Action<KeyboardKey, CalculatorViewModel>> KeyHandlers = new()
    {
        { KeyType.Command, HandleCommandKey },
        { KeyType.Enter, HandleEnterKey },
        { KeyType.Operator, HandleOperatorKey },
        { KeyType.Number, HandleNumberKey },
        { KeyType.Function, HandleFunctionKey }
    };

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
            if (KeyHandlers.TryGetValue(key.Type, out var handler))
                handler(key, viewModel);
            else
                viewModel.AppendToInput(key.Value);
        }
    }

    private static void HandleCommandKey(KeyboardKey key, CalculatorViewModel viewModel)
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

    private static void HandleEnterKey(KeyboardKey key, CalculatorViewModel viewModel)
    {
        viewModel.Enter();
    }

    private static void HandleOperatorKey(KeyboardKey key, CalculatorViewModel viewModel)
    {
        var prefix = GetOperatorPrefix(viewModel);
        viewModel.AppendToInput(prefix + key.Value);
    }

    private static void HandleNumberKey(KeyboardKey key, CalculatorViewModel viewModel)
    {
        viewModel.AppendToInput(key.Value);
    }

    private static void HandleFunctionKey(KeyboardKey key, CalculatorViewModel viewModel)
    {
        viewModel.AppendToInput(key.Value);
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

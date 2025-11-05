using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Clac.UI.ViewModels;
using Clac.UI.Models;

namespace Clac.UI.Views;

/// <summary>
/// View for a single keyboard key.
/// </summary>
public partial class KeyboardKeyView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the KeyboardKeyView class.
    /// </summary>
    public KeyboardKeyView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the key button click event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments.</param>
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

    /// <summary>
    /// Finds the CalculatorViewModel in the parent hierarchy.
    /// </summary>
    /// <returns>The CalculatorViewModel if found, otherwise null.</returns>
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
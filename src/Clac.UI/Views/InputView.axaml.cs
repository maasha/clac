using Avalonia.Controls;
using Avalonia.Input;
using Clac.UI.ViewModels;

namespace Clac.UI.Views;

/// <summary>
/// View for the input field. This is where the user enters the input in the
/// form of numbers and operators.
/// </summary>
public partial class InputView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the InputView class.
    /// </summary>
    public InputView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the KeyDown event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void OnInputKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && DataContext is CalculatorViewModel viewModel)
        {
            viewModel.Enter();
        }
    }
}
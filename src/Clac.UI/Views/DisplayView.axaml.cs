using Avalonia.Controls;
using Clac.UI.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace Clac.UI.Views;

/// <summary>
/// View for displaying the stack in the display.
/// </summary>
public partial class DisplayView : UserControl
{
    private CalculatorViewModel? _viewModel;

    /// <summary>
    /// Initializes a new instance of the DisplayView class.
    /// </summary>
    public DisplayView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    /// <summary>
    /// Event handler for the DataContextChanged event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments.</param>

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        // Unsubscribe from old ViewModel
        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        // Subscribe to new ViewModel
        if (DataContext is CalculatorViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
            UpdateDisplay(viewModel.StackDisplay);
        }
    }

    /// <summary>
    /// Event handler for the PropertyChanged event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CalculatorViewModel.StackDisplay) && _viewModel != null)
        {
            UpdateDisplay(_viewModel.StackDisplay);
        }
    }

    /// <summary>
    /// Updates the display with the new stack.
    /// </summary>
    /// <param name="stack">The new stack.</param>
    private void UpdateDisplay(string[] stack)
    {
        // Get the last 4 items from the stack (or fewer if stack is smaller)
        var displayItems = stack.TakeLast(4).ToArray();

        // HP48 style: Line 1 is top of stack, Line 4 is 4th from top
        Line1.Text = FormatLine(1, displayItems.Length >= 1 ? displayItems[^1] : null);
        Line2.Text = FormatLine(2, displayItems.Length >= 2 ? displayItems[^2] : null);
        Line3.Text = FormatLine(3, displayItems.Length >= 3 ? displayItems[^3] : null);
        Line4.Text = FormatLine(4, displayItems.Length >= 4 ? displayItems[^4] : null);
    }

    /// <summary>
    /// Formats a line of the display.
    /// </summary>
    /// <param name="lineNumber">The line number.</param>
    /// <param name="value">The value to format.</param>
    /// <returns>The formatted line.</returns>
    private string FormatLine(int lineNumber, string? value)
    {
        return value != null ? $"{lineNumber}: {value}" : $"{lineNumber}:";
    }
}
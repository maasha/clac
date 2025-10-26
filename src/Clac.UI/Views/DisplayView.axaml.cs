using Avalonia.Controls;
using Clac.UI.ViewModels;
using Clac.UI.Helpers;
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
        int stackSize = stack.Length;

        // Get non-empty values for calculating max integer part length
        var visibleValues = displayItems.Where(v => !string.IsNullOrEmpty(v)).ToArray();
        int maxIntegerPartLength = visibleValues.Length > 0
            ? DisplayFormatter.GetMaxIntegerPartLength(visibleValues)
            : 0;

        // HP48 style: Line 1 is top of stack, Line 4 is 4th from top
        SetLine(LineNumber1, Line1, 1, stackSize, displayItems.Length >= 1 ? displayItems[^1] : null, maxIntegerPartLength);
        SetLine(LineNumber2, Line2, 2, stackSize, displayItems.Length >= 2 ? displayItems[^2] : null, maxIntegerPartLength);
        SetLine(LineNumber3, Line3, 3, stackSize, displayItems.Length >= 3 ? displayItems[^3] : null, maxIntegerPartLength);
        SetLine(LineNumber4, Line4, 4, stackSize, displayItems.Length >= 4 ? displayItems[^4] : null, maxIntegerPartLength);
    }

    /// <summary>
    /// Sets the line number and value for a display line.
    /// </summary>
    /// <param name="lineNumberBlock">The TextBlock for the line number.</param>
    /// <param name="valueBlock">The TextBlock for the value.</param>
    /// <param name="lineNum">The line number.</param>
    /// <param name="maxLineNum">The maximum line number (stack size).</param>
    /// <param name="value">The value to display.</param>
    /// <param name="maxIntegerPartLength">The maximum integer part length.</param>
    private void SetLine(TextBlock lineNumberBlock, TextBlock valueBlock, int lineNum, int maxLineNum, string? value, int maxIntegerPartLength)
    {
        lineNumberBlock.Text = DisplayFormatter.FormatLineNumber(lineNum, maxLineNum);
        valueBlock.Text = value != null ? DisplayFormatter.FormatValue(value, maxIntegerPartLength) : "";
    }
}
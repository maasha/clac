using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Clac.UI.ViewModels;
using Clac.UI.Helpers;
using Clac.UI.Configuration;
using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace Clac.UI.Views;

/// <summary>
/// Represents a single line item in the stack display.
/// </summary>
public class StackLineItem
{
    public string LineNumber { get; set; } = "";
    public string FormattedValue { get; set; } = "";
}

/// <summary>
/// View for displaying the stack in the display.
/// </summary>
public partial class DisplayView : UserControl
{
    private const int LineHeight = 30;
    private const int DisplayBorderThickness = 4; // 2px top + 2px bottom
    private CalculatorViewModel? _viewModel;

    /// <summary>
    /// Initializes a new instance of the DisplayView class.
    /// </summary>
    public DisplayView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;

        // Set display height based on configured number of lines
        int displayLines = SettingsManager.UI.DisplayLines;
        DisplayBorder.Height = (displayLines * LineHeight) + DisplayBorderThickness;
    }

    /// <summary>
    /// Event handler for the DataContextChanged event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments.</param>

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

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
        int displayLines = SettingsManager.UI.DisplayLines;

        var visibleValues = stack.Where(v => !string.IsNullOrEmpty(v)).ToArray();
        int maxIntegerPartLength = visibleValues.Length > 0
            ? DisplayFormatter.GetMaxIntegerPartLength(visibleValues)
            : 0;

        var items = new List<StackLineItem>();
        
        // Determine how many lines to show: at least displayLines, or all stack items if more
        int totalLines = Math.Max(displayLines, stack.Length);

        for (int lineNum = totalLines; lineNum >= 1; lineNum--)
        {
            // Calculate which stack position this line should display
            int stackIndex = stack.Length - lineNum;
            string value = stackIndex >= 0 ? stack[stackIndex] : "";

            items.Add(new StackLineItem
            {
                LineNumber = DisplayFormatter.FormatLineNumber(lineNum, totalLines),
                FormattedValue = string.IsNullOrEmpty(value)
                    ? ""
                    : DisplayFormatter.FormatValue(value, maxIntegerPartLength)
            });
        }

        StackItemsControl.ItemsSource = items;

        // Show scrollbar only if stack has more values than display lines
        StackScrollViewer.VerticalScrollBarVisibility = stack.Length > displayLines
            ? ScrollBarVisibility.Auto
            : ScrollBarVisibility.Hidden;

        // Scroll to bottom to show the top of stack (Line 1)
        Dispatcher.UIThread.Post(() =>
        {
            StackScrollViewer.ScrollToEnd();
        }, DispatcherPriority.Loaded);
    }
}
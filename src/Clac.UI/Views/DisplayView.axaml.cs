using Avalonia.Controls;
using Avalonia.Threading;
using Clac.UI.Configuration;
using Clac.UI.ViewModels;
using System.Collections.Specialized;

namespace Clac.UI.Views;

/// <summary>
/// View for displaying the stack in the display.
/// </summary>
public partial class DisplayView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the DisplayView class.
    /// </summary>
    public DisplayView()
    {
        InitializeComponent();

        int displayLines = SettingsManager.UI.DisplayLines;
        int lineHeight = SettingsManager.UI.LineHeight;
        int borderThickness = SettingsManager.UI.BorderThickness;
        DisplayBorder.Height = (displayLines * lineHeight) + (borderThickness * 2); // top + bottom border

        // Subscribe to DataContext changes to wire up auto-scroll
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is CalculatorViewModel viewModel)
        {
            // Subscribe to collection changes to auto-scroll
            viewModel.DisplayItems.CollectionChanged += OnDisplayItemsChanged;
        }
    }

    private void OnDisplayItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Scroll to bottom to show the top of stack (Line 1)
        Dispatcher.UIThread.Post(() =>
        {
            StackScrollViewer.ScrollToEnd();
        }, DispatcherPriority.Loaded);
    }
}
using Avalonia.Controls;
using Avalonia.Threading;
using Clac.UI.Configuration;
using Clac.UI.ViewModels;
using System.Collections.Specialized;

namespace Clac.UI.Views;

public partial class DisplayView : UserControl
{
    public DisplayView()
    {
        InitializeComponent();

        int displayLines = SettingsManager.UI.DisplayLines;
        int lineHeight = SettingsManager.UI.LineHeight;
        int borderThickness = SettingsManager.UI.BorderThickness;
        DisplayBorder.Height = (displayLines * lineHeight) + borderThickness;

        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is CalculatorViewModel viewModel)
            viewModel.DisplayItems.CollectionChanged += OnDisplayItemsChanged;
    }

    private void OnDisplayItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            StackScrollViewer.ScrollToEnd();
        }, DispatcherPriority.Loaded);
    }
}

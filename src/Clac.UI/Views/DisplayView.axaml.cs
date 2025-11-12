using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Clac.UI.Configuration;
using Clac.UI.ViewModels;
using System.Collections.Specialized;
using System.ComponentModel;

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
        {
            viewModel.DisplayItems.CollectionChanged += OnDisplayItemsChanged;
            viewModel.PropertyChanged += OnViewModelPropertyChanged;
            UpdateScrollBarVisibility(viewModel);
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CalculatorViewModel.ShowScrollBar) && DataContext is CalculatorViewModel viewModel)
        {
            UpdateScrollBarVisibility(viewModel);
        }
    }

    private void UpdateScrollBarVisibility(CalculatorViewModel viewModel)
    {
        StackScrollViewer.VerticalScrollBarVisibility = viewModel.ShowScrollBar
            ? ScrollBarVisibility.Auto
            : ScrollBarVisibility.Hidden;
    }

    private void OnDisplayItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            StackScrollViewer.ScrollToEnd();
        }, DispatcherPriority.Loaded);
    }
}

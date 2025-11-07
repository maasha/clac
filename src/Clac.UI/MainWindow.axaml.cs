using Avalonia.Controls;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;
using System.ComponentModel;

namespace Clac.UI;

public partial class MainWindow : Window
{
    private const double ErrorLineHeight = 30.0;
    private double _baseWindowHeight;
    private CalculatorViewModel? _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new CalculatorViewModel();
        DataContext = _viewModel;

        // Set initial window size from settings
        Width = SettingsManager.UI.WindowWidth;
        Height = SettingsManager.UI.WindowHeight;
        _baseWindowHeight = SettingsManager.UI.WindowHeight;

        // Wait for window to load before setting base height
        Loaded += OnWindowLoaded;

        // Subscribe to HasError changes
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnWindowLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Set base height after window is loaded
        _baseWindowHeight = Height;
    }

    /// <summary>
    /// Handles changes to the view model's HasError property.
    /// If the HasError property is true, the window height is increased by the error line height.
    /// If the HasError property is false, the window height is restored to the base height.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CalculatorViewModel.HasError) && _viewModel != null)
        {
            if (_viewModel.HasError)
            {
                Height = _baseWindowHeight + ErrorLineHeight;
            }
            else
            {
                Height = _baseWindowHeight;
            }
        }
    }
}
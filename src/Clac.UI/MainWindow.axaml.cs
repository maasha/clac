using Avalonia.Controls;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;
using System.ComponentModel;

namespace Clac.UI;

public partial class MainWindow : Window
{
    private const double ErrorLineHeight = 50.0;
    private double _baseWindowHeight;
    private CalculatorViewModel? _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new CalculatorViewModel();
        DataContext = _viewModel;

        Width = SettingsManager.UI.WindowWidth;
        Height = SettingsManager.UI.WindowHeight;
        _baseWindowHeight = SettingsManager.UI.WindowHeight;

        Loaded += OnWindowLoaded;

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnWindowLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _baseWindowHeight = Height;
    }

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

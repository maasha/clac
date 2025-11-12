using Avalonia.Controls;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;

namespace Clac.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new CalculatorViewModel();
        DataContext = viewModel;

        Width = SettingsManager.UI.WindowWidth;
    }
}

using Avalonia.Controls;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;

namespace Clac.UI;

public partial class MainWindow : Window
{
    public MainWindow(CalculatorViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }

    public MainWindow()
    {
        InitializeComponent();
        Width = SettingsManager.UI.WindowWidth;
    }
}

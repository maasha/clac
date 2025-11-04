using Avalonia.Controls;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;

namespace Clac.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new CalculatorViewModel();

        // Set initial window size from settings
        Width = SettingsManager.UI.WindowWidth;
        Height = SettingsManager.UI.WindowHeight;
    }
}
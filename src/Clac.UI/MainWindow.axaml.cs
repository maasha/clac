using Avalonia.Controls;
using Clac.UI.ViewModels;

namespace Clac.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new CalculatorViewModel();
    }
}
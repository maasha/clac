using Avalonia.Controls;
using Clac.UI.ViewModels;
using Clac.UI.Configuration;
using System;
using Avalonia.Threading;

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

        // Timer to increase height after 3 seconds
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(3)
        };
        timer.Tick += (sender, e) =>
        {
            Height += 30;
            timer.Stop();
        };
        timer.Start();
    }
}
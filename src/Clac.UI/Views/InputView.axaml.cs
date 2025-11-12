using Avalonia.Controls;
using Avalonia.Input;
using Clac.UI.ViewModels;

namespace Clac.UI.Views;

public partial class InputView : UserControl
{
    public InputView()
    {
        InitializeComponent();
    }

    private void OnInputKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && DataContext is CalculatorViewModel viewModel)
            viewModel.Enter();
    }
}

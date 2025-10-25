using Avalonia.Controls;
using Clac.UI.ViewModels;
using System.Linq;

namespace Clac.UI.Views;

public partial class DisplayView : UserControl
{
    public DisplayView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is CalculatorViewModel viewModel)
        {
            UpdateDisplay(viewModel.StackDisplay);
        }
    }

    private void UpdateDisplay(string[] stack)
    {
        // Get the last 4 items from the stack (or fewer if stack is smaller)
        var displayItems = stack.TakeLast(4).ToArray();

        // HP48 style: Line 1 is top of stack, Line 4 is 4th from top
        Line1.Text = FormatLine(1, displayItems.Length >= 1 ? displayItems[^1] : null);
        Line2.Text = FormatLine(2, displayItems.Length >= 2 ? displayItems[^2] : null);
        Line3.Text = FormatLine(3, displayItems.Length >= 3 ? displayItems[^3] : null);
        Line4.Text = FormatLine(4, displayItems.Length >= 4 ? displayItems[^4] : null);
    }

    private string FormatLine(int lineNumber, string? value)
    {
        return value != null ? $"{lineNumber}: {value}" : $"{lineNumber}:";
    }
}
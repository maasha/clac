using Avalonia.Controls;
using Clac.UI.Configuration;

namespace Clac.UI.Views;

/// <summary>
/// View for displaying the stack in the display.
/// </summary>
public partial class DisplayView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the DisplayView class.
    /// </summary>
    public DisplayView()
    {
        InitializeComponent();

        int displayLines = SettingsManager.UI.DisplayLines;
        int lineHeight = SettingsManager.UI.LineHeight;
        int borderThickness = SettingsManager.UI.BorderThickness;
        DisplayBorder.Height = (displayLines * lineHeight) + (borderThickness * 2); // top + bottom border
    }
}
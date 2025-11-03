using Avalonia.Controls;
using Clac.UI.Configuration;

namespace Clac.UI.Views;

/// <summary>
/// View for displaying the stack in the display.
/// </summary>
public partial class DisplayView : UserControl
{
    private const int DisplayBorderThickness = 4; // 2px top + 2px bottom

    /// <summary>
    /// Initializes a new instance of the DisplayView class.
    /// </summary>
    public DisplayView()
    {
        InitializeComponent();

        // Set display height based on configured number of lines and line height
        int displayLines = SettingsManager.UI.DisplayLines;
        int lineHeight = SettingsManager.UI.LineHeight;
        DisplayBorder.Height = (displayLines * lineHeight) + DisplayBorderThickness;
    }
}
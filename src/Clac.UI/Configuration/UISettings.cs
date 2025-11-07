namespace Clac.UI.Configuration;

/// <summary>
/// Contains user interface settings and preferences.
/// </summary>
public class UISettings
{
    /// <summary>
    /// Gets or sets the height of the window in pixels.
    /// Default is 450 pixels.
    /// </summary>
    public int WindowHeight { get; set; } = 450;

    /// <summary>
    /// Gets or sets the width of the window in pixels.
    /// Default is 400 pixels.
    /// </summary>
    public int WindowWidth { get; set; } = 400;

    /// <summary>
    /// Gets or sets the number of lines to display in the stack display.
    /// Default is 6 lines.
    /// </summary>
    public int DisplayLines { get; set; } = 6;

    /// <summary>
    /// Gets or sets the height of each line in the display in pixels.
    /// Default is 30 pixels.
    /// </summary>
    public int LineHeight { get; set; } = 30;

    /// <summary>
    /// Gets or sets the border thickness of the display in pixels.
    /// Default is 2 pixels.
    /// </summary>
    public int BorderThickness { get; set; } = 2;
}


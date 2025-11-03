namespace Clac.UI.Models;

/// <summary>
/// Represents a single line item in the stack display.
/// </summary>
public class StackLineItem
{
    /// <summary>
    /// Gets or sets the line number text (e.g., "1:", "2:").
    /// </summary>
    public string LineNumber { get; set; } = "";

    /// <summary>
    /// Gets or sets the formatted value to display.
    /// </summary>
    public string FormattedValue { get; set; } = "";
}


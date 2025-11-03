namespace Clac.UI.Models;

/// <summary>
/// Represents a single line item in the stack display.
/// </summary>
public class StackLineItem
{
    /// <summary>
    /// Initializes a new instance of the StackLineItem class.
    /// </summary>
    /// <param name="lineNumber">The line number text (e.g., "1:", "2:").</param>
    /// <param name="formattedValue">The formatted value to display.</param>
    public StackLineItem(string lineNumber, string formattedValue)
    {
        LineNumber = lineNumber;
        FormattedValue = formattedValue;
    }

    /// <summary>
    /// Gets the line number text (e.g., "1:", "2:").
    /// </summary>
    public string LineNumber { get; }

    /// <summary>
    /// Gets the formatted value to display.
    /// </summary>
    public string FormattedValue { get; }
}


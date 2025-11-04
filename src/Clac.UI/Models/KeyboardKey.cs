using Clac.UI.Enums;

namespace Clac.UI.Models;

/// <summary>
///  A calculator keyboard key.
/// </summary>
public class KeyboardKey
{
    /// <summary>
    /// The label on the key, e.g. "7", "+", "ENTER".
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// The text appended to the input when the button is pressed.
    /// </summary>
    public required string Value { get; set; } // What to append to input

    /// <summary>
    /// Key type such as number, operator, command or function.
    /// </summary>
    public required KeyType Type { get; set; }

    /// <summary>
    /// Grid column span for the layout. Default is 1.
    /// </summary>
    public int ColumnSpan { get; set; } = 1;
}
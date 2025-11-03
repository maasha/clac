using System.Linq;

namespace Clac.UI.Helpers;

/// <summary>
/// Class for formatting the display of the calculator.
/// 
/// The display shows the values from the stack on separate lines.
/// Each line is prefixed with line number in such a way that the numbers are
/// aligned vertically. The values are aligned to the right in such a way that the
/// integer part of the numbers are aligned.
/// 
/// Example:
/// 10:                        1
///  9:                        3
///  8:                        3
///  7:                        3 
///  6:                        4
///  5:                        3
///  3:                        3.0
///  4:                        4.2
///  2:                       -2.0
///  1:                        1
/// </summary>
public static class DisplayFormatter
{
    /// <summary>
    /// Formats the line number for the display.
    /// 
    /// The line number is prefixed with padding to ensure vertical alignment.
    /// 
    /// Examples:
    /// 100: 4
    /// ...
    ///  10: 5
    /// ...
    ///   1: 6
    /// </summary>
    /// <param name="lineNumber">The line number to format.</param>
    /// <param name="maxLineNumber">The maximum line number to format.</param>
    /// <returns>The formatted line number.</returns>
    public static string FormatLineNumber(int lineNumber, int maxLineNumber)
    {
        int maxWidth = maxLineNumber.ToString().Length;
        return lineNumber.ToString().PadLeft(maxWidth) + ":";
    }

    /// <summary>
    /// Gets the maximum length of the integer part of the given values. For
    /// negative numbers, the minus sign is included in the length.
    /// </summary>
    /// <param name="values">The values to get the maximum length from.</param>
    /// <returns>The maximum length of the integer part of the values.</returns>
    public static int GetMaxIntegerPartLength(string[] values)
    {
        return values.Max(value => value.Split('.')[0].Length);
    }

    /// Formats the value for the display.
    /// 
    /// The value is is prefixed with padding to ensure vertical alignment or
    /// of the integer part of the number.
    /// 
    /// Examples:
    /// 4:  1
    /// 3:  2.0
    /// 2: -2.0
    /// 1: -1
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="maxIntegerPartLength">The maximum length of the integer part of the numbers.</param>
    /// <returns>The formatted value.</returns>
    public static string FormatValue(string value, int maxIntegerPartLength)
    {
        int currentIntegerPartLength = value.Split('.')[0].Length;
        int paddingNeeded = maxIntegerPartLength - currentIntegerPartLength;
        return new string(' ', paddingNeeded) + value;
    }
}
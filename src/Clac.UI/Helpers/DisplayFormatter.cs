using System.Linq;

namespace Clac.UI.Helpers;

public static class DisplayFormatter
{
    public static string FormatLineNumber(int lineNumber, int maxLineNumber)
    {
        int maxWidth = maxLineNumber.ToString().Length;
        return lineNumber.ToString().PadLeft(maxWidth) + ":";
    }

    public static int GetMaxIntegerPartLength(string[] values)
    {
        return values.Max(value => value.Split('.')[0].Length);
    }

    public static string FormatValue(string value, int maxIntegerPartLength)
    {
        int currentIntegerPartLength = value.Split('.')[0].Length;
        int paddingNeeded = maxIntegerPartLength - currentIntegerPartLength;
        return new string(' ', paddingNeeded) + value;
    }
}

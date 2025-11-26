using System.Linq;

namespace Clac.UI.Helpers;

public static class DisplayFormatter
{
    public static string FormatLineNumber(int lineNumber, int maxLineNumber)
    {
        int maxWidth = maxLineNumber.ToString().Length;
        return lineNumber.ToString().PadLeft(maxWidth) + ":";
    }

    public static int GetMaxDecimalPartLength(string[] values)
    {
        return values.Max(value =>
        {
            var parts = value.Split('.');
            return parts.Length > 1 ? parts[1].Length : 0;
        });
    }

    public static string FormatValue(string value, int maxDecimalPartLength)
    {
        if (maxDecimalPartLength == 0)
            return value;

        int paddingNeeded = CalculatePaddingNeeded(value, maxDecimalPartLength);
        return value + new string(' ', paddingNeeded);
    }

    private static int CalculatePaddingNeeded(string value, int maxDecimalPartLength)
    {
        var parts = value.Split('.');
        int currentDecimalPartLength = parts.Length > 1 ? parts[1].Length : 0;
        return CalculatePaddingAmount(currentDecimalPartLength, maxDecimalPartLength);
    }

    private static int CalculatePaddingAmount(int currentDecimalPartLength, int maxDecimalPartLength)
    {
        return currentDecimalPartLength == 0
            ? 1 + maxDecimalPartLength
            : System.Math.Max(0, maxDecimalPartLength - currentDecimalPartLength);
    }
}

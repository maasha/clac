namespace Clac.UI.Tests;

using Xunit;
using Clac.UI.Helpers;

public class DisplayFormatterTests
{
    [Fact]
    public void FormatLineNumber_SingleDigitWithMaxSingleDigit_NoPadding()
    {
        int lineNumber = 1;
        int maxLineNumber = 9;

        string result = DisplayFormatter.FormatLineNumber(lineNumber, maxLineNumber);

        Assert.Equal("1:", result);
    }

    [Fact]
    public void FormatLineNumber_SingleDigitWithMaxTwoDigit_PadsWithOneSpace()
    {
        int lineNumber = 1;
        int maxLineNumber = 10;

        string result = DisplayFormatter.FormatLineNumber(lineNumber, maxLineNumber);

        Assert.Equal(" 1:", result);
    }

    [Fact]
    public void GetMaxDecimalPartLength_MixedInteger_ReturnsZero()
    {
        string[] values = ["1", "2", "3"];

        int result = DisplayFormatter.GetMaxDecimalPartLength(values);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetMaxDecimalPartLength_MixedValues_ReturnsMax()
    {
        string[] values = ["1", "3.01", "4.2"];

        int result = DisplayFormatter.GetMaxDecimalPartLength(values);

        Assert.Equal(2, result);
    }

    [Fact]
    public void FormatValue_IntegerWithMaxSameLength_NoPadding()
    {
        string value = "1";
        int maxDecimalPartLength = 0;

        string result = DisplayFormatter.FormatValue(value, maxDecimalPartLength);

        Assert.Equal("1", result);
    }

    [Fact]
    public void FormatValue_IntegerOnly_PadsRightToMatchMaxDecimalPartLength()
    {
        string value = "1";
        int maxDecimalPartLength = 2;

        string result = DisplayFormatter.FormatValue(value, maxDecimalPartLength);

        Assert.Equal("1   ", result);
    }

    [Fact]
    public void FormatValue_DecimalPartShorterThanMax_PadsRight()
    {
        string value = "3.0";
        int maxDecimalPartLength = 2;

        string result = DisplayFormatter.FormatValue(value, maxDecimalPartLength);

        Assert.Equal("3.0 ", result);
    }

    [Fact]
    public void FormatValue_WithEqualDecimalPartLength_NoPadding()
    {
        string value = "3.0";
        int maxDecimalPartLength = 1;

        string result = DisplayFormatter.FormatValue(value, maxDecimalPartLength);

        Assert.Equal("3.0", result);
    }
}
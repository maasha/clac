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
    public void GetMaxIntegerPartLength_MixedValues_ReturnsMax()
    {
        string[] values = ["1", "3.0", "4.2"];

        int result = DisplayFormatter.GetMaxIntegerPartLength(values);

        Assert.Equal(1, result); // All have 1-digit integer part
    }

    [Fact]
    public void GetMaxIntegerPartLength_MultipleDigitInteger_ReturnsMax()
    {
        string[] values = ["1", "123", "4.2"];

        int result = DisplayFormatter.GetMaxIntegerPartLength(values);

        Assert.Equal(3, result);
    }

    [Fact]
    public void GetMaxIntegerPartLength_DecimalWithoutIntegerPart_CountsAsZero()
    {
        string[] values = [".3"];

        int result = DisplayFormatter.GetMaxIntegerPartLength(values);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetMaxIntegerPartLength_NegativeNumber_IncludesMinusSign()
    {
        string[] values = ["1", "-2"];

        int result = DisplayFormatter.GetMaxIntegerPartLength(values);

        Assert.Equal(2, result);
    }

    [Fact]
    public void GetMaxIntegerPartLength_NegativeDecimalWithoutIntegerPart_IncludesMinusSign()
    {
        string[] values = ["-.3"];

        int result = DisplayFormatter.GetMaxIntegerPartLength(values);

        Assert.Equal(1, result);
    }

    [Fact]
    public void FormatValue_IntegerWithMaxSameLength_NoPadding()
    {
        string value = "1";
        int maxIntegerPartLength = 1;

        string result = DisplayFormatter.FormatValue(value, maxIntegerPartLength);

        Assert.Equal("1", result);
    }

    [Fact]
    public void FormatValue_IntegerShorterThanMax_PadsLeft()
    {
        string value = "1";
        int maxIntegerPartLength = 2;

        string result = DisplayFormatter.FormatValue(value, maxIntegerPartLength);

        Assert.Equal(" 1", result);
    }

    [Fact]
    public void FormatValue_DecimalShorterThanMax_PadsLeftPreservesDecimal()
    {
        string value = "3.0";
        int maxIntegerPartLength = 2;

        string result = DisplayFormatter.FormatValue(value, maxIntegerPartLength);

        Assert.Equal(" 3.0", result);
    }

    [Fact]
    public void FormatValue_DecimalWithoutIntegerPart_PadsLeft()
    {
        string value = ".3";
        int maxIntegerPartLength = 2;

        string result = DisplayFormatter.FormatValue(value, maxIntegerPartLength);

        Assert.Equal("  .3", result);
    }

    [Fact]
    public void FormatValue_NegativeValue_PadsCorrectly()
    {
        string value = "-2";
        int maxIntegerPartLength = 3;

        string result = DisplayFormatter.FormatValue(value, maxIntegerPartLength);

        Assert.Equal(" -2", result);
    }

    [Fact]
    public void FormatValue_NegativeDecimal_PadsCorrectly()
    {
        string value = "-2.0";
        int maxIntegerPartLength = 3;

        string result = DisplayFormatter.FormatValue(value, maxIntegerPartLength);

        Assert.Equal(" -2.0", result);
    }

    [Fact]
    public void FormatMultipleLines_MixedValues_AlignCorrectly()
    {
        string[] values = ["43", "34", "34", "0.3"];
        int maxLineNumber = 4;
        int maxIntegerPartLength = DisplayFormatter.GetMaxIntegerPartLength(values);

        string line4 = DisplayFormatter.FormatLineNumber(4, maxLineNumber);
        string value4 = DisplayFormatter.FormatValue("43", maxIntegerPartLength);

        string line3 = DisplayFormatter.FormatLineNumber(3, maxLineNumber);
        string value3 = DisplayFormatter.FormatValue("34", maxIntegerPartLength);

        string line2 = DisplayFormatter.FormatLineNumber(2, maxLineNumber);
        string value2 = DisplayFormatter.FormatValue("34", maxIntegerPartLength);

        string line1 = DisplayFormatter.FormatLineNumber(1, maxLineNumber);
        string value1 = DisplayFormatter.FormatValue("0.3", maxIntegerPartLength);

        Assert.Equal("4:", line4);
        Assert.Equal("43", value4);
        Assert.Equal("3:", line3);
        Assert.Equal("34", value3);
        Assert.Equal("2:", line2);
        Assert.Equal("34", value2);
        Assert.Equal("1:", line1);
        Assert.Equal(" 0.3", value1);
    }

    [Fact]
    public void FormatMultipleLines_WithNegatives_AlignCorrectly()
    {
        string[] values = ["10", "-5", "3.14"];
        int maxLineNumber = 3;
        int maxIntegerPartLength = DisplayFormatter.GetMaxIntegerPartLength(values);

        string line3 = DisplayFormatter.FormatLineNumber(3, maxLineNumber);
        string value3 = DisplayFormatter.FormatValue("10", maxIntegerPartLength);

        string line2 = DisplayFormatter.FormatLineNumber(2, maxLineNumber);
        string value2 = DisplayFormatter.FormatValue("-5", maxIntegerPartLength);

        string line1 = DisplayFormatter.FormatLineNumber(1, maxLineNumber);
        string value1 = DisplayFormatter.FormatValue("3.14", maxIntegerPartLength);

        Assert.Equal("3:", line3);
        Assert.Equal("10", value3);
        Assert.Equal("2:", line2);
        Assert.Equal("-5", value2);
        Assert.Equal("1:", line1);
        Assert.Equal(" 3.14", value1);
    }
}
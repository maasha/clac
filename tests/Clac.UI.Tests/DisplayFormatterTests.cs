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

        Assert.Equal(1, result);
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
}
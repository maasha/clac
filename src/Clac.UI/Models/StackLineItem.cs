namespace Clac.UI.Models;

public class StackLineItem
{
    public StackLineItem(string lineNumber, string formattedValue)
    {
        LineNumber = lineNumber;
        FormattedValue = formattedValue;
    }

    public string LineNumber { get; }

    public string FormattedValue { get; }
}

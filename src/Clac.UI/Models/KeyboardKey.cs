using Clac.UI.Enums;

namespace Clac.UI.Models;

public class KeyboardKey
{
    public required string Label { get; set; }

    public required string Value { get; set; }

    public required KeyType Type { get; set; }

    public int ColumnSpan { get; set; } = 1;
}

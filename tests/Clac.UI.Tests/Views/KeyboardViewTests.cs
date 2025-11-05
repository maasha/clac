namespace Clac.UI.Tests.Views;

using Xunit;
using Clac.UI.Views;
using Clac.UI.Models;
using Clac.UI.Enums;
using Avalonia.Controls;

public class KeyboardViewTests
{
    [Fact]
    public void KeyboardView_ShouldHaveDeleteKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var deleteKeyView = view.FindControl<KeyboardKeyView>("DeleteKeyView");
        Assert.NotNull(deleteKeyView);

        var deleteKey = deleteKeyView.DataContext as KeyboardKey;
        Assert.NotNull(deleteKey);
        Assert.Equal("DEL", deleteKey.Label);
        Assert.Equal(KeyType.Command, deleteKey.Type);
    }
}


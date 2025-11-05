namespace Clac.UI.Tests.Views;

using Xunit;
using Clac.UI.Views;
using Clac.UI.Models;
using Clac.UI.Enums;
using Avalonia.Controls;

public class KeyboardViewTests
{
    [Fact]
    public void KeyboardView_ShouldHaveKey1_WhenInitialized()
    {
        var view = new KeyboardView();

        var key1View = view.FindControl<KeyboardKeyView>("Key1View");
        Assert.NotNull(key1View);

        var key1 = key1View.DataContext as KeyboardKey;
        Assert.NotNull(key1);
        Assert.Equal("1", key1.Label);
        Assert.Equal("1", key1.Value);
        Assert.Equal(KeyType.Number, key1.Type);
    }

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

    [Fact]
    public void KeyboardView_ShouldHavePlusKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var plusKeyView = view.FindControl<KeyboardKeyView>("PlusKeyView");
        Assert.NotNull(plusKeyView);

        var plusKey = plusKeyView.DataContext as KeyboardKey;
        Assert.NotNull(plusKey);
        Assert.Equal("+", plusKey.Label);
        Assert.Equal("+", plusKey.Value);
        Assert.Equal(KeyType.Operator, plusKey.Type);
    }
}


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

    [Fact]
    public void KeyboardView_ShouldHaveMinusKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var minusKeyView = view.FindControl<KeyboardKeyView>("MinusKeyView");
        Assert.NotNull(minusKeyView);

        var minusKey = minusKeyView.DataContext as KeyboardKey;
        Assert.NotNull(minusKey);
        Assert.Equal("-", minusKey.Label);
        Assert.Equal("-", minusKey.Value);
        Assert.Equal(KeyType.Operator, minusKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveEnterKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var enterKeyView = view.FindControl<KeyboardKeyView>("EnterKeyView");
        Assert.NotNull(enterKeyView);

        var enterKey = enterKeyView.DataContext as KeyboardKey;
        Assert.NotNull(enterKey);
        Assert.Equal("ENTER", enterKey.Label);
        Assert.Equal("", enterKey.Value);
        Assert.Equal(KeyType.Enter, enterKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveZeroKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var zeroKeyView = view.FindControl<KeyboardKeyView>("Key0View");
        Assert.NotNull(zeroKeyView);

        var zeroKey = zeroKeyView.DataContext as KeyboardKey;
        Assert.NotNull(zeroKey);
        Assert.Equal("0", zeroKey.Label);
        Assert.Equal("0", zeroKey.Value);
        Assert.Equal(KeyType.Number, zeroKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveTwoKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var twoKeyView = view.FindControl<KeyboardKeyView>("Key2View");
        Assert.NotNull(twoKeyView);

        var twoKey = twoKeyView.DataContext as KeyboardKey;
        Assert.NotNull(twoKey);
        Assert.Equal("2", twoKey.Label);
        Assert.Equal("2", twoKey.Value);
        Assert.Equal(KeyType.Number, twoKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveThreeKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var threeKeyView = view.FindControl<KeyboardKeyView>("Key3View");
        Assert.NotNull(threeKeyView);

        var threeKey = threeKeyView.DataContext as KeyboardKey;
        Assert.NotNull(threeKey);
        Assert.Equal("3", threeKey.Label);
        Assert.Equal("3", threeKey.Value);
        Assert.Equal(KeyType.Number, threeKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveFourKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var fourKeyView = view.FindControl<KeyboardKeyView>("Key4View");
        Assert.NotNull(fourKeyView);

        var fourKey = fourKeyView.DataContext as KeyboardKey;
        Assert.NotNull(fourKey);
        Assert.Equal("4", fourKey.Label);
        Assert.Equal("4", fourKey.Value);
        Assert.Equal(KeyType.Number, fourKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveFiveKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var fiveKeyView = view.FindControl<KeyboardKeyView>("Key5View");
        Assert.NotNull(fiveKeyView);

        var fiveKey = fiveKeyView.DataContext as KeyboardKey;
        Assert.NotNull(fiveKey);
        Assert.Equal("5", fiveKey.Label);
        Assert.Equal("5", fiveKey.Value);
        Assert.Equal(KeyType.Number, fiveKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveSixKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var sixKeyView = view.FindControl<KeyboardKeyView>("Key6View");
        Assert.NotNull(sixKeyView);

        var sixKey = sixKeyView.DataContext as KeyboardKey;
        Assert.NotNull(sixKey);
        Assert.Equal("6", sixKey.Label);
        Assert.Equal("6", sixKey.Value);
        Assert.Equal(KeyType.Number, sixKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveSevenKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var sevenKeyView = view.FindControl<KeyboardKeyView>("Key7View");
        Assert.NotNull(sevenKeyView);

        var sevenKey = sevenKeyView.DataContext as KeyboardKey;
        Assert.NotNull(sevenKey);
        Assert.Equal("7", sevenKey.Label);
        Assert.Equal("7", sevenKey.Value);
        Assert.Equal(KeyType.Number, sevenKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveEightKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var eightKeyView = view.FindControl<KeyboardKeyView>("Key8View");
        Assert.NotNull(eightKeyView);

        var eightKey = eightKeyView.DataContext as KeyboardKey;
        Assert.NotNull(eightKey);
        Assert.Equal("8", eightKey.Label);
        Assert.Equal("8", eightKey.Value);
        Assert.Equal(KeyType.Number, eightKey.Type);
    }

    [Fact]
    public void KeyboardView_ShouldHaveNineKey_WhenInitialized()
    {
        var view = new KeyboardView();

        var nineKeyView = view.FindControl<KeyboardKeyView>("Key9View");
        Assert.NotNull(nineKeyView);

        var nineKey = nineKeyView.DataContext as KeyboardKey;
        Assert.NotNull(nineKey);
        Assert.Equal("9", nineKey.Label);
        Assert.Equal("9", nineKey.Value);
        Assert.Equal(KeyType.Number, nineKey.Type);
    }
}


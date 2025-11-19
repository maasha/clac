using Xunit;
using Clac.UI.ViewModels;
using Clac.Core;
using static Clac.Core.ErrorMessages;
using System.ComponentModel;
using System.Linq;
using Clac.Core.Services;
using Clac.UI.Tests.Spies;
using System.IO.Abstractions;

namespace Clac.UI.Tests.ViewModels;

public class CalculatorViewModelErrorTests
{
    private readonly CalculatorViewModel _vm;
    private static IPersistence DummyPersistence() => new PersistenceSpy(new FileSystem());

    public CalculatorViewModelErrorTests()
    {
        _vm = new CalculatorViewModel(DummyPersistence());
    }

    [Fact]
    public void UserCanSeeErrorMessage_WhenInputIsInvalid()
    {
        _vm.CurrentInput = "abc";

        _vm.Enter();

        var errorInfo = _vm as INotifyDataErrorInfo;
        Assert.NotNull(errorInfo);
        Assert.True(errorInfo.HasErrors);
    }

    [Fact]
    public void ErrorMessageDisappears_WhenValidInputIsEntered()
    {

        // First, create an error
        _vm.CurrentInput = "abc";
        _vm.Enter();

        // Then, enter valid input
        _vm.CurrentInput = "5";
        _vm.Enter();

        var errorInfo = _vm as INotifyDataErrorInfo;
        Assert.NotNull(errorInfo);
        Assert.False(errorInfo.HasErrors);
    }

    [Fact]
    public void OnlyMostRecentError_IsShown_WhenMultipleInvalidInputsEntered()
    {
        var errorInfo = _vm as INotifyDataErrorInfo;
        Assert.NotNull(errorInfo);

        // First invalid input
        _vm.CurrentInput = "abc";
        _vm.Enter();

        var firstErrors = errorInfo.GetErrors(nameof(_vm.CurrentInput)).Cast<string>().ToList();
        Assert.Single(firstErrors);
        Assert.Contains("abc", firstErrors[0]);

        // Second invalid input
        _vm.CurrentInput = "xyz";
        _vm.Enter();

        var secondErrors = errorInfo.GetErrors(nameof(_vm.CurrentInput)).Cast<string>().ToList();
        Assert.Single(secondErrors); // Should still be only ONE error
        Assert.Contains("xyz", secondErrors[0]); // Should be the NEW error
        Assert.DoesNotContain("abc", secondErrors[0]); // Should NOT contain old error
    }

    [Fact]
    public void StackShouldReflectSuccessfulOperations_WhenErrorOccursAfterPartialSuccess()
    {

        _vm.CurrentInput = "5";
        _vm.Enter();

        _vm.CurrentInput = "3";
        _vm.Enter();

        Assert.Equal(2, _vm.StackDisplay.Length);
        Assert.Equal("5", _vm.StackDisplay[0]);
        Assert.Equal("3", _vm.StackDisplay[1]);

        _vm.CurrentInput = "+ +";
        _vm.Enter();

        Assert.True(_vm.HasError);
        Assert.Contains(StackHasLessThanTwoNumbers, _vm.ErrorMessage);
        Assert.Single(_vm.StackDisplay);
        Assert.Equal("8", _vm.StackDisplay[0]);
    }

    [Fact]
    public void DisplayShouldReflectSuccessfulOperations_WhenErrorOccursAfterPartialSuccess()
    {

        _vm.CurrentInput = "5";
        _vm.Enter();

        _vm.CurrentInput = "3";
        _vm.Enter();

        _vm.CurrentInput = "+ +";
        _vm.Enter();

        Assert.True(_vm.HasError);
        Assert.Contains(StackHasLessThanTwoNumbers, _vm.ErrorMessage);
        var displayItem = Assert.Single(_vm.DisplayItems, item => !string.IsNullOrEmpty(item.FormattedValue));
        Assert.Equal("8", displayItem.FormattedValue.Trim());
    }

    [Fact]
    public void ErrorMessageShouldBeCleared_WhenUserStartsTypingAfterError()
    {

        _vm.CurrentInput = "5";
        _vm.Enter();

        _vm.CurrentInput = "+";
        _vm.Enter();

        Assert.True(_vm.HasError);
        Assert.Contains(StackHasLessThanTwoNumbers, _vm.ErrorMessage);

        _vm.CurrentInput = "1";

        Assert.False(_vm.HasError);
        Assert.Empty(_vm.ErrorMessage);
    }
}


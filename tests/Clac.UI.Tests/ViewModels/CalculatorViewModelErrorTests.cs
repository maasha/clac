using Xunit;
using Clac.UI.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace Clac.UI.Tests.ViewModels;

public class CalculatorViewModelErrorTests
{
    [Fact]
    public void UserCanSeeErrorMessage_WhenInputIsInvalid()
    {
        var vm = new CalculatorViewModel();
        vm.CurrentInput = "abc";

        vm.Enter();

        var errorInfo = vm as INotifyDataErrorInfo;
        Assert.NotNull(errorInfo);
        Assert.True(errorInfo.HasErrors);
    }

    [Fact]
    public void ErrorMessageDisappears_WhenValidInputIsEntered()
    {
        var vm = new CalculatorViewModel();

        // First, create an error
        vm.CurrentInput = "abc";
        vm.Enter();

        // Then, enter valid input
        vm.CurrentInput = "5";
        vm.Enter();

        var errorInfo = vm as INotifyDataErrorInfo;
        Assert.NotNull(errorInfo);
        Assert.False(errorInfo.HasErrors);
    }

    [Fact]
    public void OnlyMostRecentError_IsShown_WhenMultipleInvalidInputsEntered()
    {
        var vm = new CalculatorViewModel();
        var errorInfo = vm as INotifyDataErrorInfo;
        Assert.NotNull(errorInfo);

        // First invalid input
        vm.CurrentInput = "abc";
        vm.Enter();

        var firstErrors = errorInfo.GetErrors(nameof(vm.CurrentInput)).Cast<string>().ToList();
        Assert.Single(firstErrors);
        Assert.Contains("abc", firstErrors[0]);

        // Second invalid input
        vm.CurrentInput = "xyz";
        vm.Enter();

        var secondErrors = errorInfo.GetErrors(nameof(vm.CurrentInput)).Cast<string>().ToList();
        Assert.Single(secondErrors); // Should still be only ONE error
        Assert.Contains("xyz", secondErrors[0]); // Should be the NEW error
        Assert.DoesNotContain("abc", secondErrors[0]); // Should NOT contain old error
    }

    [Fact]
    public void StackShouldReflectSuccessfulOperations_WhenErrorOccursAfterPartialSuccess()
    {
        var vm = new CalculatorViewModel();

        vm.CurrentInput = "5";
        vm.Enter();

        vm.CurrentInput = "3";
        vm.Enter();

        Assert.Equal(2, vm.StackDisplay.Length);
        Assert.Equal("5", vm.StackDisplay[0]);
        Assert.Equal("3", vm.StackDisplay[1]);

        vm.CurrentInput = "+ +";
        vm.Enter();

        Assert.True(vm.HasError);
        Assert.Contains("Stack has less than two numbers", vm.ErrorMessage);
        Assert.Single(vm.StackDisplay);
        Assert.Equal("8", vm.StackDisplay[0]);
    }

    [Fact(Skip = "Pending implementation")]
    public void DisplayShouldReflectSuccessfulOperations_WhenErrorOccursAfterPartialSuccess()
    {
        var vm = new CalculatorViewModel();

        vm.CurrentInput = "5";
        vm.Enter();

        vm.CurrentInput = "3";
        vm.Enter();

        vm.CurrentInput = "+ +";
        vm.Enter();

        Assert.True(vm.HasError);
        Assert.Contains("Stack has less than two numbers", vm.ErrorMessage);
        Assert.Single(vm.DisplayItems.Where(item => !string.IsNullOrEmpty(item.FormattedValue)));
        Assert.Equal("8", vm.DisplayItems.First(item => !string.IsNullOrEmpty(item.FormattedValue)).FormattedValue.Trim());
    }
}


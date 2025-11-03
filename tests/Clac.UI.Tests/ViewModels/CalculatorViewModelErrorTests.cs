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
}


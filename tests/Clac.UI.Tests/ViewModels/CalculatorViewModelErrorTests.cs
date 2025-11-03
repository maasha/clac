using Xunit;
using Clac.UI.ViewModels;
using System.ComponentModel;

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
}


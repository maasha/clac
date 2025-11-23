
using Clac.UI.ViewModels;
using Clac.Core.Services;
using Clac.UI.Tests.Spies;
using System.IO.Abstractions.TestingHelpers;
using Clac.Core.History;
using Clac.Core.Rpn;

namespace Clac.UI.Tests;

public class CalculatorViewModelTests
{
    private readonly MockFileSystem mockFileSystem = new();
    private CalculatorViewModel _vm;
    public CalculatorViewModelTests()
    {
        IPersistence persistence = new PersistenceSpy(mockFileSystem);
        _vm = new CalculatorViewModel(persistence);
    }

    public class ConstructorTests : CalculatorViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithEmptyCurrentInput()
        {
            Assert.Equal("", _vm.CurrentInput);
        }

        [Fact]
        public void Constructor_WhenNoHistoryExists_ShouldInitializeWithEmptyStackDisplay()
        {
            Assert.Empty(_vm.StackDisplay);
        }

        [Fact]
        public void Constructor_WhenHistoryExists_ShouldInitializeWithHistoryLoadedOnStackDisplay()
        {
            var spy = new PersistenceSpy(mockFileSystem)
            {
                LoadResultToReturn = DummyHistory()
            };

            _vm = new CalculatorViewModel(spy);

            Assert.NotEmpty(_vm.StackDisplay);
        }
    }

    public class EnterTests : CalculatorViewModelTests
    {
        [Fact]
        public void Enter_WithNumber_ShouldPushToStackAndClearInput()
        {
            _vm.CurrentInput = "42";

            _vm.Enter();

            Assert.Single(_vm.StackDisplay);
            Assert.Equal("42", _vm.StackDisplay[0]);
            Assert.Equal("", _vm.CurrentInput);
        }

        [Fact]
        public void Enter_WithInvalidInput_ShouldSetErrorState()
        {
            _vm.CurrentInput = "abc";

            _vm.Enter();

            Assert.True(_vm.HasError);
            Assert.Contains("Invalid", _vm.ErrorMessage);
            Assert.Empty(_vm.StackDisplay);
        }

        [Fact]
        public void Enter_WithEmptyInput_ShouldDoNothing()
        {
            _vm.Enter();

            Assert.False(_vm.HasError);
            Assert.Empty(_vm.StackDisplay);
        }

        [Fact]
        public void Enter_WithInvalidInput_ShouldNotSaveToHistory()
        {
            _vm.CurrentInput = "1";
            _vm.Enter();

            Assert.True(_vm.CanUndo);

            _vm.CurrentInput = "abc";
            _vm.Enter();

            Assert.True(_vm.HasError);
            Assert.False(_vm.CanUndo);

            _vm.CurrentInput = "2";
            _vm.Enter();

            Assert.False(_vm.HasError);
            Assert.True(_vm.CanUndo);

            _vm.Undo();

            Assert.Single(_vm.StackDisplay);
            Assert.Equal("1", _vm.StackDisplay[0]);
            Assert.Equal("2", _vm.CurrentInput);
        }

        [Fact]
        public void Enter_WithValidInput_ShouldPersistHistoryToFile()
        {
            var persistenceSpy = new PersistenceSpy(mockFileSystem);
            var vm = new CalculatorViewModel(persistenceSpy)
            {
                CurrentInput = "42"
            };
            vm.Enter();

            Assert.Equal(1, persistenceSpy.SaveCallCount);
        }
    }

    public class UndoTests : CalculatorViewModelTests
    {
        [Fact]
        public void Undo_WithNoHistory_ShouldDoNothingInDisplay()
        {
            _vm.Undo();

            Assert.Empty(_vm.StackDisplay);
        }

        [Fact]
        public void Undo_WithHistory_ShouldRestoreBothStackAndInputTogether()
        {
            _vm.CurrentInput = "1";
            _vm.Enter();
            _vm.CurrentInput = "2";
            _vm.Enter();

            _vm.Undo();

            Assert.Single(_vm.StackDisplay);
            Assert.Equal("1", _vm.StackDisplay[0]);
            Assert.Equal("2", _vm.CurrentInput);
        }

        [Fact]
        public void Undo_MultipleTimes_ShouldMaintainSynchronization()
        {
            _vm.CurrentInput = "1";
            _vm.Enter();
            _vm.CurrentInput = "2";
            _vm.Enter();
            _vm.CurrentInput = "3";
            _vm.Enter();

            _vm.Undo();
            Assert.Equal(2, _vm.StackDisplay.Length);
            Assert.Equal("2", _vm.StackDisplay[1]);
            Assert.Equal("3", _vm.CurrentInput);

            _vm.Undo();
            Assert.Single(_vm.StackDisplay);
            Assert.Equal("1", _vm.StackDisplay[0]);
            Assert.Equal("2", _vm.CurrentInput);

            _vm.Undo();
            Assert.Empty(_vm.StackDisplay);
            Assert.Equal("1", _vm.CurrentInput);
        }

        [Fact]
        public void Undo_WithNoHistory_ShouldNotPersistHistoryToFile()
        {
            var persistenceSpy = new PersistenceSpy(mockFileSystem);
            var vm = new CalculatorViewModel(persistenceSpy)
            {
                CurrentInput = ""
            };
            vm.Undo();

            Assert.Equal(0, persistenceSpy.SaveCallCount);
        }

        [Fact]
        public void Undo_WithHistory_ShouldPersistHistoryToFile()
        {
            var persistenceSpy = new PersistenceSpy(mockFileSystem);
            var vm = new CalculatorViewModel(persistenceSpy)
            {
                CurrentInput = "42"
            };
            vm.Enter();
            vm.Undo();

            Assert.Equal(2, persistenceSpy.SaveCallCount);
        }
    }

    [Fact]
    public void CurrentInput_CanBeSetDirectly()
    {
        _vm.CurrentInput = "42";

        Assert.Equal("42", _vm.CurrentInput);
    }

    [Fact]
    public void CanUndo_WithError_ShouldReturnFalse()
    {
        _vm.CurrentInput = "1";
        _vm.Enter();
        _vm.CurrentInput = "abc";
        _vm.Enter();

        Assert.True(_vm.HasError);
        Assert.False(_vm.CanUndo);
    }

    [Fact]
    public void CanUndo_WithHistory_ShouldReturnTrue()
    {
        _vm.CurrentInput = "1";
        _vm.Enter();

        Assert.False(_vm.HasError);
        Assert.True(_vm.CanUndo);
    }

    public class ClearTests : CalculatorViewModelTests
    {
        [Fact]
        public void Clear_WithError_ShouldClearInputAndErrorState()
        {
            _vm.CurrentInput = "abc";
            _vm.Enter();
            _vm.Clear();

            Assert.False(_vm.HasError);
            Assert.Equal("", _vm.CurrentInput);
        }

        [Fact]
        public void Clear_WithError_ShouldClearStackAndErrorState()
        {
            _vm.CurrentInput = "1";
            _vm.Enter();
            _vm.CurrentInput = "abc";
            _vm.Enter();
            _vm.Clear();

            Assert.False(_vm.HasError);
            Assert.Equal(0, _vm.Stack.Count);
        }

        [Fact]
        public void Clear_WithInput_ShouldClearInput()
        {
            _vm.CurrentInput = "1";
            _vm.Clear();

            Assert.False(_vm.HasError);
            Assert.Equal("", _vm.CurrentInput);
            Assert.Equal(0, _vm.Stack.Count);
        }

        [Fact]
        public void Clear_WithStack_ShouldClearStack()
        {
            _vm.CurrentInput = "1";
            _vm.Enter();
            _vm.Clear();

            Assert.False(_vm.HasError);
            Assert.Equal("", _vm.CurrentInput);
            Assert.Equal(0, _vm.Stack.Count);

        }

        [Fact]
        public void Clear_WithInputAndStack_ShouldClearInputAndStack()
        {
            _vm.CurrentInput = "1";
            _vm.Enter();
            _vm.CurrentInput = "2";
            _vm.Clear();

            Assert.False(_vm.HasError);
            Assert.Equal("", _vm.CurrentInput);
            Assert.Equal(0, _vm.Stack.Count);

        }

        [Fact]
        public void Clear_ShouldPersistHistoryToFile()
        {
            var persistenceSpy = new PersistenceSpy(mockFileSystem);
            var vm = new CalculatorViewModel(persistenceSpy)
            {
                CurrentInput = "42"
            };
            vm.Clear();

            Assert.Equal(1, persistenceSpy.SaveCallCount);
        }
    }

    private StackAndInputHistory DummyHistory()
    {
        var history = new StackAndInputHistory();
        var stack = new Stack();
        stack.Push(123);
        history.Push(stack, "1 2 3");
        return history;
    }
}
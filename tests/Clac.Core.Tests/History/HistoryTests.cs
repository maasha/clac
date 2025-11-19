namespace Clac.Core.Tests.History;

using Xunit;
using Clac.Core.History;
using static Clac.Core.ErrorMessages;

public class HistoryTests
{
    public class PushTests
    {
        [Fact]
        public void WithItem_ShouldReturnSuccess()
        {
            History<int> history = new();
            var result = history.Push(42);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void WithItem_ShouldIncreaseCount()
        {
            History<int> history = new();
            Assert.Equal(0, history.Count);
            history.Push(42);
            Assert.Equal(1, history.Count);
        }

        [Fact]
        public void WillNotExceedMaxHistorySize()
        {
            History<int> history = new();
            for (int i = 0; i < 101; i++)
                history.Push(i);
            Assert.Equal(100, history.Count);
        }

        [Fact]
        public void WithMaxHistorySize_ShouldRemoveOldestItem()
        {
            History<int> history = new();
            const int maxHistorySize = 100;
            const int itemsToPush = maxHistorySize + 1;

            for (int i = 0; i < itemsToPush; i++)
                history.Push(i);

            Assert.Equal(maxHistorySize, history.Count);
            var result = history.Pop();
            Assert.Equal(maxHistorySize, result.Value);
        }

        [Fact]
        public void WithCustomMaxHistorySize_ShouldRespectLimit()
        {
            const int customMaxSize = 5;
            History<int> history = new(maxHistorySize: customMaxSize);

            for (int i = 0; i < 10; i++)
                history.Push(i);

            Assert.Equal(customMaxSize, history.Count);
            var result = history.Pop();
            Assert.Equal(9, result.Value); // Last item should be 9
        }

        [Fact]
        public void WithCloneFunc_ShouldCloneItemBeforeSaving()
        {
            var original = new List<int> { 1, 2, 3 };
            History<List<int>> history = new(cloneFunc: list => new List<int>(list));

            history.Push(original);
            original.Add(4);

            var result = history.Pop();
            Assert.True(result.IsSuccessful);
            Assert.Equal([1, 2, 3], result.Value);
        }

        [Fact]
        public void WithValidationFunc_ShouldRejectInvalidItem()
        {
            History<string> history = new(validateFunc: s => !string.IsNullOrWhiteSpace(s));

            var result = history.Push("");

            Assert.False(result.IsSuccessful);
            Assert.Contains(ValidationFailed, result.Error.Message);
            Assert.Equal(0, history.Count);
        }

        [Fact]
        public void WithValidationFunc_ShouldAcceptValidItem()
        {
            History<string> history = new(validateFunc: s => !string.IsNullOrWhiteSpace(s));

            var result = history.Push("valid");

            Assert.True(result.IsSuccessful);
            Assert.Equal(1, history.Count);
        }
    }

    public class PopTests
    {
        [Fact]
        public void WithEmptyHistory_ShouldReturnError()
        {
            History<int> history = new();
            var result = history.Pop();
            Assert.False(result.IsSuccessful);
            Assert.Contains(HistoryIsEmpty, result.Error.Message);
        }

        [Fact]
        public void WithNonEmptyHistory_ShouldReturnSuccess()
        {
            History<int> history = new();
            history.Push(42);
            var result = history.Pop();
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void WithNonEmptyHistory_ShouldDecreaseCount()
        {
            History<int> history = new();
            history.Push(42);
            Assert.Equal(1, history.Count);
            history.Pop();
            Assert.Equal(0, history.Count);
        }

        [Fact]
        public void WithNonEmptyHistory_ShouldReturnItem()
        {
            History<int> history = new();
            history.Push(42);
            var result = history.Pop();
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void WithNonEmptyHistory_ShouldReturnLastItem()
        {
            History<int> history = new();
            history.Push(42);
            history.Push(43);
            var result = history.Pop();
            Assert.Equal(43, result.Value);
        }

        [Fact]
        public void Twice_WithNonEmptyHistory_ShouldReturnSecondLastItem()
        {
            History<int> history = new();
            history.Push(42);
            history.Push(43);
            history.Pop();
            var result = history.Pop();
            Assert.Equal(42, result.Value);
        }
    }

    public class CanUndoTests
    {
        [Fact]
        public void WithNoHistory_ShouldReturnFalse()
        {
            History<int> history = new();
            Assert.False(history.CanUndo);
        }

        [Fact]
        public void WithHistory_ShouldReturnTrue()
        {
            History<int> history = new();
            history.Push(42);
            Assert.True(history.CanUndo);
        }
    }

    public class ToArrayTests
    {
        [Fact]
        public void WithEmptyHistory_ShouldReturnEmptyArray()
        {
            History<int> history = new();
            var array = history.ToArray();
            Assert.Empty(array);
        }

        [Fact]
        public void WithNonEmptyHistory_ShouldReturnItems()
        {
            History<int> history = new();
            history.Push(42);
            history.Push(43);
            var array = history.ToArray();
            Assert.Equal([42, 43], array);
        }
    }

    public class LastTests
    {
        [Fact]
        public void WithEmptyHistory_ShouldReturnError()
        {
            History<int> history = new();
            var result = history.Last();

            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void WithEmptyHistory_ShouldReturnErrorMessage()
        {
            History<int> history = new();
            var result = history.Last();

            Assert.False(result.IsSuccessful);
            Assert.Contains(HistoryIsEmpty, result.Error.Message);
        }

        [Fact]
        public void WithHistory_ShouldReturnLastItem()
        {
            History<int> history = new();
            history.Push(32);
            history.Push(42);
            var result = history.Last();
            Assert.Equal(42, result.Value);
        }
    }
}


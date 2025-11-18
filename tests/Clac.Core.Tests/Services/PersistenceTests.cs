namespace Clac.Core.Tests.Services;

using System.IO.Abstractions.TestingHelpers;
using Xunit;
using Clac.Core.Services;
using Clac.Core.History;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

public class PersistenceTests
{
    protected readonly StackAndInputHistory _history;
    protected readonly MockFileSystem _mockFileSystem;
    protected readonly Persistence _persistence;
    protected static readonly string InvalidPath = "\0/invalid/path/test.json";
    protected static readonly string ValidPath = "/existent/directory/test.json";

    public PersistenceTests()
    {
        _history = new StackAndInputHistory();
        _mockFileSystem = new MockFileSystem();
        _persistence = new Persistence(_mockFileSystem);
    }

    public class SaveTests : PersistenceTests
    {
        [Fact]
        public void Save_WithNullHistory_ShouldDoNothing()
        {
            var result = _persistence.Save(null!);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Save_WithEmptyHistory_ShouldDoNothing()
        {
            var result = _persistence.Save(_history);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Save_WhenFileOperationFails_ShouldReturnError()
        {
            var result = _persistence.Save(_history, InvalidPath);
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void Save_WhenFileOperationFails_ShouldReturnErrorMessage()
        {
            var result = _persistence.Save(_history, InvalidPath);
            Assert.False(result.IsSuccessful);
            Assert.Contains(SavingFailed, result.Error.Message);
        }

        [Fact]
        public void Save_WhenFileOperationSucceeds_ShouldReturnSuccess()
        {
            var result = _persistence.Save(_history, ValidPath);
            Assert.True(result.IsSuccessful);
            Assert.False(_persistence.HasError);
            Assert.True(_mockFileSystem.File.Exists(ValidPath));
        }

        [Fact]
        public void Save_WithNullFilePath_ShouldUseDefaultPath()
        {
            var result = _persistence.Save(_history);
            Assert.True(result.IsSuccessful);
            Assert.False(_persistence.HasError);
        }
    }

    public class ErrorTests : PersistenceTests
    {
        [Fact]
        public void HasError_WhenNoError_ShouldReturnFalse()
        {
            var hasError = _persistence.HasError;
            Assert.False(hasError);
        }

        [Fact]
        public void HasError_WhenError_ShouldReturnTrue()
        {
            _persistence.Save(_history, InvalidPath);
            var hasError = _persistence.HasError;
            Assert.True(hasError);
        }

        [Fact]
        public void GetError_WhenNoErrors_ShouldReturnEmpty()
        {
            var errors = _persistence.GetError();
            Assert.Empty(errors);
        }

        [Fact]
        public void GetError_WhenSaveFailedError_ShouldReturnSaveError()
        {
            _persistence.Save(_history, InvalidPath);
            Assert.Contains(SavingFailed, _persistence.GetError());
        }

        [Fact]
        public void ClearError_WhenNoErrors_ShouldDoNothing()
        {
            var result = _persistence.Save(_history);
            Assert.Empty(_persistence.GetError());
        }

        [Fact]
        public void ClearError_WhenErrorExists_ShouldClearError()
        {
            _persistence.Save(_history, InvalidPath);
            Assert.True(_persistence.HasError);

            _persistence.ClearError();

            Assert.False(_persistence.HasError);
            Assert.Empty(_persistence.GetError());
        }
    }

    public class LoadTests : PersistenceTests
    {
        private const double TestStackValue1 = 123;
        private const double TestStackValue2 = 456;
        private const string TestInput = "1 2 3";

        private Stack CreateTestStack()
        {
            var stack = new Stack();
            stack.Push(TestStackValue1);
            stack.Push(TestStackValue2);
            return stack;
        }

        private StackAndInputHistory CreateTestHistory()
        {
            var history = new StackAndInputHistory();
            history.Push(CreateTestStack(), TestInput);
            return history;
        }

        private void SaveTestHistory(string path)
        {
            var historyToSave = CreateTestHistory();
            var persistenceToSave = new Persistence(_mockFileSystem);
            persistenceToSave.Save(historyToSave, path);
        }

        [Fact]
        public void Load_WhenFileDoesNotExists_ShouldDoNothing()
        {
            var result = _persistence.Load();
            Assert.True(result.IsSuccessful);
            Assert.False(_persistence.HasError);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Load_WhenFileOperationFails_ShouldReturnError()
        {
            _mockFileSystem.AddFile(ValidPath, "invalid json content");
            var result = _persistence.Load(ValidPath);
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void Load_WhenFileOperationFails_ShouldReturnErrorMessage()
        {
            _mockFileSystem.AddFile(ValidPath, "invalid json content");
            var result = _persistence.Load(ValidPath);
            Assert.False(result.IsSuccessful);
            Assert.True(_persistence.HasError);
            Assert.Contains(LoadingFailed, _persistence.GetError());
        }

        [Fact]
        public void Load_WhenFileOperationSucceeds_ShouldReturnHistory()
        {
            var savedHistory = CreateTestHistory();
            SaveTestHistory(ValidPath);
            var result = _persistence.Load(ValidPath);
            Assert.True(result.IsSuccessful);
            Assert.True(LoadedHistoryEqualsSavedHistory(result.Value, savedHistory));
        }

        [Fact]
        public void Load_WithNullFilePath_ShouldUseDefaultPath()
        {
            var result = _persistence.Load();
            Assert.True(result.IsSuccessful);
            Assert.False(_persistence.HasError);
        }

        private static bool LoadedHistoryEqualsSavedHistory(StackAndInputHistory? loadedHistory, StackAndInputHistory savedHistory)
        {
            return HistoryComparer.AreEqual(loadedHistory, savedHistory);
        }
    }
}
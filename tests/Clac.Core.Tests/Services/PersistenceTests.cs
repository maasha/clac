namespace Clac.Core.Tests.Services;

using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Xunit;
using Clac.Core.Services;
using Clac.Core.History;
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
        _persistence = new Persistence(_history, _mockFileSystem);
    }

    public class SaveTests : PersistenceTests
    {
        [Fact]
        public void Save_WithNullHistory_ShouldDoNothing()
        {
            var persistence = new Persistence(null, _mockFileSystem);
            var result = persistence.Save();
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Save_WithEmptyHistory_ShouldDoNothing()
        {
            var result = _persistence.Save("test.json");
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Save_WhenFileOperationFails_ShouldReturnError()
        {
            var result = _persistence.Save(InvalidPath);
            Assert.False(result.IsSuccessful);
        }

        [Fact]
        public void Save_WhenFileOperationFails_ShouldReturnErrorMessage()
        {
            var result = _persistence.Save(InvalidPath);
            Assert.False(result.IsSuccessful);
            Assert.Contains(SavingFailed, result.Error.Message);
        }

        [Fact]
        public void Save_WhenFileOperationSucceeds_ShouldReturnSuccess()
        {
            var result = _persistence.Save(ValidPath);
            Assert.True(result.IsSuccessful);
            Assert.False(_persistence.HasError);
            Assert.True(_mockFileSystem.File.Exists(ValidPath));
        }

        [Fact]
        public void Save_WithNullFilePath_ShouldUseDefaultPath()
        {
            var result = _persistence.Save();
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
            _persistence.Save(InvalidPath);
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
            _persistence.Save(InvalidPath);
            Assert.Contains(SavingFailed, _persistence.GetError());
        }

        [Fact]
        public void ClearError_WhenNoErrors_ShouldDoNothing()
        {
            var result = _persistence.Save();
            Assert.Empty(_persistence.GetError());
        }

        [Fact]
        public void ClearError_WhenErrorExists_ShouldClearError()
        {
            _persistence.Save(InvalidPath);
            Assert.True(_persistence.HasError);

            _persistence.ClearError();

            Assert.False(_persistence.HasError);
            Assert.Empty(_persistence.GetError());
        }
    }

    public class LoadTests : PersistenceTests
    {
        [Fact]
        public void Load_WhenFileDoesNotExists_ShouldDoNothing()
        {
            var result = _persistence.Load();
            Assert.True(result.IsSuccessful);
            Assert.False(_persistence.HasError);
            Assert.Null(result.Value);
        }

        [Fact]
        public void Save_WhenFileOperationFails_ShouldReturnError()
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

        // [Fact]
        // public void Load_WhenFileOperationSucceeds_ShouldReturnSuccess()
        // {
        //     _persistence.Save(ValidPath);
        //     var result = _persistence.Load(ValidPath);
        //     Assert.True(result.IsSuccessful);
        //     Assert.True(_mockFileSystem.File.Exists(ValidPath));
        // }

        [Fact]
        public void Load_WithNullFilePath_ShouldUseDefaultPath()
        {
            var result = _persistence.Load();
            Assert.True(result.IsSuccessful);
            Assert.False(_persistence.HasError);
        }
    }
}
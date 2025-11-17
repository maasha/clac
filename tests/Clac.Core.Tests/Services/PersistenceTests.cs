namespace Clac.Core.Tests.Services;

using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Xunit;
using Clac.Core.Services;
using Clac.Core.History;
using static Clac.Core.ErrorMessages;

public class PersistenceTests
{
    private readonly StackAndInputHistory _history;
    private readonly MockFileSystem _mockFileSystem;
    private readonly Persistence _persistence;
    private static readonly string InvalidPath = "/nonexistent/directory/test.json";

    public PersistenceTests()
    {
        _history = new StackAndInputHistory();
        _mockFileSystem = new MockFileSystem();
        _persistence = new Persistence(_history, _mockFileSystem);
    }

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
        var result = _persistence.Save("test.json");

        Assert.True(result.IsSuccessful);
        Assert.False(_persistence.HasError);
        Assert.True(_mockFileSystem.File.Exists("test.json"));
    }

    [Fact]
    public void Save_WithNullFilePath_ShouldUseDefaultPath()
    {
        var result = _persistence.Save();

        Assert.True(result.IsSuccessful);
        Assert.False(_persistence.HasError);
    }

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
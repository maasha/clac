using System.IO.Abstractions;
using System.Text.Json;
using DotNext;
using Clac.Core.History;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Services;

public class Persistence
{
    private readonly StackAndInputHistory? _history;
    private readonly IFileSystem _fileSystem;
    private string _error = "";

    public Persistence(StackAndInputHistory? history, IFileSystem? fileSystem = null)
    {
        _history = history;
        _fileSystem = fileSystem ?? new FileSystem();
    }

    public bool HasError => !string.IsNullOrEmpty(_error);

    public string GetError()
    {
        return _error;
    }

    public void ClearError()
    {
        _error = "";
    }

    public Result<bool> Save(string? filePath = null)
    {
        if (_history == null)
            return new Result<bool>(true);

        var path = filePath ?? GetDefaultFilePath();

        try
        {
            var directory = _fileSystem.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
                _fileSystem.Directory.CreateDirectory(directory);

            _fileSystem.File.WriteAllText(path, "");
            _error = "";
            return new Result<bool>(true);
        }
        catch (Exception ex)
        {
            _error = $"{SavingFailed}: {ex.Message}";
            return new Result<bool>(new InvalidOperationException(_error));
        }
    }

    public Result<StackAndInputHistory?> Load(string? filePath = null)
    {
        var path = filePath ?? GetDefaultFilePath();

        if (!_fileSystem.File.Exists(path))
        {
            StackAndInputHistory? loadedHistory = null;
            return new Result<StackAndInputHistory?>(loadedHistory);
        }

        try
        {
            var content = _fileSystem.File.ReadAllText(path);
            JsonSerializer.Deserialize<StackAndInputHistory>(content);
            StackAndInputHistory? loadedHistory = null;
            return new Result<StackAndInputHistory?>(loadedHistory);
        }
        catch (Exception ex)
        {
            _error = $"{LoadingFailed}: {ex.Message}";
            return new Result<StackAndInputHistory?>(new InvalidOperationException(_error));
        }
    }

    private string GetDefaultFilePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = _fileSystem.Path.Combine(appDataPath, "Clac");
        _fileSystem.Directory.CreateDirectory(appFolder);
        return _fileSystem.Path.Combine(appFolder, "state.json");
    }
}
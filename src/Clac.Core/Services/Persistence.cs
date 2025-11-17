using System;
using System.Collections;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
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

    public Result<bool> Save(string? filePath = null)
    {
        if (_history == null)
            return new Result<bool>(true);

        var path = filePath ?? GetDefaultFilePath();

        try
        {
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

    private string GetDefaultFilePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = _fileSystem.Path.Combine(appDataPath, "Clac");
        _fileSystem.Directory.CreateDirectory(appFolder);
        return _fileSystem.Path.Combine(appFolder, "state.json");
    }
}
using System.IO.Abstractions;
using System.Text.Json;
using DotNext;
using Clac.Core.History;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Services;

public class Persistence
{
    private static readonly JsonSerializerOptions JsonOptions = new() { IncludeFields = true };

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

            var serializedHistory = SerializeHistory(_history);
            _fileSystem.File.WriteAllText(path, serializedHistory);
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
            var deserialized = JsonSerializer.Deserialize<List<JsonElement>>(content, JsonOptions);

            if (deserialized == null || deserialized.Count != 2)
            {
                _error = $"{LoadingFailed}: Invalid file format";
                return new Result<StackAndInputHistory?>(new InvalidOperationException(_error));
            }

            var inputArray = JsonSerializer.Deserialize<string[]>(deserialized[0].GetRawText(), JsonOptions);
            var stackDataArray = JsonSerializer.Deserialize<double[][]>(deserialized[1].GetRawText(), JsonOptions);

            if (inputArray == null || stackDataArray == null || inputArray.Length != stackDataArray.Length)
            {
                _error = $"{LoadingFailed}: Mismatched array lengths";
                return new Result<StackAndInputHistory?>(new InvalidOperationException(_error));
            }

            var loadedHistory = new StackAndInputHistory();

            // Push items back in order (oldest first, as ToArray() returns oldest first)
            for (int i = 0; i < inputArray.Length; i++)
            {
                var stack = new Rpn.Stack();
                foreach (var value in stackDataArray[i])
                {
                    stack.Push(value);
                }
                loadedHistory.Push(stack, inputArray[i]);
            }

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

    private string SerializeHistory(StackAndInputHistory history)
    {
        var stackHistoryData = history.StackHistory.ToArray().Select(s => s.ToArray()).ToArray();
        var serializedHistory = new List<object>
        {
            history.InputHistory.ToArray(),
            stackHistoryData,
        };

        return JsonSerializer.Serialize(serializedHistory, JsonOptions);
    }

}
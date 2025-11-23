using System.IO.Abstractions;
using System.Text.Json;
using DotNext;
using Clac.Core.History;
using Clac.Core.Rpn;
using static Clac.Core.ErrorMessages;

namespace Clac.Core.Services;

public class Persistence(IFileSystem fileSystem) : IPersistence
{
    private static readonly JsonSerializerOptions JsonOptions = new() { IncludeFields = true };

    private readonly IFileSystem _fileSystem = fileSystem ?? new FileSystem();
    private string _error = "";

    public bool HasError => !string.IsNullOrEmpty(_error);

    public string GetError()
    {
        return _error;
    }

    public void ClearError()
    {
        _error = "";
    }

    public Result<bool> Save(StackAndInputHistory history, string? filePath = null)
    {
        if (history == null)
            return new Result<bool>(true);

        var path = filePath ?? GetDefaultFilePath();
        return WriteHistoryToFile(path, history);
    }

    private Result<bool> WriteHistoryToFile(string path, StackAndInputHistory history)
    {
        try
        {
            WriteSerializedHistoryToFile(path, history);
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

        Console.WriteLine(path);

        if (!_fileSystem.File.Exists(path))
            return ReturnNullHistory();

        try
        {
            var content = _fileSystem.File.ReadAllText(path);
            return DeserializeAndReconstructHistory(content);
        }
        catch (Exception ex)
        {
            _error = $"{LoadingFailed}: {ex.Message}";
            return new Result<StackAndInputHistory?>(new InvalidOperationException(_error));
        }
    }

    private Result<StackAndInputHistory?> ReturnNullHistory()
    {
        StackAndInputHistory? loadedHistory = null;
        return new Result<StackAndInputHistory?>(loadedHistory);
    }

    private void EnsureDirectoryExists(string filePath)
    {
        var directory = _fileSystem.Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
            _fileSystem.Directory.CreateDirectory(directory);
    }

    private void WriteSerializedHistoryToFile(string path, StackAndInputHistory history)
    {
        EnsureDirectoryExists(path);
        _fileSystem.File.WriteAllText(path, SerializeHistory(history));
    }

    private StackAndInputHistory ReconstructHistory(string[] inputArray, double[][] stackDataArray)
    {
        var loadedHistory = new StackAndInputHistory();
        for (int i = 0; i < inputArray.Length; i++)
            loadedHistory.Push(CreateStackFromArray(stackDataArray[i]), inputArray[i]);
        return loadedHistory;
    }

    private static Stack CreateStackFromArray(double[] values)
    {
        var stack = new Rpn.Stack();
        foreach (var value in values)
            stack.Push(value);
        return stack;
    }

    private Result<StackAndInputHistory?> DeserializeAndReconstructHistory(string content)
    {
        var deserialized = JsonSerializer.Deserialize<List<JsonElement>>(content, JsonOptions);

        var formatError = ValidateDeserializedFormat(deserialized);
        if (formatError != null)
            return formatError.Value;

        var (inputArray, stackDataArray) = DeserializeArrays(deserialized!);

        var arrayError = ValidateArrays(inputArray, stackDataArray);
        if (arrayError != null)
            return arrayError.Value;

        var loadedHistory = ReconstructHistory(inputArray!, stackDataArray!);
        return new Result<StackAndInputHistory?>(loadedHistory);
    }

    private Result<StackAndInputHistory?>? ValidateDeserializedFormat(List<JsonElement>? deserialized)
    {
        if (deserialized == null || deserialized.Count != 2)
        {
            _error = $"{LoadingFailed}: Invalid file format";
            return new Result<StackAndInputHistory?>(new InvalidOperationException(_error));
        }
        return null;
    }

    private (string[]? inputArray, double[][]? stackDataArray) DeserializeArrays(List<JsonElement> deserialized)
    {
        var inputArray = JsonSerializer.Deserialize<string[]>(deserialized[0].GetRawText(), JsonOptions);
        var stackDataArray = JsonSerializer.Deserialize<double[][]>(deserialized[1].GetRawText(), JsonOptions);
        return (inputArray, stackDataArray);
    }

    private Result<StackAndInputHistory?>? ValidateArrays(string[]? inputArray, double[][]? stackDataArray)
    {
        if (inputArray == null || stackDataArray == null || inputArray.Length != stackDataArray.Length)
        {
            _error = $"{LoadingFailed}: Mismatched array lengths";
            return new Result<StackAndInputHistory?>(new InvalidOperationException(_error));
        }
        return null;
    }

    private string GetDefaultFilePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = _fileSystem.Path.Combine(appDataPath, "Clac");
        _fileSystem.Directory.CreateDirectory(appFolder);
        return _fileSystem.Path.Combine(appFolder, "state.json");
    }

    private static string SerializeHistory(StackAndInputHistory history)
    {
        var stackHistoryData = history.StackHistory.ToArray().Select(s => s.ToArray()).ToArray();
        var serializedHistory = CreateSerializedHistoryList(history.InputHistory.ToArray(), stackHistoryData);
        return JsonSerializer.Serialize(serializedHistory, JsonOptions);
    }

    private static List<object> CreateSerializedHistoryList(string[] inputHistory, double[][] stackHistoryData)
    {
        return [inputHistory, stackHistoryData];
    }
}
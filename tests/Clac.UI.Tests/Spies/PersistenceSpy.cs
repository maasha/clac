using Clac.Core.Services;
using DotNext;
using Clac.Core.History;
using System.IO.Abstractions;
using Clac.Core.Rpn;

namespace Clac.UI.Tests.Spies;

public class PersistenceSpy : IPersistence
{
    public int SaveCallCount { get; private set; }
    public int LoadCallCount { get; private set; }
    public Result<StackAndInputHistory?> LoadResultToReturn { get; set; }

    public PersistenceSpy(IFileSystem fileSystem)
    {
        _ = fileSystem;
    }

    public Result<bool> Save(StackAndInputHistory history, string? filePath = null)
    {
        SaveCallCount++;
        return new Result<bool>(true);
    }

    public Result<StackAndInputHistory?> Load(string? filePath = null)
    {
        LoadCallCount++;
        return LoadResultToReturn;
    }

    public bool HasError => false;
    public string GetError() => "";
    public void ClearError() { }
}


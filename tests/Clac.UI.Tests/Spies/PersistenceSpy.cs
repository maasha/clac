using Clac.Core.Services;
using DotNext;
using Clac.Core.History;
using System.IO.Abstractions;

namespace Clac.UI.Tests.Spies;

public class PersistenceSpy : IPersistence
{
    public int SaveCallCount { get; private set; }

    public PersistenceSpy(IFileSystem fileSystem)
    {
        _ = fileSystem;
    }

    public Result<bool> Save(StackAndInputHistory history, string? filePath = null)
    {
        SaveCallCount++;
        return new Result<bool>(true);
    }

    public Result<StackAndInputHistory?> Load(string? filePath = null) => new((StackAndInputHistory?)null);
    public bool HasError => false;
    public string GetError() => "";
    public void ClearError() { }
}


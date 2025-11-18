using Clac.Core.Services;
using DotNext;
using Clac.Core.History;

namespace Clac.Core.Tests.History;

public class PersistenceSpy : IPersistence
{
    public int SaveCallCount { get; private set; }

    public Result<bool> Save(StackAndInputHistory history, string? filePath = null)
    {
        SaveCallCount++;
        return new Result<bool>(true);
    }

    public Result<StackAndInputHistory?> Load(string? filePath = null) => new Result<StackAndInputHistory?>((StackAndInputHistory?)null);
    public bool HasError => false;
    public string GetError() => "";
    public void ClearError() { }
}


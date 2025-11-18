using DotNext;
using Clac.Core.History;

namespace Clac.Core.Services;

public interface IPersistence
{
    Result<bool> Save(StackAndInputHistory history, string? filePath = null);
    Result<StackAndInputHistory?> Load(string? filePath = null);
    bool HasError { get; }
    string GetError();
    void ClearError();
}


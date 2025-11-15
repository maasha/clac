using DotNext;
namespace Clac.Core;

public class RpnStackHistory
{
    public int Size { get; private set; }

    public RpnStackHistory()
    {
        Size = 0;
    }

    public Result<bool> SaveStackSnapShot(RpnStack stack)
    {
        if (stack.Count == 0)
            return new Result<bool>(new InvalidOperationException(ErrorMessages.StackEmpty));

        Size++;
        return new Result<bool>(true);
    }
}

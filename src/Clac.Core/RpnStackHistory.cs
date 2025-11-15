namespace Clac.Core;

public class RpnStackHistory
{
    public int Size { get; private set; }

    public RpnStackHistory()
    {
        Size = 0;
    }

    public void SaveStackSnapShot(RpnStack stack)
    { }
}

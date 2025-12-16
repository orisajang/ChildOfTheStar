using UnityEngine;

public enum TileStatus
{
    Frenzy,
    Recovery,
    Growth,
    Rebirth,
    Destruction,
}
public abstract class TileStatusBase
{
    public abstract void Execute();
}

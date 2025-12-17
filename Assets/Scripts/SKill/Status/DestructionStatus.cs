using UnityEngine;

[CreateAssetMenu(fileName = "Destruction", menuName = "Scriptable Objects/Destruction")]
public class Destruction : TileStatusBase
{
    public override void Execute(Tile[,] board, Tile casterTile)
    {
        Debug.Log("파괴 예약");

    }
}

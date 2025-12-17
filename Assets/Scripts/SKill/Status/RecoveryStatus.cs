using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryStatus", menuName = "Scriptable Objects/Status/RecoveryStatus")]
public class RecoveryStatus : TileStatusBase
{
    [SerializeField] private int _healAmout = 2;
    public override void Execute(Tile[,] board, Tile casterTile)//player 인자 필요함. 추후 추가.
    {
        //player.Heal(_healAmout);

        Debug.Log("체력 회복");
    }
}

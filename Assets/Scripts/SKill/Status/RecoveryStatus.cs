using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryStatus", menuName = "Scriptable Objects/Status/RecoveryStatus")]
public class RecoveryStatus : TileStatusBase
{
    [SerializeField] private int _healAmout = 2;
    public override void Execute(Tile[,] board, Tile casterTile)//player 인자 필요함. 추후 추가.
    {

        Debug.Log("회복으로 인한 플레이어 체력 2 회복");

        PlayerManager.Instance._player.TakeHeal(_healAmout);
    }
}

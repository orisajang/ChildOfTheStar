using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryStatus", menuName = "Scriptable Objects/Status/RecoveryStatus")]
public class RecoveryStatus : TileStatusBase
{
    [SerializeField] private int _healAmout = 2;
    public override void Execute(Tile[,] board, Tile casterTile)
    {

        SkillManager.Instance.TileEventBus.TriggerEvent(TileStatus);

        PlayerManager.Instance._player.TakeHeal(_healAmout);
    }
}

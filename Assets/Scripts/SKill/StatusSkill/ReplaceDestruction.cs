using UnityEngine;

[CreateAssetMenu(fileName = "ReplaceDestruction", menuName = "Scriptable Objects/ActionSkill/ReplaceDestruction")]
public class ReplaceDestruction : TileSkillBase
{
    [SerializeField] private TileStatus _statusType = TileStatus.Destruction;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        casterTile.replaceWillDestroy = true;
        casterTile.replaceDestruction = true;
    }
}
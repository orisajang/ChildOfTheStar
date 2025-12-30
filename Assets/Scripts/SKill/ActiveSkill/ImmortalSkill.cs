using UnityEngine;

[CreateAssetMenu(fileName = "ImmortalSkill", menuName = "Scriptable Objects/Skill/ImmortalSkill")]
public class ImmortalSkill : TileSkillBase
{
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        PlayerManager.Instance._player.OverHealToshield = true;
    }
}

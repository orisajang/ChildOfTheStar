using UnityEngine;

[CreateAssetMenu(fileName = "ImmortalSkill", menuName = "Scriptable Objects/Skill/ImmortalSkill")]
public class ImmortalSkill : TileSkillBase
{
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        Debug.Log($"플레이어 불사");
        PlayerManager.Instance._player.OverHealToshield = true;
    }
}

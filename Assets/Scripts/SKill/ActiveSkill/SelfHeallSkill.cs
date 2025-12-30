using UnityEngine;

[CreateAssetMenu(fileName = "SelfHeallSkill", menuName = "Scriptable Objects/Skill/SelfHeallSkill")]
public class SelfHeallSkill : TileSkillBase
{
    [SerializeField]private int _heal;
    public int Heal=> _heal;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int growthValue = casterTile.GetApplyGrowth(_heal);
        PlayerManager.Instance._player.TakeHeal(growthValue);
    }
}

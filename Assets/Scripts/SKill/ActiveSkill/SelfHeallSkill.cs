using UnityEngine;

[CreateAssetMenu(fileName = "SelfHeallSkill", menuName = "Scriptable Objects/Skill/SelfHeallSkill")]
public class SelfHeallSkill : TileSkillBase
{
    [SerializeField]private int _heal;
    public int Heal=> _heal;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int growthValue = casterTile.GetApplyGrowth(_heal);
        Debug.Log($"플레이어 {growthValue} 회복");
        PlayerManager.Instance._player.TakeHeal(growthValue);
    }
}

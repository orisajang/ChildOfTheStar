using UnityEngine;

[CreateAssetMenu(fileName = "OverChargeOverflowSkill", menuName = "Scriptable Objects/Skill/OverChargeOverflowSkill")]
public class OverChargeOverflowSkill : TileSkillBase
{
    [SerializeField]private int _damage=1;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {

        var monsters = MonsterManager.Instance.SpawnedMonster;
        if (monsters == null || monsters.Count <= 0) return;
        int overFlow = SkillManager.Instance.BoardController.BoardModel.GetChargeOverflow() *_damage;

        int growthValue = casterTile.GetApplyGrowth(_damage);
        Debug.Log($"모든 적에게 {growthValue} 피해");
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            monsters[i].TakeDamage(growthValue);
        }
    }
}

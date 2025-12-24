using UnityEngine;

[CreateAssetMenu(fileName = "AllTargetDamageSkill", menuName = "Scriptable Objects/Skill/AllTargetDamageSkill")]
public class AllTargetDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        var monsters = MonsterManager.Instance.SpawnedMonster;
        if (monsters == null
            || monsters.Count <= 0)
        {
            return;
        }

        int growthValue = casterTile.GetApplyGrowth(_damage);
        Debug.Log($"모든 적에게 {growthValue} 피해");
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            monsters[i].TakeDamage(growthValue);
        }

    }
}

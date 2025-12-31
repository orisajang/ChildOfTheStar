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

        TileColor casterColor = TileColor.White;
        if (casterTile != null)
            casterColor = casterTile.Color;

        int growthValue = casterTile.GetApplyGrowth(_damage);
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            monsters[i].TakeDamage(growthValue,casterColor);
        }

    }
}

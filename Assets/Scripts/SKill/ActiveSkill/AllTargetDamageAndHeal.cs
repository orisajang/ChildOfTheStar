using UnityEngine;

[CreateAssetMenu(fileName = "AllTargetDamageAndHeal", menuName = "Scriptable Objects/Skill/AllTargetDamageAndHeal")]
public class AllTargetDamageAndHeal : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        var monsters = MonsterManager.Instance.SpawnedMonster;
        if (monsters == null|| monsters.Count <= 0) return;

        TileColor casterColor = TileColor.White;
        if (casterTile != null)
            casterColor = casterTile.Color;

        int totalHealAmount = 0;
        int growthValue = casterTile.GetApplyGrowth(_damage);
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            monsters[i].TakeDamage(growthValue, casterColor);
            totalHealAmount += growthValue;
           
        }
        if (totalHealAmount > 0)
        {
            PlayerManager.Instance._player.TakeHeal(totalHealAmount);
        }
    }
}

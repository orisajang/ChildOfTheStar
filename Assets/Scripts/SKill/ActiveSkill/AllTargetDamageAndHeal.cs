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

        int totalHealAmount = 0;
        int growthValue = casterTile.GetApplyGrowth(_damage);
        Debug.Log($"모든 적에게 {growthValue} 피해");
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            monsters[i].TakeDamage(growthValue);
            totalHealAmount += growthValue;
           
        }
        if (totalHealAmount > 0)
        {
            Debug.Log($"총 {totalHealAmount}만큼 회복");
            PlayerManager.Instance._player.TakeHeal(totalHealAmount);
        }
    }
}

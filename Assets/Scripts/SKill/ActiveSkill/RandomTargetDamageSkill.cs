using UnityEngine;

[CreateAssetMenu(fileName = "RandomTargetDamageSkill", menuName = "Scriptable Objects/Skill/RandomTargetDamageSkill")]
public class RandomTargetDamageSkill : TileSkillBase
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
        Debug.Log($"랜덤 적에게 {growthValue} 피해");

        int randTarget = Random.Range(0, monsters.Count);
        monsters[randTarget].TakeDamage(growthValue);
    }
}

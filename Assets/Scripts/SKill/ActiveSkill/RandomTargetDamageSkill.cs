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

        int randTarget = Random.Range(0, monsters.Count);

        if (monsters[randTarget] != null)
            monsters[randTarget].TakeDamage(_damage);
#if UNITY_EDITOR
        Debug.Log($"랜덤타겟 {monsters[randTarget].name}에게 {growthValue}의 피해를 입혔습니다.");
#endif
    }
}

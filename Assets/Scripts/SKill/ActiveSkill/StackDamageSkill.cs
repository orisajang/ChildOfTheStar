using UnityEngine;

[CreateAssetMenu(fileName = "StackDamageSkill", menuName = "Scriptable Objects/Skill/StackDamageSkill")]
public class StackDamageSkill : TileSkillBase
{
    [Tooltip("스택당 데미지")]
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int growthValue = casterTile.GetApplyGrowth(1);
        int finalValue = growthValue * SkillManager.Instance.GetStack(casterTile.TileData.Id) * _damage;
        SkillManager.Instance.AddStack(casterTile.TileData.Id);

        var targetMonster = MonsterManager.Instance._targetMonster;

        if(targetMonster == null )
        {
            var monsters = MonsterManager.Instance.SpawnedMonster;
            if (monsters == null || monsters.Count <= 0)
            {
                return;
            }
            int randTarget = Random.Range(0, monsters.Count);
            targetMonster = monsters[randTarget];
        }

        if (targetMonster != null)
        {
            targetMonster.TakeDamage(finalValue);
        }


    }
}

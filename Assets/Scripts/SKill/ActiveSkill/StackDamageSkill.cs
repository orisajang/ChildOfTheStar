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

        Debug.Log($"적에게 몰아치는 샛별 {finalValue} 피해");

        if(MonsterManager.Instance._targetMonster != null)
        {
            MonsterManager.Instance._targetMonster.TakeDamage(finalValue);
        }
    }
}

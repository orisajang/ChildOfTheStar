using UnityEngine;

[CreateAssetMenu(fileName = "OverChargeDamageSkill", menuName = "Scriptable Objects/Skill/OverChargeDamageSkill")]
public class OverChargeDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        
        int growthValue = casterTile.GetApplyGrowth(_damage)* SkillManager.Instance.BoardController.BoardModel.OverChargeValue;
       
        if (MonsterManager.Instance._targetMonster != null)
        {
            MonsterManager.Instance._targetMonster.TakeDamage(growthValue);
        }
    }
}

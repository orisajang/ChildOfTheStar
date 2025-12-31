using UnityEngine;

[CreateAssetMenu(fileName = "OverChargeDamageSkill", menuName = "Scriptable Objects/Skill/OverChargeDamageSkill")]
public class OverChargeDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        TileColor casterColor = TileColor.White;
        if (casterTile != null)
            casterColor = casterTile.Color;

        int growthValue = casterTile.GetApplyGrowth(_damage)* SkillManager.Instance.BoardController.BoardModel.OverChargeValue;


        var targetMonster = MonsterManager.Instance._targetMonster;

        if (targetMonster == null)
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
            targetMonster.TakeDamage(growthValue, casterColor);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NormalDamageSkill", menuName = "Scriptable Objects/Skill/NormalDamageSkill")]
public class NormalDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        TileColor casterColor = TileColor.White;
        if (casterTile != null)
            casterColor = casterTile.Color;

        int growthValue=_damage;
        if (casterTile != null)
        {
            growthValue = casterTile.GetApplyGrowth(_damage);
        }
        //여기서 몬스터매니저를 통해서 데미지 처리
        //이미 데미지 처리 받아서 죽은 경우가 아닐때만 데미지를 주자
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

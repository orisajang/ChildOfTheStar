using UnityEngine;

[CreateAssetMenu(fileName = "NormalDamageSkill", menuName = "Scriptable Objects/Skill/NormalDamageSkill")]
public class NormalDamageSkill : TileSkillBase
{
    [SerializeField]private int _damage;
    public int Damage=> _damage;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        Debug.Log($"적에게 {_damage} 피해");
        //여기서 몬스터매니저를 통해서 데미지 처리
        //이미 데미지 처리 받아서 죽은 경우가 아닐때만 데미지를 주자
        if(MonsterManager.Instance._targetMonster != null)
        {
            MonsterManager.Instance._targetMonster.TakeDamage(_damage);
        }
    }
}

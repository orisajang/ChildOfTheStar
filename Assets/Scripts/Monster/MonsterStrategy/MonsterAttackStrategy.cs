using UnityEngine;

public class MonsterAttackStrategy : MonsterStrategy
{
    public override void MonsterActDo(Monster monster, MonsterActionCycleValue action)
    {
        Debug.Log("AttackDo");
        monster.MonsterAttacktypeDic[action.monsterActionData.attackType].DoAttack(monster, action);
        //애니메이션 재생
        monster.MonsterAnimatorChange(MonsterAnimatorParameterName.Attack);
        //소리 재생
        SoundManager.Instance.PlayEffect(action.monsterActionData.sound);
    }
}

using UnityEngine;

public class MonsterAttackStrategy : MonsterStrategy
{
    public override void MonsterActDo(Monster monster, MonsterActionCycleValue action)
    {
        Debug.Log("AttackDo");
        monster.MonsterAttacktypeDic[action.monsterActionData.attackType].DoAttack(monster, action);
        monster.MonsterAnimatorChange(MonsterAnimatorParameterName.Attack);
    }
}

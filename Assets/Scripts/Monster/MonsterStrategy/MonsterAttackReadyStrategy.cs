using UnityEngine;

public class MonsterAttackReadyStrategy : MonsterStrategy
{
    public override void MonsterActDo(Monster monster, MonsterActionCycleValue action)
    {
        //현재 아무것도안함. 추후 sprite이미지, sound, effect만 변경되면 될듯 
        Debug.Log("AttackReadyAct");
        monster.MonsterAnimatorChange(MonsterAnimatorParameterName.AttackReady);
    }
}

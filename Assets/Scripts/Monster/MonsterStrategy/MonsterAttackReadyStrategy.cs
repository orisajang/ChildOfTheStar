using UnityEngine;

public class MonsterAttackReadyStrategy : MonsterStrategy
{
    public override void MonsterActDo(Monster monster, MonsterActionCycleValue action)
    {
        //현재 아무것도안함. 추후 sprite이미지, sound, effect만 변경되면 될듯 
        //Debug.Log("AttackReadyAct");
        //애니메이션 재생
        monster.MonsterAnimatorChange(MonsterAnimatorParameterName.AttackReady);
        //소리 재생
        SoundManager.Instance.PlayEffect(action.monsterActionData.sound);
    }
}

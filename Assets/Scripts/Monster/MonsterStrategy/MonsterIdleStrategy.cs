using UnityEngine;

public class MonsterIdleStrategy : MonsterStrategy
{
    public override void MonsterActDo(Monster monster, MonsterActionCycleValue action)
    {
        //현재 아무것도안함. 추후 sprite이미지, sound, effect만 변경되면 될듯 
        Debug.Log("MonsterIdelAct");
        //몬스터의 애니메이션을 설정한다
        //monster.MonsterAnimatorChange(); //아무것도 안해도됨 
    }
}

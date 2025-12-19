using UnityEngine;

/// <summary>
/// 몬스터 힐일때 아래 코드 사용
/// </summary>
public class MonsterHealSelfAction : MonsterAttackBehaviorStrategy
{
    public override void DoAttack(Monster monster, MonsterActionCycleValue monsterActionValue)
    {
        int healAmount = BattleManager.Instance.CalcMonsterHeal(monster._monsterAttackPower, monsterActionValue.monsterActionData.attackValue);
        monster.MonsterHealSelf(healAmount);
        Debug.Log("몬스터 자힐");
    }
}
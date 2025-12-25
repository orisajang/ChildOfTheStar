using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return; //이거도 추가
    }

    /// <summary>
    /// 몬스터의 공격데미지 계산 공격*공격수치
    /// </summary>
    /// <param name="attack">공격력</param>
    /// <param name="attackValue">공격수치</param>
    /// <returns></returns>
    public int CalcMonsterDamage(int attack, float attackValue)
    {
        //소수점이 있을경우 반올림 처리로

        //계산한 후에 반올림한 int값을 보낸다
        float floatDamage = attack * attackValue;
        return Mathf.RoundToInt(floatDamage);
    }
    /// <summary>
    /// 몬스터의 자힐량 계산 최대체력*공격수치
    /// </summary>
    /// <param name="maxHp">최대체력</param>
    /// <param name="attackValue">공격수치</param>
    /// <returns></returns>
    public int CalcMonsterHeal(int maxHp, float attackValue)
    {
        //소수점이 있을경우 반올림 처리로

        //계산한 후에 반올림한 int값을 보낸다
        float floatHealAmount = maxHp * attackValue;
        return Mathf.RoundToInt(floatHealAmount);
    }


}

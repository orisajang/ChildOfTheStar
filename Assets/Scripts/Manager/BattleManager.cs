using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 공격자의 공격력과 피격자의 hp를 이용해서 피격자의 남은 hp 반환
    /// </summary>
    /// <returns></returns>
    public int CalcRemainHp(int attack, int hp)
    {
        hp -= attack;

        if (hp < 0) hp = 0;

        return hp;
    }



}

using UnityEngine;

public class MonsterAttackPlayer : MonsterAttackBehaviorStrategy
{
    public override void DoAttack(Monster monster, MonsterActionCycleValue monsterActionValue)
    {
        int damage = BattleManager.Instance.CalcMonsterDamage(monster._monsterAttackPower, monsterActionValue.monsterActionData.attackValue);
        PlayerManager.Instance._player.TakeDamage(damage);
        //Debug.Log($"플레이어에게 데미지줌 {damage}");
        //몬스터는 공격 이펙트가 없음
        //monster.MonsterEffectPlay(monsterActionValue.monsterActionData.effect);
    }
}

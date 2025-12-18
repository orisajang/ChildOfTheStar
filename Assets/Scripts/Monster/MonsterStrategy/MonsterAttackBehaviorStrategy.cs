using UnityEngine;

public abstract class MonsterAttackBehaviorStrategy
{
    public abstract void DoAttack(Monster monster, MonsterActionCycleValue monsterActionValue);
}

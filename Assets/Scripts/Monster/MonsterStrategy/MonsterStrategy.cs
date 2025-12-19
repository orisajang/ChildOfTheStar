using UnityEngine;

public abstract class MonsterStrategy
{
    public abstract void MonsterActDo(Monster monster, MonsterActionCycleValue action);
}

using System.Collections.Generic;

public struct MonsterActionCycleValue
{
    public int id;
    public int step;
    public int actionId;

    public MonsterActionCSVData monsterActionData;
    public void SetMonsterActionData(MonsterActionCSVData data)
    {
        monsterActionData = data;
    }
}

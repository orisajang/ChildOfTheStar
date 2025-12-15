using System.Collections.Generic;
using UnityEngine;

public class MonsterCSVData
{
    //CSV파일에 저장되어있는 속성들
    public int monsterId;
    public string monsterName;
    public eMonsterType monsterType;
    public eMonsterSize monsterSize;
    public int monsterHp;
    public int monsterAttackPower;
    public int monsterMaxEnergy;
    public int monsterCycleId;
    public string monsterAnimation;
    public string monsterSound;

    ///어떤 프리팹을 쓸건지
    public GameObject monsterPrefab;
    //Cycle ID를 통해 ActionCycle들을 가지고있어야해서 추가
    public List<MonsterActionCycleCSVData> monsterActionCycleList;
}

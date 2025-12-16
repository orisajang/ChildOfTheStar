using System.Collections.Generic;
using UnityEngine;

public struct StageCSVData
{
    //CSV파일에 저장되어있는 속성들
    public string stageId;
    public string monsterWaveId;
    public bool isDimdis;

    public List<MonsterWaveCSVData> monsterWaveList;

    public void AddMonsterWaveList(MonsterWaveCSVData waveData)
    {
        if (monsterWaveList == null) monsterWaveList = new List<MonsterWaveCSVData>();
        monsterWaveList.Add(waveData);
    }
}
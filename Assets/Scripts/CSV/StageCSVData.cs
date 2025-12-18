using System.Collections.Generic;
using UnityEngine;

public struct StageCSVData
{
    //CSV파일에 저장되어있는 속성들
    public string stageId;
    public string monsterWaveId1;
    public string monsterWaveId2;
    public string monsterWaveId3;
    public bool isDimdis;

    public List<MonsterWaveCSVData> monsterWaveList;

    public void AddMonsterWaveList(MonsterWaveCSVData waveData)
    {
        if (monsterWaveList == null) monsterWaveList = new List<MonsterWaveCSVData>();
        monsterWaveList.Add(waveData);
    }
    /// <summary>
    /// 몬스터 웨이브ID를 전부 가져올수있도록
    /// </summary>
    /// <returns></returns>
    public string[] GetMonsterWaveIdArray()
    {
        string[] monsterWaveIdArray = new string[]
        {
            monsterWaveId1,
            monsterWaveId2,
            monsterWaveId3
        };
        return monsterWaveIdArray;
    }
}
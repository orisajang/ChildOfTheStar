using System.Collections.Generic;
using System;
using UnityEngine;

public class MonsterActionCycleCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    List<MonsterActionCycleCSVData> monsterActionCycleCSVDataList = new List<MonsterActionCycleCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    public List<MonsterActionCycleCSVData> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv, 3);

        SetData(csv.Data);
        AssignPrefabs();

        return monsterActionCycleCSVDataList;
    }

    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {
            MonsterActionCycleCSVData monsterActionCycleData = new MonsterActionCycleCSVData();
            monsterActionCycleData.id = int.Parse(item[0]);
            monsterActionCycleData.groupId = int.Parse(item[1]);
            monsterActionCycleData.step = int.Parse(item[2]);
            monsterActionCycleData.actionId = int.Parse(item[3]);

            //monsterData.prefabKey = item[4];
            monsterActionCycleCSVDataList.Add(monsterActionCycleData);
        }
    }
    private void AssignPrefabs()
    {
        foreach (var w in monsterActionCycleCSVDataList)
        {
            //w.weaponPrefab = PrefabManager.Instance.GetPrefab(w.prefabKey);
        }

        Debug.Log("Prefab 연결 완료");
    }
}

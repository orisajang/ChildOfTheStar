using System.Collections.Generic;
using System;
using UnityEngine;

public class MonsterCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    List<MonsterCSVData> monsterCSVDataList = new List<MonsterCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    public List<MonsterCSVData> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv,3);

        SetData(csv.Data);
        AssignPrefabs();

        return monsterCSVDataList;
    }

    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {
            MonsterCSVData monsterData = new MonsterCSVData();
            monsterData.monsterId = int.Parse(item[0]);
            monsterData.monsterName = item[1];
            monsterData.monsterType = (eMonsterType)Enum.Parse(typeof(eMonsterType), item[2]);
            monsterData.monsterSize = (eMonsterSize)Enum.Parse(typeof(eMonsterSize), item[3]);
            monsterData.monsterHp = int.Parse(item[4]);
            monsterData.monsterAttackPower = int.Parse(item[5]);
            monsterData.monsterMaxEnergy = int.Parse(item[6]);
            monsterData.monsterCycleId = int.Parse(item[7]);
            monsterData.monsterAnimation = item[8];
            monsterData.monsterSound = item[9];



            //monsterData.prefabKey = item[4];
            monsterCSVDataList.Add(monsterData);
        }
    }
    private void AssignPrefabs()
    {
        foreach (var w in monsterCSVDataList)
        {
            //w.weaponPrefab = PrefabManager.Instance.GetPrefab(w.prefabKey);
        }

        Debug.Log("Prefab 연결 완료");
    }
}

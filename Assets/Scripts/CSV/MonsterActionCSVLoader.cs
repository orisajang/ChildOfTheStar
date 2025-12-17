using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class MonsterActionCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    //List<MonsterActionCSVData> monsterActionCSVDataList = new List<MonsterActionCSVData>();
    Dictionary<int, MonsterActionCSVData> monsterActionCSVDataDic = new Dictionary<int, MonsterActionCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    public Dictionary<int, MonsterActionCSVData> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv, 3);

        SetData(csv.Data);
        AssignPrefabs();

        return monsterActionCSVDataDic;
    }

    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {
            MonsterActionCSVData monsterActionData = new MonsterActionCSVData();
            monsterActionData.id = int.Parse(item[0]);
            monsterActionData.actionName = item[1];
            monsterActionData.actionType = (eMonsterAction)Enum.Parse(typeof(eMonsterAction), item[2]);
            monsterActionData.attackType = (eMonsterAttackType)Enum.Parse(typeof(eMonsterAttackType), item[3]);
            monsterActionData.attackValue = float.Parse(item[4]);
            monsterActionData.actionmon_id = int.Parse(item[5]);
            monsterActionData.animation = item[6];
            monsterActionData.effect = item[7];
            monsterActionData.sound = item[8];

            //monsterData.prefabKey = item[4];
            //monsterActionCSVDataList.Add(monsterActionData);
            monsterActionCSVDataDic.Add(monsterActionData.id, monsterActionData);
        }
    }
    private void AssignPrefabs()
    {
        //foreach (var w in monsterActionCSVDataList)
        {
            //w.weaponPrefab = PrefabManager.Instance.GetPrefab(w.prefabKey);
        }

        Debug.Log("Prefab 연결 완료");
    }
}

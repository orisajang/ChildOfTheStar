using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class MonsterActionCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    List<MonsterActionCSVData> monsterActionCSVDataList = new List<MonsterActionCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    public List<MonsterActionCSVData> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv, 3);

        SetData(csv.Data);
        AssignPrefabs();

        return monsterActionCSVDataList;
    }

    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {
            MonsterActionCSVData monsterActionData = new MonsterActionCSVData();
            monsterActionData.id = int.Parse(item[0]);
            monsterActionData.actionName = item[1];
            monsterActionData.actionType = (eMonsterAction)Enum.Parse(typeof(eMonsterAction), item[2]);
            monsterActionData.actionMonsterId = int.Parse(item[3]);
            monsterActionData.actionTarget = int.Parse(item[4]);
            monsterActionData.actionCost = int.Parse(item[5]);
            monsterActionData.isAttack = item[6] == "0" ? false : true;
            monsterActionData.isRestore = item[7] == "0" ? false : true;
            monsterActionData.isSpecial = item[8] == "0" ? false : true;
            monsterActionData.specialValue = item[9] == "null" ? 0 : float.Parse(item[9]);
            //monsterActionData.sprite = item[10];
            monsterActionData.animation = item[10];
            monsterActionData.effect = item[11];
            monsterActionData.sound = item[12];


            //monsterData.prefabKey = item[4];
            monsterActionCSVDataList.Add(monsterActionData);
        }
    }
    private void AssignPrefabs()
    {
        foreach (var w in monsterActionCSVDataList)
        {
            //w.weaponPrefab = PrefabManager.Instance.GetPrefab(w.prefabKey);
        }

        Debug.Log("Prefab 연결 완료");
    }
}

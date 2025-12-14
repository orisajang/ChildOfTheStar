using System.Collections.Generic;
using System;
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
    public string monsterSprite;
    public string monsterAnimation;
    public string monsterSound;

    ///어떤 프리팹을 쓸건지
    public GameObject monsterPrefab;
}
public class MonsterCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    List<MonsterCSVData> weaponCSVDataList = new List<MonsterCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    public List<MonsterCSVData> LoadWeaponData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv,3);

        SetData(csv.Data);
        AssignPrefabs();

        return weaponCSVDataList;
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
            monsterData.monsterSprite = item[8];
            monsterData.monsterAnimation = item[9];
            monsterData.monsterSound = item[10];



            //monsterData.prefabKey = item[4];
            weaponCSVDataList.Add(monsterData);
        }
    }
    private void AssignPrefabs()
    {
        foreach (var w in weaponCSVDataList)
        {
            //w.weaponPrefab = PrefabManager.Instance.GetPrefab(w.prefabKey);
        }

        Debug.Log("Prefab 연결 완료");
    }
}

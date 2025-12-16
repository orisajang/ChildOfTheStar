using System.Collections.Generic;
using System;
using UnityEngine;

public class MonsterActionCycleCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    //List<MonsterActionCycleCSVData> monsterActionCycleCSVDataList = new List<MonsterActionCycleCSVData>();
    public Dictionary<int, List<MonsterActionCycleValue>> monsterActionCycleData;

    //생성자
    public MonsterActionCycleCSVLoader()
    {
        monsterActionCycleData = new Dictionary<int, List<MonsterActionCycleValue>>();
    }
    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    public Dictionary<int, List<MonsterActionCycleValue>> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv, 3);

        SetData(csv.Data);
        AssignPrefabs();

        return monsterActionCycleData;
    }

    /// <summary>
    /// CSV 데이터를 딕셔너리에 정리 함
    /// </summary>
    /// <param name="Data"></param>
    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {

            AddData(int.Parse(item[1]), int.Parse(item[0]), int.Parse(item[2]), int.Parse(item[3]));

        }
    }
    private void AssignPrefabs()
    {
        var keys = monsterActionCycleData.Keys;
        foreach (var w in keys)
        {
            //w.weaponPrefab = PrefabManager.Instance.GetPrefab(w.prefabKey);
        }

        Debug.Log("Prefab 연결 완료");
    }

    /// <summary>
    /// 딕셔너리에 데이터 넣는 함수. 
    /// 데이터는 스트럭트 타입으로 넣음
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="id"></param>
    /// <param name="step"></param>
    /// <param name="actionId"></param>
    private void AddData(int groupId, int id, int step, int actionId)
    {
        MonsterActionCycleValue newData = new MonsterActionCycleValue();
        newData.actionId = actionId;
        newData.id = id;
        newData.step = step;

        if (monsterActionCycleData == null)
        {
            Debug.LogError($"MonsterActionCycleCSVData : AddData(int groupId, int id, int step, int actionId) 에서 CycleData 가 초기화 되어 있지 않습니다.");
            return;
        }
        if (monsterActionCycleData.ContainsKey(groupId))
            monsterActionCycleData[groupId].Add(newData);
        else
            monsterActionCycleData.Add(groupId,new List<MonsterActionCycleValue> { newData });
    }
}

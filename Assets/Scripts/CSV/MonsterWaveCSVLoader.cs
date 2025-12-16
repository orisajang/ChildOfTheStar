using System.Collections.Generic;
using UnityEngine;

public class MonsterWaveCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    //List<StageCSVData> stageCSVDataList = new List<StageCSVData>();
    Dictionary<string, MonsterWaveCSVData> monsterWaveCSVDataDic = new Dictionary<string, MonsterWaveCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    //public List<StageCSVData> LoadData(string fileName)
    public Dictionary<string, MonsterWaveCSVData> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv, 3);

        SetData(csv.Data);

        return monsterWaveCSVDataDic;
    }

    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {
            MonsterWaveCSVData monsterWaveData = new MonsterWaveCSVData();
            monsterWaveData.monsterWaveId = item[0];
            monsterWaveData.monsterNumber = int.Parse(item[1]);
            monsterWaveData.monsterId = item[2];

            //monsterData.prefabKey = item[4];
            monsterWaveCSVDataDic.Add(item[0], monsterWaveData);
        }
    }
}

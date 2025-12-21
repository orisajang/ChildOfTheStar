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
        int monsterCount = 5;
        foreach (var item in Data)
        {
            MonsterWaveCSVData monsterWaveData = new MonsterWaveCSVData();
            //monsterWaveData.monsterWaveId = item[0];
            //monsterWaveData.monsterNumber = int.Parse(item[1]);
            //monsterWaveData.monsterId1 = item[2];
            //monsterWaveData.monsterId2 = item[3];
            //monsterWaveData.monsterId3 = item[4];
            //monsterWaveData.monsterId4 = item[5];
            //monsterWaveData.monsterId5 = item[6];

            monsterWaveData.monsterWaveId = int.Parse(item[0]);
            //몬스터 정보가 1~5까지 있기때문에 몬스터를 한꺼번에 배열에 담아서 정보 담음
            monsterWaveData.monsterWaveInfo = new MonsterWaveInfo[monsterCount];
            for (int index =0; index < monsterCount; index++)
            {
                int rowIndex = index * 2;
                monsterWaveData.monsterWaveInfo[index].monsterNumber = item[rowIndex + 1] == "" ? 0 : int.Parse(item[rowIndex + 1]);
                monsterWaveData.monsterWaveInfo[index].monsterId = item[rowIndex + 2] == "" ? 0 : int.Parse(item[rowIndex + 2]);
            }

            //monsterData.prefabKey = item[4];
            monsterWaveCSVDataDic.Add(item[0], monsterWaveData);
        }
    }
}

using System.Collections.Generic;
using System;
using UnityEngine;

public class StageCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    //List<StageCSVData> stageCSVDataList = new List<StageCSVData>();
    Dictionary<string, StageCSVData> stageCSVDataDic = new Dictionary<string, StageCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    //public List<StageCSVData> LoadData(string fileName)
    public Dictionary<string,StageCSVData> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv, 3);

        SetData(csv.Data);

        return stageCSVDataDic;
    }

    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {
            StageCSVData stageData = new StageCSVData();

            stageData.stageId = item[0];
            stageData.monsterWaveId = item[1];
            stageData.isDimdis = item[2] == "FALSE" ? false : true;


            //monsterData.prefabKey = item[4];
            stageCSVDataDic.Add(item[0], stageData);
        }
    }
}

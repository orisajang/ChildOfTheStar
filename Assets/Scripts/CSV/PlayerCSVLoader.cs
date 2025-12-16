using System.Collections.Generic;
using UnityEngine;

public class PlayerCSVLoader
{
    CSV csv;
    CSVParser parser = new CSVParser();

    Dictionary<int, PlayerCSVData> playerCSVDataDic = new Dictionary<int, PlayerCSVData>();

    /// <summary>
    /// CSV 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일이름</param>
    /// <returns></returns>
    public Dictionary<int, PlayerCSVData> LoadData(string fileName)
    {
        //LoadCSV
        csv = new CSV(fileName);
        parser.Load(csv, 3);

        SetData(csv.Data);

        return playerCSVDataDic;
    }

    public void SetData(List<List<string>> Data)
    {
        foreach (var item in Data)
        {
            PlayerCSVData playerData = new PlayerCSVData();
            playerData.id = int.Parse(item[0]);
            playerData.name = item[1];
            //현재 startDeckId가 공백이라서 예외처리
            playerData.startDeckId = item[2] == "" ? 0 : int.Parse(item[2]);
            playerData.charHP = int.Parse(item[3]);
            playerData.movePoint = int.Parse(item[4]);
            playerData.sprite = item[5];
            playerData.animation = item[6];
            playerData.sound = item[7];


            //monsterData.prefabKey = item[4];
            playerCSVDataDic.Add(playerData.id, playerData);
        }
    }
}

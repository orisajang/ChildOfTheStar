using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private StageCSVLoader _stageCSVLoader = new StageCSVLoader();
    private MonsterWaveCSVLoader _monsterWaveCSVLoader = new MonsterWaveCSVLoader();
    //읽어온 CSV데이터를 한줄씩 리스트로 저장하고있음
    private Dictionary<string, StageCSVData> _stageCSVDataDic = new Dictionary<string, StageCSVData>();
    private Dictionary<string, MonsterWaveCSVData> _monsterWaveCSVDataDic = new Dictionary<string, MonsterWaveCSVData>();

    protected override void Awake()
    {
        base.Awake();

        //CSV파일 읽어서 monsterWave, Stage정보를 가지고있기
        SetMonsterWaveDataByCSV();
        SetStageDataByCSV();
    }

    private void SetStageDataByCSV()
    {
        //정보를 불러옴
        _stageCSVDataDic = _stageCSVLoader.LoadData("StageTable");

        //몬스터 웨이브데이터에서 해당 스테이지에 어떤 몬스터가 생성되어야하는지 정보를 저장해둔다.
        //키를 돌아가면서 키 안의 monsterWaveData를 채워줌
        List<string> stageKeys = new List<string>(_stageCSVDataDic.Keys);
        foreach(string key in stageKeys)
        {
            StageCSVData data = _stageCSVDataDic[key];

            //몬스터 웨이브 아이디를 하나의 배열로 가져와서 동시에 처리
            string[] monsterWaveIdArray = data.GetMonsterWaveIdArray();
            foreach(var waveId in monsterWaveIdArray)
            {
                //예외처리 (웨이브 아이디가 없고, 키가 존재하지않으면 break)
                if (waveId == null) break;
                if (!_monsterWaveCSVDataDic.ContainsKey(waveId)) break;

                //몬스터
                MonsterWaveCSVData waveData = _monsterWaveCSVDataDic[waveId];
                data.AddMonsterWaveList(waveData);
            }
        }
    }
    private void SetMonsterWaveDataByCSV()
    {
        //정보를 불러옴
        _monsterWaveCSVDataDic = _monsterWaveCSVLoader.LoadData("MonsterWave");
    }



}

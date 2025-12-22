using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    private StageCSVLoader _stageCSVLoader = new StageCSVLoader();
    private MonsterWaveCSVLoader _monsterWaveCSVLoader = new MonsterWaveCSVLoader();
    //읽어온 CSV데이터를 한줄씩 리스트로 저장하고있음
    private Dictionary<string, StageCSVData> _stageCSVDataDic = new Dictionary<string, StageCSVData>();
    private Dictionary<string, MonsterWaveCSVData> _monsterWaveCSVDataDic = new Dictionary<string, MonsterWaveCSVData>();

    //현재 스테이지와 웨이브 번호를 기억한다
    int currentStageId;
    int currentWaveIndex;

    protected override void Awake()
    {
        base.Awake();

        //CSV파일 읽어서 monsterWave, Stage정보를 가지고있기
        SetMonsterWaveDataByCSV();
        SetStageDataByCSV();
    }

    private void Start()
    {
        //게임매니저에서 스테이지 몇을 시작하라는 명령이 오면 해당 정보를 가지고 스테이지를 실행을 한다.
        //일단 무조건 스테이지 10000101 만 가지고 시작을 해보자
        StageCSVData currentStageData = _stageCSVDataDic["10000401"];
        List<MonsterWaveCSVData> waveData = currentStageData.monsterWaveList;
        currentStageId = currentStageData.stageId;
        currentWaveIndex = 0;
        //웨이브 1번 진행
        StartMonsterWave(waveData);
    }
    public void PlayNextStage()
    {
        //다음  스테이지가 존재하는지 확인
        StageCSVData currentStageData = _stageCSVDataDic[currentStageId.ToString()];
        
        if (currentWaveIndex >= currentStageData.monsterWaveList.Count)
        {
            //웨이브가 더 안남아있음., 다음 스테이지로 이동하도록 처리해야함
            
        }
        else
        {
            //스테이지가 남아있다. 계속 진행
            List<MonsterWaveCSVData> waveData = currentStageData.monsterWaveList;
            StartMonsterWave(waveData);
        }
    }
    private void StartMonsterWave(List<MonsterWaveCSVData> waveData)
    {
        //소환할 몬스터 정보는?
        MonsterWaveInfo[] monsterInfo = waveData[currentWaveIndex].monsterWaveInfo;
        //그대로 정보를 몬스터매니저에 넘겨준다
        MonsterManager.Instance.SetMonsterWaveInfo(monsterInfo);

        //다음 실행할 웨이브 번호를 위해서 ++
        currentWaveIndex++;
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
            //스테이지 하나의 값을 가져온다
            StageCSVData data = _stageCSVDataDic[key];

            //몬스터 웨이브 아이디를 하나의 배열로 가져와서 동시에 처리
            string[] monsterWaveIdArray = _stageCSVDataDic[key].GetMonsterWaveIdArray();
            foreach (var waveId in monsterWaveIdArray)
            {
                if (waveId == "") continue;
                if (!_monsterWaveCSVDataDic.ContainsKey(waveId)) continue;
                MonsterWaveCSVData waveData = _monsterWaveCSVDataDic[waveId];
                data.AddMonsterWaveList(waveData);
                //struct라서 리스트에서 직접 접근
                if (data.monsterWaveList == null) data.monsterWaveList = new List<MonsterWaveCSVData>();
                //struct라서 모든작업끝내면 그대로 넣어줘야한다
                _stageCSVDataDic[key] = data;


            }
            //string[] monsterWaveIdArray = data.GetMonsterWaveIdArray();
            //foreach(var waveId in monsterWaveIdArray)
            //{
            //    //예외처리 (웨이브 아이디가 없고, 키가 존재하지않으면 break)
            //    if (waveId == null) break;
            //    if (!_monsterWaveCSVDataDic.ContainsKey(waveId)) break;
            //
            //    //몬스터
            //    MonsterWaveCSVData waveData = _monsterWaveCSVDataDic[waveId];
            //    data.AddMonsterWaveList(waveData);
            //}
        }
    }
    private void SetMonsterWaveDataByCSV()
    {
        //정보를 불러옴
        _monsterWaveCSVDataDic = _monsterWaveCSVLoader.LoadData("MonsterWave");
    }



}

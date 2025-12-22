using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    //private StageCSVLoader _stageCSVLoader = new StageCSVLoader();
    //private MonsterWaveCSVLoader _monsterWaveCSVLoader = new MonsterWaveCSVLoader();
    ////읽어온 CSV데이터를 한줄씩 리스트로 저장하고있음
    //private Dictionary<string, StageCSVData> _stageCSVDataDic = new Dictionary<string, StageCSVData>();
    //private Dictionary<string, MonsterWaveCSVData> _monsterWaveCSVDataDic = new Dictionary<string, MonsterWaveCSVData>();

    //private Dictionary<string, StageCSVData> _stageInstanceDataDic = new Dictionary<string, StageCSVData>();
    StageCSVData currentStageData;
    //현재 스테이지인스턴스ID와 웨이브 번호를 기억한다
    int currentStageInstanceId;
    int currentWaveIndex;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        
    }
    public void SetStageInstanceData(StageCSVData stageInstancedata)
    {
        currentStageData = stageInstancedata;
    }
    public void StartStageTask()
    {
        //게임매니저에서 스테이지 몇을 시작하라는 명령이 오면 해당 정보를 가지고 스테이지를 실행을 한다.
        List<MonsterWaveCSVData> waveData = currentStageData.monsterWaveList;
        currentStageInstanceId = currentStageData.stageId;
        currentWaveIndex = 0;
        //웨이브 1번 진행
        StartMonsterWave(waveData);
    }
    public void PlayNextStage()
    {
        //다음 몬스터 웨이브가 남아있는지 확인, 
        //currentStageData = _stageInstanceDataDic[currentStageInstanceId.ToString()];
        
        if (currentWaveIndex >= currentStageData.monsterWaveList.Count)
        {
            //웨이브가 더 안남아있음., 다음 스테이지로 이동하도록 처리해야함
            //실제 게임에서는 스테이지 클리어 했다면 스테이지 선택창으로 다시 돌아감. (자동으로 다음 스테이지 실행하지 않는다)
            //테스트용 -> 스테이지1 끝나면 바로 스테이지 2 시작되도록
            DengeonManager.Instance.SetAndStartNextStage();
        }
        else
        {
            //몬스터 웨이브가 남아있다. 계속 진행
            List<MonsterWaveCSVData> waveData = currentStageData.monsterWaveList;
            StartMonsterWave(waveData);
        }
    }
    private void StartMonsterWave(List<MonsterWaveCSVData> waveData)
    {
        Debug.Log($"현재 스테이지번호:{currentStageData.stageId} 몬스터 웨이브: {currentWaveIndex+1}번째");
        //소환할 몬스터 정보는?
        MonsterWaveInfo[] monsterInfo = waveData[currentWaveIndex].monsterWaveInfo;
        //그대로 정보를 몬스터매니저에 넘겨준다
        MonsterManager.Instance.SetMonsterWaveInfo(monsterInfo);

        //다음 실행할 웨이브 번호를 위해서 ++
        currentWaveIndex++;
        //플레이어 턴 부터 시작
        TurnManager.Instance.StartPlayerTurn();
    }



}

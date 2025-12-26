using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    //private StageCSVLoader _stageCSVLoader = new StageCSVLoader();
    //private MonsterWaveCSVLoader _monsterWaveCSVLoader = new MonsterWaveCSVLoader();
    ////읽어온 CSV데이터를 한줄씩 리스트로 저장하고있음
    //private Dictionary<string, StageCSVData> _stageCSVDataDic = new Dictionary<string, StageCSVData>();
    //private Dictionary<string, MonsterWaveCSVData> _monsterWaveCSVDataDic = new Dictionary<string, MonsterWaveCSVData>();

    //private Dictionary<string, StageCSVData> _stageInstanceDataDic = new Dictionary<string, StageCSVData>();
    private StageCSVData _currentStageData;
    //현재 스테이지인스턴스ID와 웨이브 번호를 기억한다
    private int _currentStageInstanceId;
    private int _currentWaveIndex;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return; //이거도 추가
    }
    public void SetStageInstanceData(StageCSVData stageInstancedata)
    {
        _currentStageData = stageInstancedata;
    }
    public void StartStageTask()
    {
        //게임매니저에서 스테이지 몇을 시작하라는 명령이 오면 해당 정보를 가지고 스테이지를 실행을 한다.
        List<MonsterWaveCSVData> waveData = _currentStageData.monsterWaveList;
        _currentStageInstanceId = _currentStageData.stageId;
        _currentWaveIndex = 0;
        //웨이브 1번 진행
        //StartMonsterWave(waveData);
        PlayNextStage();
    }
    public void PlayNextStage()
    {
        //다음 몬스터 웨이브가 남아있는지 확인, 
        //currentStageData = _stageInstanceDataDic[currentStageInstanceId.ToString()];
        
        if (_currentWaveIndex >= _currentStageData.monsterWaveList.Count)
        {
            //웨이브가 더 안남아있음., 다음 스테이지로 이동하도록 처리해야함
            //실제 게임에서는 스테이지 클리어 했다면 스테이지 선택창으로 다시 돌아감. (자동으로 다음 스테이지 실행하지 않는다)
            //DungeonManager.Instance.ReturnToStageSelect();
            GameManager.Instance.GoToResourceEarnedScene();
        }
        else
        {
            //몬스터 웨이브가 남아있다. 계속 진행
            List<MonsterWaveCSVData> waveData = _currentStageData.monsterWaveList;
            StartMonsterWave(waveData);
        }
    }
    private void StartMonsterWave(List<MonsterWaveCSVData> waveData)
    {
        Debug.Log($"현재 스테이지번호:{_currentStageData.stageId} 몬스터 웨이브: {_currentWaveIndex+1}번째");

        //StageSelectScene 디버그용 코드 (삭졔예정)=> 몬스터 소환안하고 턴매니저 없이 스테이지 자동 진행
        //currentWaveIndex++; //디버그용 코드
        //PlayNextStage();    //디버그용 코드
        //return;             //디버그용 코드

        //소환할 몬스터 정보는?
        MonsterWaveInfo[] monsterInfo = waveData[_currentWaveIndex].monsterWaveInfo;
        //그대로 정보를 몬스터매니저에 넘겨준다
        MonsterManager.Instance.SetMonsterWaveInfo(monsterInfo);
        
        //다음 실행할 웨이브 번호를 위해서 ++
        _currentWaveIndex++;
        //플레이어 턴 부터 시작
        TurnManager.Instance.StartPlayerTurn();
    }



}

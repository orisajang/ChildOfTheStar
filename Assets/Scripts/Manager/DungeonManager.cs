using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : Singleton<DungeonManager>
{
    //각 던전이 어떤 스테이지를 가지고있는지와 현재 스테이지가 몇스테이지인지,  스테이지별로 랜덤확률로 스테이지 선택
    //현재 던전번호
    int currentDungeonNumber;
    //현재 스테이지 번호
    int currentStageNumber;
    //현재 스테이지에서 몇단계 진행중인지
    //int stageInstanceNumber;
    //현재 선택중인 스테이지
    StageCSVData currentSelectStage;

    Dictionary<string, List<StageCSVData>> _dungeonDataDic = new Dictionary<string, List<StageCSVData>>();
    Dictionary<int, List<StageCSVData>> _stageDataDic = new Dictionary<int, List<StageCSVData>>();

    //CSV읽어오기
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
        //딕셔너리에 스테이지별 어떤 스테이지 인스턴스를 가지고있는지 설정하기
        SetStageInstanceData();

        //사용한다고 가정  (아직 UI에서 입력하고 그런 작업이 없기때문에 했다고 가정하면?)
        //3. 사용
        //던전1의 스테이지 1번을 쓰겠다.
        //지금 던전에는 키값으로 10, 20, 30, 40 -> int형으로 변환 int.Parse 하고 /10
        //스테이지에는 키값으로 그룹넘버 100001
        //예시) 던전4를 진행하겠다
        //던전 넘버 선택은 
        //currentDengeonNumber = 1;
        //SetStageDataForStageManager();
        //초기시작시 1회 버튼을 활성화해주기위해
        //SetStageDataForStageManager();
        Debug.Log($"DungeonManager Awake: {GetInstanceID()}");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += StartStageScene;
    }

    private void Start()
    {
        //스테이지매니저에 정보 설정
        //StageManager.Instance.SetStageInstanceData(currentSelectStage);
        //스테이지 시작 명령
        //StageManager.Instance.StartStageTask();

    }
    /// <summary>
    /// 현재 던전번호 설정
    /// </summary>
    /// <param name="dungeonNumber"></param>
    public void SetDengeonNumber(int dungeonNumber)
    {
        //현재 던전번호
        currentDungeonNumber = dungeonNumber;
    }
    /// <summary>
    /// 씬이 시작될때마다 호출되는 메서드
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void StartStageScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != SceneName.Stage) return;
        //스테이지 씬이면 아래 작업 진행
        SetStageDataForStageManager();
    }

    //private void SetStageDataForStageManager(int dengeonSelect)
    public void SetStageDataForStageManager()
    {
        //예시) 던전 1을 선택, 스테이지는 무조건 1부터 시작
        //int dengeonSelect = 4;
        //currentDengeonNumber = dengeonSelect;
        currentStageNumber++;

        //던전 키값, 스테이지 키값 생성
        string dungeonSelectKey = (currentDungeonNumber * 10).ToString(); //10
        string stageSelectKeyString = dungeonSelectKey.ToString() + "000" + currentStageNumber.ToString(); //10 000 1
        int stageSelectKey = int.Parse(stageSelectKeyString); //1000001
        //2. 스테이지 그룹id로 다시 묶어준다
        //foreach(string dengeonNumber in _dengeonDataDic.Keys)
        {
            //foreach로 던전1,던전2... 의 각각의 데이터를 가져온다
            List<StageCSVData> dungeonList = _dungeonDataDic[dungeonSelectKey];
            //이거를 스테이지 그룹id로 묶자
            for (int index = 0; index < dungeonList.Count; index++)
            {
                StageCSVData stageInfo = dungeonList[index];
                int groupId = stageInfo.stageGroupId;

                if (!_stageDataDic.ContainsKey(groupId))
                {
                    List<StageCSVData> stageInfoList = new List<StageCSVData>();
                    stageInfoList.Add(stageInfo);
                    _stageDataDic[groupId] = stageInfoList;
                }
                else
                {
                    _stageDataDic[groupId].Add(stageInfo);
                }
            }
        }
        //현재 선택된 던전의 스테이지 리스트를 가져옴
        if(!_stageDataDic.ContainsKey(stageSelectKey))
        {
            //키가 존재하지않는다면 return; -> (보스까지 끝났을떄 체크 해야함)
            //return 하지말고 뭔가 스테이지가 보스까지 끝났다면 바로 던전메뉴로 나가게 해야할듯
            Debug.Log("여기 뜨면 안됨. 뭔가 확인해보기");
            return;
        }
        List<StageCSVData> currentStageInstanceList = _stageDataDic[stageSelectKey];


        //리스트중에서 랜덤으로 1개를 선택한다  (그런데 각자 가지고있는 확률을 고려해서 스테이지를 선택해야함)
        float randomValue = Random.Range(0f, 1.0f);
        float currentPercent = 0;
        int selectInstanceIndex = 0;
        //확률을 더해주면서 설정된 스테이지 랜덤 가중치에 따라 스테이지 인스턴스를 선택한다
        for (int index = 0; index < currentStageInstanceList.Count; index++)
        {
            //1회 랜덤 확률을 더해줌
            currentPercent += currentStageInstanceList[index].stageGroupChance;
            if (currentPercent >= randomValue)
            {
                //현재 선택된 스테이지를 저장
                selectInstanceIndex = index;
                currentSelectStage = currentStageInstanceList[index];
                Debug.Log($"랜덤으로 선택된 스테이지: {currentSelectStage.stageId}");
                break;
            }
        }

        //UI에도 정보 설정
        StageSelectUIManager.Instance.SetStageInfo(currentStageNumber-1, selectInstanceIndex);

    }

    /// <summary>
    /// 스테이지가 전부 끝나면 처리할 행동 메서드
    /// </summary>
    public void AllStageClear()
    {
        //SetStageDataForStageManager();
        //스테이지가 전부 끝났다면 던전 선택화면으로 돌아가면 됨.
        GameManager.Instance.GoToTitleScene();
    }
    public void ReturnToStageSelect()
    {
        //GameManager.Instance.GoToStageScene(); //추후 구현예정
        //현재 스테이지 번호가 총 스테이지 갯수보다 많다면
        if (currentStageNumber >= _stageDataDic.Count)
        {
            //스테이지를 전부 클리어했으니 해당 작업 진행
            Debug.Log("해당 던전의 모든 스테이지 클리어");
            AllStageClear();
        }
        else
        {
            //아니라면 계속 다음 스테이지 진행할 수 있게 다음 스테이지 랜덤으로 버튼 활성화
            SetStageDataForStageManager();
        }
    }

    public void SetAndStartNextStage()
    {
        //SetStageDataForStageManager();
        //스테이지매니저에 정보 설정
        StageManager.Instance.SetStageInstanceData(currentSelectStage);
        //스테이지 시작 명령
        StageManager.Instance.StartStageTask();
    }

    private void SetStageInstanceData()
    {
        //_stageInfoDic에 뭔가를 넣어줘야함
        foreach(string stageId in _stageCSVDataDic.Keys)
        {
            StageCSVData stageData = _stageCSVDataDic[stageId];
            //앞에서부터 2글자 잘라서 던전번호, 4글자 잘라서 스테이지번호, 2글자 잘라서 인스턴스 번호 얻어옴
            string dungeonNumber = stageData.stageId.ToString().Substring(0, 2);
            //string stageNumber = stageData.stageId.ToString().Substring(2, 4);
            //string stageInstnceNumber = stageData.stageId.ToString().Substring(6, 2);

            //던전 번호로 딕셔너리 나눔
            if (!_dungeonDataDic.ContainsKey(dungeonNumber))
            {
                List<StageCSVData> stageCSVDataBuf = new List<StageCSVData>();
                stageCSVDataBuf.Add(stageData);
                _dungeonDataDic[dungeonNumber] = stageCSVDataBuf;
            }
            else
            {
                _dungeonDataDic[dungeonNumber].Add(stageData);
            }
        }
        
        
    }

    private void SetStageDataByCSV()
    {
        //정보를 불러옴
        _stageCSVDataDic = _stageCSVLoader.LoadData("StageTable");

        //몬스터 웨이브데이터에서 해당 스테이지에 어떤 몬스터가 생성되어야하는지 정보를 저장해둔다.
        //키를 돌아가면서 키 안의 monsterWaveData를 채워줌
        List<string> stageKeys = new List<string>(_stageCSVDataDic.Keys);
        foreach (string key in stageKeys)
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
        }
    }
    private void SetMonsterWaveDataByCSV()
    {
        //정보를 불러옴
        _monsterWaveCSVDataDic = _monsterWaveCSVLoader.LoadData("MonsterWave");
    }
}

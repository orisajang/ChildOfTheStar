using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterManager : Singleton<MonsterManager>
{
    //몬스터 정보들을 CSV파일에서 읽어서 딕셔너리에 저장
    //CSV에서 읽어오는 Loader 클래스 생성
    private MonsterCSVLoader _monsterCSVLoader = new MonsterCSVLoader();
    private MonsterActionCycleCSVLoader _monsterActionCycleCSVLoader = new MonsterActionCycleCSVLoader();
    private MonsterActionCSVLoader _monsterActionCSVLoader = new MonsterActionCSVLoader();
    //딕셔너리로 id값을 입력하면 데이터를 불러오게 하기위해서 선언
    private Dictionary<int, MonsterCSVData> _monsterDataDic = new Dictionary<int, MonsterCSVData>();
    private Dictionary<int, List<MonsterActionCycleValue>> _monsterActionCycleDataDic;
    private Dictionary<int, MonsterActionCSVData> _monsterActionDataDic = new Dictionary<int, MonsterActionCSVData>();

    //테스트용 몬스터프리팹(삭제예정)
    [SerializeField] GameObject _monsterPrefab;
    //몬스터가 생성될 위치
    [SerializeField] Transform _monsterCreatePos;
    Transform[] _monsterCreatePosArray;
    //몬스터가 소환될 위치
    int _currentSpawnIndex = 0;
    //생성한 몬스터 저장
    List<Monster> _spawnedMonster = new List<Monster>();
    public List<Monster> SpawnedMonster
    {
        get
        {
            return _spawnedMonster;
        }
    }

    //플레이어가 공격을위해 선택한 몬스터가 무엇인지 알기위해서 (이거는 턴관리 매니저에 있어야할듯)
    InputAction _mouseClickAction;
    InputAction _pointAction;
    [SerializeField] LayerMask _monsterLayer;
    public Monster _targetMonster { get; private set; }

    public event Action OnTargetMonsterSelected;
    //몬스터 행동 처리할때 어떤 몬스터의 행동이 진행중인지 
    private int _currentActMonsterIndex;

    //MonsterManager 에 있는 테스트 생성용 코드를 여러 몬스터가 있을떄 어떻게 생성시키게 할건지 고민 필요
    protected override void Awake()
    {
        base.Awake();

        //몬스터가 생성될 좌표 설정
        SetMonsterSpawnPosition();

        //CSV파일을 통해 몬스터 정보 Set
        AddMonsterActionByCSV();
        AddMonsterActionCycleByCSV();
        //Monster에 MonsterActionCycle이 들어가있어야해서 AddMonsterActionCycleByCSV를 먼저 해야함
        AddMonsterByCSV();
        

        //몬스터 생성 테스트(테스트)
        MakeMonsterById(3101);
        MakeMonsterById(3102);
        MakeMonsterById(3103);
        //몬스터 액션 주기 테스트(테스트)
        //MonsterDataDic에 있는 몬스터 종류마다 Monster의 ActionCycle을 채워줘야함
        //아래를 for문돌려서 몬스터 종류마다 가지고 있도록 처리해야함
        MakeMosnterActionCycle(3101);
        MakeMonsterAction(310101);

        //몬스터 사망 테스트 (전투로인해 몬스터 HP가 0이되었다고 가정)
        //_spawnedMonster[0].MonsterDead();

        //몬스터 액션 시작 테스트 (턴매니저에서 몬스터 턴이라고 알릴때 동작)
        //StartMonsterAction();

        //입력
        _mouseClickAction = InputSystem.actions.FindAction("UI/Click");
        _pointAction = InputSystem.actions.FindAction("UI/Point");
    }

    private void OnEnable()
    {
        _mouseClickAction.started += OnMouseClick;
        _mouseClickAction.Disable();
        
    }
    private void OnDisable()
    {
        _mouseClickAction.started -= OnMouseClick;
    }
    public void EnableSelectMonsterTarget()
    {
        _mouseClickAction.Enable();
    }
    public void DisableSelectMonsterTarget()
    {
        _mouseClickAction.Disable();
    }
    private void OnMouseClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("클릭중");
        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 100f, _monsterLayer);
        if(hit.collider != null)
        {
            Monster mon = hit.collider.GetComponent<Monster>();
            _targetMonster = mon;
            //몬스터가 선택되었다는 것을 알림
            OnTargetMonsterSelected?.Invoke();
        }
        
        //_targetMonster.TakeDamage(100);
    }
    private void SetMonsterSpawnPosition()
    {
        _monsterCreatePosArray = new Transform[_monsterCreatePos.childCount];
        for (int i = 0; i < _monsterCreatePos.childCount; i++)
        {
            _monsterCreatePosArray[i] = _monsterCreatePos.GetChild(i);
        }
    }
    /// <summary>
    /// 몬스터에 대한 정보를 CSV에서 불러와서 SET
    /// </summary>
    private void AddMonsterByCSV()
    {
        //몬스터 정보를 불러옴
        _monsterDataDic = _monsterCSVLoader.LoadData("MonsterCSVData");



        //몬스터 안에 존재하는 몬스터 행동 주기마다 각각 액션을 넣어준다
        //미리 CSV데이터를 딕셔너리에 넣었던 것들을 이용
        int[] keys = _monsterDataDic.Keys.ToArray();
        foreach(int item in keys)
        {
            //현재 선택된 몬스터 데이터
            MonsterCSVData data = _monsterDataDic[item];
            //몬스터에서 몬스터마다 사이클 id를 꺼낸다
            int cycleId = data.monsterCycleId;
            //사이클 id로 monsterActionCycle에 존재하는  사이클id가 동일한 것들을 전부 찾는다(이미 만들어두었음)
            List<MonsterActionCycleValue> actionCycleList =_monsterActionCycleDataDic[cycleId];

            //이제 해당 리스트안에 어떤 몬스터 액션이 들어가는지 저장해준다
            int actionCount = actionCycleList.Count;
            for(int index=0; index < actionCount; index++)
            {
                //id를 찾고 액션 딕셔너리에서 정보를 빼온다
                int actionId = actionCycleList[index].actionId;
                MonsterActionCSVData actionData =  _monsterActionDataDic[actionId];
                //struct 값타입이라서 임시변수 만들고 값넣고 다시 원본에 넣어줘야함
                MonsterActionCycleValue actionCycleData = actionCycleList[index];
                actionCycleData.SetMonsterActionData(actionData);
                actionCycleList[index] = actionCycleData;
            }
            //data.SetMonsterActionCycle(actionCycleList);
            //struct 값타입이라서 임시변수 만들고 값넣고 다시 원본에 넣어줘야함
            MonsterCSVData monsterData = _monsterDataDic[item];
            monsterData.SetMonsterActionCycle(actionCycleList);
            _monsterDataDic[item] = monsterData;
        }


    }
    /// <summary>
    /// 몬스터 액션 주기를 CSV에서 불러와서 SET
    /// </summary>
    private void AddMonsterActionCycleByCSV()
    {
        //리스트로 전부 데이터들을 불러온다
        _monsterActionCycleDataDic = _monsterActionCycleCSVLoader.LoadData("MonsterActionCycleCSVData");

        //몬스터 액션


        ////딕셔너리를 같은 group ID로 묶을 수 있도록 리스트를 따로 만들어준다
        //List<MonsterActionCycleCSVData> dataList = new List<MonsterActionCycleCSVData>();
        ////private Dictionary<int, List<MonsterActionCycleCSVData>> _monsterActionCycleDataDic = new Dictionary<int, List<MonsterActionCycleCSVData>>();
        //foreach(var item in _monsterActionCycleCSVDataList)
        //{
        //    int currentGroupid = item.groupId;
        //    //딕셔너리에 그룹id가 포함되어있다면
        //    if (_monsterActionCycleDataDic.ContainsKey(currentGroupid))
        //    {
        //        _monsterActionCycleDataDic[currentGroupid].Add(item);
        //    }
        //    else
        //    {
        //        //딕셔너리에 그룹id가 없는상태 (처음 시작)
        //        List<MonsterActionCycleCSVData> list = new List<MonsterActionCycleCSVData>();
        //        list.Add(item);
        //        _monsterActionCycleDataDic[currentGroupid] = list;
        //    }
        //}
       
    }
    /// <summary>
    /// 몬스터 행동 정보를 CSV파일에서 가져옴
    /// </summary>
    private void AddMonsterActionByCSV()
    {
        //행동 정보를 불러옴
        _monsterActionDataDic = _monsterActionCSVLoader.LoadData("MonsterActionCSVData");
    }
    /// <summary>
    /// _monsterDic에 저장된 ID값을 입력하면 해당 정보로 몬스터를 생성해주도록
    /// </summary>
    private void MakeMonsterById(int id)
    {
        //몬스터 데이터를 꺼내서 몬스터 정보 지정
        MonsterCSVData data = _monsterDataDic[id];
        Monster mon = _monsterPrefab.GetComponent<Monster>();
        //몬스터 생성 (임시 테스트)
        if(_currentSpawnIndex < _monsterCreatePosArray.Length)
        {
            //몬스터 생성후 몬스터를 매니저에서 가지고있음
            Monster monsterBuf = Instantiate(mon, _monsterCreatePosArray[_currentSpawnIndex].position, _monsterCreatePosArray[_currentSpawnIndex].rotation);
            monsterBuf.SetMonsterInfo(data);
            _spawnedMonster.Add(monsterBuf);
            //몬스터 사망시 생존 몬스터 삭제
            monsterBuf.OnMonsterDead += MonsterRemove;
            monsterBuf.OnMonsterActEnd += MonsterActEnd;
            _currentSpawnIndex++;
        }

        //만약에 몬스터의 턴이라고 가정하자. 그렇다면 행동 1개를 사용해야한다.

    }
    private void MakeMosnterActionCycle(int groupId)
    {
        var data = _monsterActionCycleDataDic[groupId];
        //Debug.Log($"액션 사이클은 총 {data.Count}개 입니다.");
        //이거를 이제 Monster에 있는 곳에 넣어주면 됨.
        //3개가 있다는 것을 확인했다. 그렇다면 이제? monster_action에서 해당 키값을 찾아서 MonsterAction이라는 클래스에 가지고있게 하자
        //MonsterAction이라는 클래스를 만들자 몬스터의 행동을 가지고있는
    }
    private void MakeMonsterAction(int id)
    {
        //행동 id를 꺼내면 해당 행동을 가져옴
        //Debug.Log(_monsterActionDataDic[id].id);
    }


    //몬스터의 상태는 3개가 있다 (대기, 공격준비, Idle) 
    //대기와 공격준비는 비슷한듯? 
    /// <summary>
    /// 몬스터의 상태 Test -Idle 상태
    /// </summary>
    private void MonsterIdleState(string animation, string effect, string sound)
    {
        //해당 애니메이션, 이펙트, 소리 동작시킨다.
        //몬스터 스스로 턴1개를 감소시킴
        //만약 턴이 종료되었다면? 턴매니저에 자신의 턴종료를 알림
        
    }
    /// <summary>
    /// 몬스터가 사망하면 몬스터매니저의 몬스터생존여부도 삭제
    /// </summary>
    private void MonsterRemove(Monster monster)
    {
        _spawnedMonster.Remove(monster);
        _targetMonster = null;
    }

    private void MonsterActEnd()
    {
        _currentActMonsterIndex++;
        if(_currentActMonsterIndex >= _spawnedMonster.Count)
        {
            //모든 몬스터 행동 종료
            TurnManager.Instance.EndMonsterTurn();
            return;
        }

        //다음 몬스터 행동 시작
        _spawnedMonster[_currentActMonsterIndex].MonsterActStart();

        //1,2
    }

    /// <summary>
    /// 턴 매니저한테 몬스터의 턴이라고 알림을 받으면 해당 메서드 실행
    /// </summary>
    public void StartMonsterAction()
    {
        //1. 몬스터의 아이디순으로 오름차순 정렬 (ID가 낮은게 먼저 행동)
        _spawnedMonster.Sort((a,b) => a._monsterId.CompareTo(b._monsterId));
        //2. 실행될 몬스터 인덱스 초기화
        _currentActMonsterIndex = 0;

        //2.몬스터가 1마리씩 전부 행동력 쓸때까지 행동
        //for문을 전부 실행시키는게 아닌 0번째끝나면 1번쨰 실행시켜야함
        //0번 몬스터를 실행시키고, 해당 몬스터가 행동 종료 이벤트를 보내면 다음 몬스터가 알아서 실행된다.
        _spawnedMonster[0].MonsterActStart();
    }

    
}

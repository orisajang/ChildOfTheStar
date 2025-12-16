using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    //몬스터 정보들을 CSV파일에서 읽어서 딕셔너리에 저장
    //CSV에서 읽어오는 Loader 클래스 생성
    private MonsterCSVLoader _monsterCSVLoader = new MonsterCSVLoader();
    private MonsterActionCycleCSVLoader _monsterActionCycleCSVLoader = new MonsterActionCycleCSVLoader();
    private MonsterActionCSVLoader _monsterActionCSVLoader = new MonsterActionCSVLoader();
    //읽어온 CSV데이터를 한줄씩 리스트로 저장하고있음
    private List<MonsterCSVData> _monsterCSVDataList = new List<MonsterCSVData>();
    private List<MonsterActionCSVData> _monsterActionCSVDataList = new List<MonsterActionCSVData>();
    //딕셔너리로 id값을 입력하면 데이터를 불러오게 하기위해서 선언
    private Dictionary<int, MonsterCSVData> _monsterDataDic = new Dictionary<int, MonsterCSVData>();
    private Dictionary<int, List<MonsterActionCycleValue>> _monsterActionCycleDataDic;
    private Dictionary<int, MonsterActionCSVData> _monsterActionDataDic = new Dictionary<int, MonsterActionCSVData>();

    //테스트용 몬스터프리팹(삭제예정)
    [SerializeField] GameObject _monsterPrefab;

    //MonsterManager 에 있는 테스트 생성용 코드를 여러 몬스터가 있을떄 어떻게 생성시키게 할건지 고민 필요

    private void Awake()
    {
        //CSV파일을 통해 몬스터 정보 Set
        AddMonsterActionByCSV();
        AddMonsterActionCycleByCSV();
        //Monster에 MonsterActionCycle이 들어가있어야해서 AddMonsterActionCycleByCSV를 먼저 해야함
        AddMonsterByCSV();
        

        //몬스터 생성 테스트(테스트)
        MakeMonsterById(3101);
        //몬스터 액션 주기 테스트(테스트)
        //MonsterDataDic에 있는 몬스터 종류마다 Monster의 ActionCycle을 채워줘야함
        //아래를 for문돌려서 몬스터 종류마다 가지고 있도록 처리해야함
        MakeMosnterActionCycle(3101);
        MakeMonsterAction(310101);
    }
    /// <summary>
    /// 몬스터에 대한 정보를 CSV에서 불러와서 SET
    /// </summary>
    private void AddMonsterByCSV()
    {
        //몬스터 정보를 불러옴
        _monsterCSVDataList = _monsterCSVLoader.LoadData("MonsterCSVData");
        //불러온 정보들로 몬스터 만들어주고 딕셔너리에 넣어줌
        foreach (var item in _monsterCSVDataList)
        {
            eMonsterType type = item.monsterType;
            eMonsterSize size = item.monsterSize;
           
            //생성된 몬스터 정보를 넣어줌
            _monsterDataDic.Add(item.monsterId, item);
        }
    }
    /// <summary>
    /// 몬스터 액션 주기를 CSV에서 불러와서 SET
    /// </summary>
    private void AddMonsterActionCycleByCSV()
    {
        //리스트로 전부 데이터들을 불러온다
        _monsterActionCycleDataDic = _monsterActionCycleCSVLoader.LoadData("MonsterActionCycleCSVData");
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
        _monsterActionCSVDataList = _monsterActionCSVLoader.LoadData("MonsterActionCSVData");
        //불러온 정보들로 몬스터 만들어주고 딕셔너리에 넣어줌
        foreach (var item in _monsterActionCSVDataList)
        {
            eMonsterAction action = item.actionType;

            //생성된 몬스터 정보를 넣어줌
            _monsterActionDataDic.Add(item.id, item);
        }
    }
    /// <summary>
    /// _monsterDic에 저장된 ID값을 입력하면 해당 정보로 몬스터를 생성해주도록
    /// </summary>
    private void MakeMonsterById(int id)
    {
        //몬스터 데이터를 꺼내서 몬스터 정보 지정
        MonsterCSVData data = _monsterDataDic[id];
        Monster mon = _monsterPrefab.GetComponent<Monster>();
        mon.SetMonsterInfo(data);
        //몬스터 생성 (임시 테스트)
        Instantiate(mon, transform.position, transform.rotation);
    }
    private void MakeMosnterActionCycle(int groupId)
    {
        var data = _monsterActionCycleDataDic[groupId];
        Debug.Log($"액션 사이클은 총 {data.Count}개 입니다.");
        //이거를 이제 Monster에 있는 곳에 넣어주면 됨.
        //3개가 있다는 것을 확인했다. 그렇다면 이제? monster_action에서 해당 키값을 찾아서 MonsterAction이라는 클래스에 가지고있게 하자
        //MonsterAction이라는 클래스를 만들자 몬스터의 행동을 가지고있는
    }
    private void MakeMonsterAction(int id)
    {
        //행동 id를 꺼내면 해당 행동을 가져옴
        Debug.Log(_monsterActionDataDic[id].id);
    }

}

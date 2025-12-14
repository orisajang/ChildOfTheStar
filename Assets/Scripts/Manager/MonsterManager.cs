using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    //몬스터 정보들을 CSV파일에서 읽어서 딕셔너리에 저장
    private MonsterCSVLoader _monsterCSVLoader = new MonsterCSVLoader();
    private List<MonsterCSVData> _monsterCSVDataList = new List<MonsterCSVData>();
    private Dictionary<int, MonsterCSVData> _monsterDataDic = new Dictionary<int, MonsterCSVData>();

    //테스트용 몬스터프리팹(삭제예정)
    [SerializeField] GameObject _monsterPrefab;

    //MonsterManager 에 있는 테스트 생성용 코드를 여러 몬스터가 있을떄 어떻게 생성시키게 할건지 고민 필요

    private void Awake()
    {
        //CSV파일을 통해 몬스터 정보 Set
        AddMonsterByCSV();

        //몬스터 생성 테스트(테스트)
        MakeMonsterById(3101);
    }
    
    private void AddMonsterByCSV()
    {
        //몬스터 정보를 불러옴
        _monsterCSVDataList = _monsterCSVLoader.LoadWeaponData("MonsterCSVData");
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

}

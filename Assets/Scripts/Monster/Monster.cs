using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.U2D;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

public enum eMonsterType
{
    Normal = 1 ,Boss
}
public enum eMonsterSize
{
    Small = 1, Medium, Large
}
public enum eMonsterAction
{
    Idle=1, AttackReady,Attack
}
public enum eMonsterAttackType
{
    playerAttack = 1, selfHeal 
}

public class Monster : MonoBehaviour
{
    //몬스터의 필드 (기획서 테이블에 나와있는 순서대로)
    public int _monsterId { get; private set; }
    string _monsterName;
    eMonsterType _monsterType;
    eMonsterSize _monsterSize;
    public int _monsterHp { get; private set; }
    public int _monsterAttackPower { get; private set; }
    int _monsterMaxEnergy;
    int _monsterCycleId;
    //몬스터가 어떤 이미지,애니메이션,음성을 가지고있는지
    public string _monsterAnimation { get; private set; }
    string _monsterSound;


    //현재 HP
    private int _monsterCurrentHp;
    //현재 행동력
    int _monsterCurrentEnergy;
    //행동 실행 횟수
    int _actPlayCount;

    //어떤 행동을 하는지
    public List<MonsterActionCycleValue> monsterActionCycleList;
    Coroutine _actionCoroutine;
    WaitForSeconds _delay;

    //몬스터 사망 처리를 위해 invoke 처리
    public event Action<Monster> OnMonsterDead;
    //몬스터 액션 행동이 전부 끝났을때 몬스터매니저에 알리기위해서
    public event Action OnMonsterActEnd;

    //몬스터의 행동(전략패턴)
    //몬스터는 3가지 행동을 가지고있을수있으며 (Idle, ready,attack)
    //턴이 남아있으면 몬스터의 행동 양식에 따라 계속 반복하면서 공격을 한다.
    private MonsterStrategy _mosnterStrategy;
    private Dictionary<eMonsterAction, MonsterStrategy> monsterStateDic = new Dictionary<eMonsterAction, MonsterStrategy>();
    private MonsterAttackBehaviorStrategy _monsterAttackStrategy;
    private Dictionary<eMonsterAttackType, MonsterAttackBehaviorStrategy> _monsterAttackTypeDic = new Dictionary<eMonsterAttackType, MonsterAttackBehaviorStrategy>();
    public Dictionary<eMonsterAttackType, MonsterAttackBehaviorStrategy> MonsterAttacktypeDic { get { return _monsterAttackTypeDic; } }

    private void Awake()
    {
        _delay = new WaitForSeconds(1);
        MakeDictionaryForMonsterState();
        MakeDictionaryForAttackType();
    }

    /// <summary>
    /// CSV로 부터 몬스터 정의에 대한 값 받기
    /// </summary>
    /// <param name="data">CSV 파싱 데이터</param>
    public void SetMonsterInfo(MonsterCSVData data)
    {
        _monsterId = data.monsterId;
        _monsterName = data.monsterName;
        _monsterType = data.monsterType;
        _monsterSize = data.monsterSize;
        _monsterHp = data.monsterHp;
        _monsterAttackPower = data.monsterAttackPower;
        _monsterMaxEnergy = data.monsterMaxEnergy;
        _monsterCycleId = data.monsterCycleId;
        _monsterAnimation = data.monsterAnimation;
        _monsterSound = data.monsterSound;
        monsterActionCycleList = data.monsterActionCycleList;

        //몬스터 이미지 설정
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Sprite monsterImage = Resources.Load<Sprite>($"MonsterImages/{_monsterAnimation}");
        sprite.sprite = monsterImage;


        //현재HP를 초기에 설정
        _monsterCurrentHp = _monsterHp;
        _actPlayCount = 0;
    }
    /// <summary>
    /// 몬스터 사망처리 (몬스터 매니저에서 받음)
    /// </summary>
    public void MonsterDead()
    {
        //invoke를 해준다
        OnMonsterDead?.Invoke(this);
        //자신 삭제 (이제 오브젝트 풀로 관리하기때문에 삭제안함)
        //Destroy(gameObject);
        
    }

    /// <summary>
    /// 현재 행동 시작
    /// </summary>
    public void MonsterActStart()
    {
        Debug.Log($"{_monsterName}행동 시작");
        //몬스터 초기값
        MonsterInit();

        //코루틴으로 1초마다 행동을 실행
        _actionCoroutine = StartCoroutine(nameof(MonsterActDo));
    }
    private void MonsterInit()
    {
        //몬스터 행동력 초기화
        _monsterCurrentEnergy = _monsterMaxEnergy;
    }
    //전략패턴 -> 행동들만 넣자
    //그리고 각자 클래스에 어떤 공격력 필요하다면 따로따로 넣기
    //1. 부모 클래스 생성 (행동을 묶을거)
    //2. 자식 클래스 생성 (각자 어떤 행동을 할건지 정의)
    //3. 자식 클래스를 딕셔너리로 묶어도 좋다, Dictionary<enum,parent>

   
    /// <summary>
    /// 몬스터 행동타입별 다른 행동(전략패턴)
    /// </summary>
    public void MakeDictionaryForMonsterState()
    {
        foreach(eMonsterAction type in Enum.GetValues(typeof(eMonsterAction)))
        {
            switch(type)
            {
                case eMonsterAction.Idle:
                    monsterStateDic[type] = new MonsterIdleStrategy();
                    break;
                case eMonsterAction.AttackReady:
                    monsterStateDic[type] = new MonsterAttackReadyStrategy();
                    break;
                case eMonsterAction.Attack:
                    monsterStateDic[type] = new MonsterAttackStrategy();
                    break;
            }
        }
    }
    
    /// <summary>
    /// 공격타입별 다른 행동을 할때 딕셔너리 사용하기위해(전략패턴)
    /// </summary>
    public void MakeDictionaryForAttackType()
    {
        foreach (eMonsterAttackType type in Enum.GetValues(typeof(eMonsterAttackType)))
        {
            switch (type)
            {
                case eMonsterAttackType.playerAttack:
                    _monsterAttackTypeDic[type] = new MonsterAttackPlayer();
                    break;
                case eMonsterAttackType.selfHeal:
                    _monsterAttackTypeDic[type] = new MonsterHealSelfAction();
                    break;
            }
        }
    }
    /// <summary>
    /// 몬스터 행동
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonsterActDo()
    {
        //시작하기전 1초대기
        yield return _delay;
        //행동력이 남아있을때까지 진행
        while (_monsterCurrentEnergy > 0)
        {
            //현재 어떤 행동을 해야하는지, 계속 반복하기 위해
            int currentAction = _actPlayCount % monsterActionCycleList.Count;

            //선택한 몬스터 행동을 코루틴으로 1초마다 실행
            MonsterActionCycleValue action = monsterActionCycleList[currentAction];

            //어떤 행동인지 찾아서 그 행동을 수행한다
            eMonsterAction actionType = action.monsterActionData.actionType;
            monsterStateDic[actionType].MonsterActDo(this, action);
            //횟수는 계속 늘리고 나머지 연산을 해준다
            _actPlayCount++;
            //현재 몬스터 턴 횟수.
            _monsterCurrentEnergy--;
            //1초마다 실행
            //Debug.Log("몬스터 행동 실행 ");
            yield return _delay;
        }

        OnMonsterActEnd?.Invoke();
    }
   


    /// <summary>
    /// 받는 데미지 처리
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg)
    {
        _monsterCurrentHp -= dmg;
        Debug.Log($"몬스터 현재 체력은{_monsterCurrentHp}");
        if (_monsterCurrentHp <= 0)
        {
            MonsterDead();
        }
    }
    public void MonsterHealSelf(int healAmount)
    {
        //힐량 처리
        _monsterCurrentHp += healAmount;
        //최대체력 넘어갔으면 최대체력으로 설정
        if (_monsterCurrentHp > _monsterHp) _monsterCurrentHp = _monsterHp;
        Debug.Log($"몬스터 회복 현재 체력은{_monsterCurrentHp}");
    }
}


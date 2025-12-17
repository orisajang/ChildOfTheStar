using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.U2D;
using System;
using System.Collections.Generic;
using System.Collections;

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
    int _monsterHp;
    public int _monsterAttackPower { get; private set; }
    int _monsterMaxEnergy;
    int _monsterCycleId;
    //몬스터가 어떤 이미지,애니메이션,음성을 가지고있는지
    string _monsterAnimation;
    string _monsterSound;

    //행동정보. 상태패턴으로 해야할듯?
    int _idle;
    int _attackReady;
    int _attack;
    int _attackSpecial;

    //현재 HP
    int _monsterCurrentHp;
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

    //제약조건
    //1. 스테이지당 최대 소환 객체수 (몬스터매니저)
    //2. 몬스터 턴에서만 행동가능(턴매니저)
    //3. HP 0되면 사망


    //몬스터의 행동 (상태패턴)
    //몬스터는 4가지 행동을 가지고있으며 (Idle, ready,attack,special)
    //턴이 남아있으면 몬스터의 행동 양식에 따라 계속 반복하면서 공격을 한다.
    //몬스터의 행동양식은 반복되어야하므로 List로 저장하고, 그것을 %나머지연산을 통해 계속 어떤 행동해야하는지 찾는다
    //CSV로 읽어야하는 종류가 3종류라 잠시 스탑.. (몬스터정보, 몬스터 행동, 몬스터 행동 정보가.. csv로 되어있음.)
    private void Awake()
    {
        _delay = new WaitForSeconds(1);
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
    }
    /// <summary>
    /// 몬스터 사망처리 (몬스터 매니저에서 받음)
    /// </summary>
    public void MonsterDead()
    {
        //invoke를 해준다
        OnMonsterDead?.Invoke(this);
        //자신 삭제
        Destroy(gameObject);
        
    }

    /// <summary>
    /// 현재 행동 시작
    /// </summary>
    public void MonsterActStart()
    {
        //몬스터 초기값
        MonsterInit();

        //코루틴으로 1초마다 행동을 실행
        _actionCoroutine = StartCoroutine(nameof(MonsterActDo));
    }
    private void MonsterInit()
    {
        //몬스터 현재 HP,행동력 초기화
        _monsterCurrentHp = _monsterHp;
        _monsterCurrentEnergy = _monsterMaxEnergy;
        _actPlayCount = 0;
    }

    /// <summary>
    /// 몬스터 행동
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonsterActDo()
    {
        while (_monsterCurrentEnergy > 0)
        {
            //현재 어떤 행동을 해야하는지, 계속 반복하기 위해
            int currentAction = _actPlayCount % monsterActionCycleList.Count;

            //선택한 몬스터 행동을 코루틴으로 1초마다 실행
            MonsterActionCycleValue action = monsterActionCycleList[currentAction];

            //어떤 행동인지 찾아서 그 행동을 수행한다
            eMonsterAction actionType = action.monsterActionData.actionType;
            switch (actionType)
            {
                case eMonsterAction.Idle:
                    MonsterIdleState(action);
                    break;
                case eMonsterAction.AttackReady:
                    MonsterAttackReadyState(action);
                    break;
                case eMonsterAction.Attack:
                    MonsterAttackState(action);
                    break;
            }

            //횟수는 계속 늘리고 나머지 연산을 해준다
            _actPlayCount++;
            //현재 몬스터 턴 횟수.
            _monsterCurrentEnergy--;
            //1초마다 실행
            Debug.Log("몬스터 행동 실행 ");
            yield return _delay;
        }
    }
    /// <summary>
    /// Idle상태
    /// </summary>
    /// <param name="animation"></param>
    /// <param name="effect"></param>
    /// <param name="sound"></param>
    private void MonsterIdleState(MonsterActionCycleValue action)
    {
        //현재로서는 아무것도안함. 애니메이션,이펙트, 사운드만 변경
        //action.monsterActionData.animation, action.monsterActionData.effect, action.monsterActionData.sound
        Debug.Log("Idle 상태 실행");
    }
    /// <summary>
    /// 공격준비 상태
    /// </summary>
    /// <param name="animation"></param>
    /// <param name="effect"></param>
    /// <param name="sound"></param>
    private void MonsterAttackReadyState(MonsterActionCycleValue action)
    {
        //현재로서는 아무것도안함. 애니메이션,이펙트, 사운드만 변경
        //action.monsterActionData.animation, action.monsterActionData.effect, action.monsterActionData.sound
        Debug.Log("AttackReady 상태 실행");
    }
    /// <summary>
    /// 몬스터가 공격을 한다
    /// </summary>
    /// <param name="action"></param>
    private void MonsterAttackState(MonsterActionCycleValue action)
    {
        //실제 공격 처리
        //플레이어에 접근해서 공격 처리 (+배틀매니저 통해서)
        //- 행동이 ‘플레이어를 공격’하는 경우, (몬스터 공격력)*(공격 수치)의 피해를 입힘
        //-행동이 ‘자신의 hp를 회복’하는 경우, (몬스터 maxhp)*(공격 수치)의 체력을 회복함
        //몬스터의 공격력과 공격 수치를 보내기

        switch(action.monsterActionData.attackType)
        {
            case eMonsterAttackType.playerAttack:
                //배틀매니저에서 계산한 값을 기준으로
                int damage = BattleManager.Instance.CalcMonsterDamage(_attack, action.monsterActionData.attackValue);
                PlayerManager.Instance._player.TakeDamage(damage);
                break;
            case eMonsterAttackType.selfHeal:
                int healAmount = BattleManager.Instance.CalcMonsterHeal(_monsterHp, action.monsterActionData.attackValue);
                //힐량 처리
                _monsterCurrentHp += healAmount;
                //최대체력 넘어갔으면 최대체력으로 설정
                if (_monsterCurrentHp > _monsterHp) _monsterCurrentHp = _monsterHp;
                break;
        }
        
    }
}


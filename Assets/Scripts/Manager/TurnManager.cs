using System;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public enum Turn
    {
        Nune,
        PlayerTurn,
        MonsterTurn,
        StageTurn
    }
    public Turn CurrentTurn { get; private set; }

    [SerializeField] private BoardBlock boardBlock;

    //보드에 플레이어 턴이 시작되었다는 것을 알림 (추가이유: 플레이어 턴이 시작될때 과충전상태면 과충전 해제해야함)
    public event Action OnPlayerTurnStart;
    private void OnEnable()
    {
        //초기 몬스터 타겟을 지정해서 공격할 몬스터를 선택하기 위해
        MonsterManager.Instance.OnTargetMonsterSelected += StartPlayerBoardEnable;
        //플레이어 턴이 종료되었으면 처리하기 위해
        PlayerManager.Instance.SendPlayerMovePoint += CheckPlayerTurnEnd;
    }
    private void OnDisable()
    {
        MonsterManager.Instance.OnTargetMonsterSelected -= StartPlayerBoardEnable;
        PlayerManager.Instance.SendPlayerMovePoint -= CheckPlayerTurnEnd;
    }
    
    /// <summary>
    /// 플레이어 턴 시작
    /// 플레이어 hp가 0보다 크고 몬스터 턴이 아닐 경우에만 실행
    /// </summary>
    public void StartPlayerTurn()
    {
        //지금은 플레이어 매니저 Start에서 맨처음 턴을 시작하는데 스테이지나 게임매니저에서 턴 시작하도록 바꿔야함
        //플레이어턴 시작 이벤트 시작
        OnPlayerTurnStart?.Invoke();

        Debug.Log("플레이어 턴 시작");
        if (PlayerManager.Instance._player.CharacterHpCurrent <= 0)
        {
            Debug.Log("플레이어 사망 상태입니다.");
            CurrentTurn = Turn.Nune;
            return;
        }
        else if (PlayerManager.Instance._player.CharacterHpCurrent > 0)
        {
            CurrentTurn = Turn.PlayerTurn;

            // player에서 가져오면 그 때 수정
            //플레이어 행동력을 최대 행동력값으로
            PlayerManager.Instance._player.PlayerTurnInit();

            StartPlayerBoardDisable();
        }
    }
    /// <summary>
    /// 플레이어 턴이 시작될때 1회, 타일이동마다 계속 몬스터 타겟을 지정해줘야해서 추가
    /// </summary>
    public void StartPlayerBoardDisable()
    {
        //타일 클릭 못하게 보드 비활성화
        boardBlock.SetBoardActive(true);

        //타겟 몬스터 지정할때까지 기다려야함. 게임뷰에서 몬스터를 클릭하면 그 이후부터 타일 활성화 (StartPlayerBoardEnable 메서드 실행)
        Debug.Log("타겟 몬스터를 클릭해주세요");
        MonsterManager.Instance.EnableSelectMonsterTarget();
    }
    /// <summary>
    /// 타겟 몬스터를 선택하면 보드를 활성화 하기 위해서 사용
    /// </summary>
    public void StartPlayerBoardEnable()
    {
        
        //마우스클릭기능 비활성화 하고
        MonsterManager.Instance.DisableSelectMonsterTarget();
        Debug.Log("타겟 몬스터가 지정되었습니다");
        //이후 타일 이동 시작
        boardBlock.SetBoardActive(false);
        Debug.Log("플레이어 턴 시작");
    }
    /// <summary>
    /// 타일 이동이 1회끝났을때 호출되는 메서드
    /// </summary>
    public void OnPlayerTurnEndOnce()
    {
        //플레이어 턴 1회 감소
        PlayerManager.Instance.OnPlayerMovePointDecrease();
    }
    /// <summary>
    /// 플레이어의 현재 이동력을 매개변수로 받아서 플레이어의 턴이 끝났는지 체크
    /// </summary>
    /// <param name="movePoint">플레이어 현재 행동력</param>
    private void CheckPlayerTurnEnd(int movePoint)
    {
        Debug.Log($"플레이어 행동력:{movePoint}");
        //몬스터가 남아있는지 확인
        int monsterCount = MonsterManager.Instance.RemainMonster;
        if(monsterCount <= 0)
        {
            //스테이지 종료 처리 필요
        }

        //아직 턴이 남아서 타일이동을 더 해야한다면
        if (movePoint > 0)
        {
            //계속진행
            StartPlayerBoardDisable();
        }
        else
        {
            //턴이 종료되었다면
            EndPlayerTurn();
        }
    }

    /// <summary>
    /// 플레이어 턴 끝
    /// 보드 비활성화 몬스터 턴 시작 이동력 남아있으면 로그 출력
    /// </summary>
    private void EndPlayerTurn()
    {
        //몬스터 타겟 지정 못하게 비활성화
        MonsterManager.Instance.DisableSelectMonsterTarget();
        //타일 이동 비활성화
        boardBlock.SetBoardActive(true);
        //몬스터 턴 시작
        StartMonsterTurn();

        //if (PlayerManager.Instance._player.MovementPointCurrent <= 0)
        //{
        //    boardBlock.SetBoardActive(true);
        //
        //    Debug.Log("플레이어 턴 종료");
        //
        //    StartMonsterTurn();
        //}
        //else if (PlayerManager.Instance._player.MovementPointCurrent > 0)
        //{
        //    Debug.Log("이동력이 남아있습니다.");
        //}
    }

    /// <summary>
    /// 몬스터 턴 시작
    /// </summary>
    private void StartMonsterTurn()
    {
        //몬스터 갯수가 0개 이상이라면 몬스터 존재 
        //if (_monsterHp <= 0)
        if (MonsterManager.Instance.SpawnedMonster.Count == 0)
        {
            Debug.Log("모든 몬스터 사망 상태입니다.");
            CurrentTurn = Turn.Nune;
            return;
        }
        //else if (_monsterHp > 0)
        else if (MonsterManager.Instance.SpawnedMonster.Count > 0)
        {
            // 몬스터 턴으로 변경
            CurrentTurn = Turn.MonsterTurn;

            // 몬스터 이동력 채우기
            //_monsterPointCurrent = _monsterMaxEnergy;

            //보유한 몬스터를 전부 전투 처리
            MonsterManager.Instance.StartMonsterAction();

            Debug.Log("몬스터 턴 시작");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void EndMonsterTurn()
    {
        //if(_monsterPointCurrent <= 0)
        //{
        //    Debug.Log("몬스터 턴 종료");
        //    StartPlayerTurn();
        //}
        //
        //else if(_monsterPointCurrent > 0)
        //{
        //    Debug.Log("몬스터의 이동력이 남았습니다.");
        //}

        //몬스터매니저에서 이미 모든 행동력을 체크했음 (턴관리매니저에서 하는거로 바꿔야해야할지? 고민해야할듯)
        StartPlayerTurn();
    }
}

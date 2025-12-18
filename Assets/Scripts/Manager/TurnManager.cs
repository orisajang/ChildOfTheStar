using NUnit.Framework.Interfaces;
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

    private Player player;

    //[SerializeField] private TileBoardView tileBoardView;

    // 나중에 Player에서 가져오게 바꾸기
    int _characterHpCurrent;
    int _MovementPointMax;
    int _MovementPointCurrent;

    // 나중에 monstermanager에서 가져오기
    int _monsterHp;
    int _monsterMaxEnergy;
    int _monsterPointCurrent;


    private void OnEnable()
    {
        MonsterManager.Instance.OnTargetMonsterSelected += StartPlayerBoardActive;
    }
    private void OnDisable()
    {
        MonsterManager.Instance.OnTargetMonsterSelected -= StartPlayerBoardActive;
    }

    /// <summary>
    /// 플레이어 턴 시작
    /// 플레이어 hp가 0보다 크고 몬스터 턴이 아닐 경우에만 실행
    /// </summary>
    public void StartPlayerTurn()
    {
        
        if(PlayerManager.Instance._player._characterHpCurrent <= 0)
        {
            Debug.Log("플레이어 사망 상태입니다.");
            CurrentTurn = Turn.Nune;
            return;
        }
        else if (PlayerManager.Instance._player._characterHpCurrent > 0)
        {
            CurrentTurn = Turn.PlayerTurn;

            // player에서 가져오면 그 때 수정
            //플레이어 행동력을 최대 행동력값으로
            PlayerManager.Instance._player.SetMovementPointInit();
            _MovementPointCurrent = PlayerManager.Instance._player._MovementPointCurrent;


            //타겟 몬스터 지정 (추가) -플레이어가 타겟 몬스터를 지정할때까지 기다려야함
            //게임뷰에서 몬스터를 클릭하면 그 이후부터 타일 활성화 (StartPlayerBoardActive 메서드 실행)
            MonsterManager.Instance.EnableSelectMonsterTarget();
            //tileBoardView.SetBoardActive(true);
            //Debug.Log("플레이어 턴 시작");
        }
    }
    public void StartPlayerBoardActive()
    {
        //마우스클릭기능 비활성화 하고
        MonsterManager.Instance.DisableSelectMonsterTarget();
        //이후 타일 이동 시작
        //tileBoardView.SetBoardActive(true);
        Debug.Log("플레이어 턴 시작");
    }

    /// <summary>
    /// 플레이어 턴 끝
    /// 보드 비활성화 몬스터 턴 시작 이동력 남아있으면 로그 출력
    /// </summary>
    private void EndPlayerTurn()
    {
        if (PlayerManager.Instance._player._MovementPointCurrent <= 0)
        {
            //tileBoardView.SetBoardActive(false);

            Debug.Log("플레이어 턴 종료");

            StartMonsterTurn();
        }
        else if(PlayerManager.Instance._player._MovementPointCurrent > 0)
        {
            Debug.Log("이동력이 남아있습니다.");
        }
    }

    /// <summary>
    /// 몬스터 턴 시작
    /// </summary>
    private void StartMonsterTurn()
    {
        //몬스터 갯수가 0개 이상이라면 몬스터 존재 
        //if (_monsterHp <= 0)
        if(MonsterManager.Instance.SpawnedMonster.Count == 0)
        {
            Debug.Log("모든 몬스터 사망 상태입니다.");
            CurrentTurn = Turn.Nune;
            return;
        }
        //else if (_monsterHp > 0)
        else if(MonsterManager.Instance.SpawnedMonster.Count > 0)
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

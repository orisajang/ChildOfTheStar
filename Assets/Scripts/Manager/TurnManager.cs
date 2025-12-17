using NUnit.Framework.Interfaces;
using UnityEngine;

public class TurnManager : MonoBehaviour
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

    [SerializeField] private TileBoardView tileBoardView;

    // 나중에 Player에서 가져오게 바꾸기
    int _characterHpCurrent;
    int _MovementPointMax;
    int _MovementPointCurrent;

    // 나중에 monstermanager에서 가져오기
    int _monsterHp;
    int _monsterMaxEnergy;
    int _monsterPointCurrent;

    /// <summary>
    /// 플레이어 턴 시작
    /// 플레이어 hp가 0보다 크고 몬스터 턴이 아닐 경우에만 실행
    /// </summary>
    private void StartPlayerTurn()
    {
        if(_characterHpCurrent <= 0)
        {
            Debug.Log("플레이어 사망 상태입니다.");
            CurrentTurn = Turn.Nune;
            return;
        }
        else if (_characterHpCurrent > 0)
        {
            CurrentTurn = Turn.PlayerTurn;

            // player에서 가져오면 그 때 수정
            _MovementPointCurrent = _MovementPointMax;

            tileBoardView.SetBoardActive(true);

            Debug.Log("플레이어 턴 시작");
        }
    }

    /// <summary>
    /// 플레이어 턴 끝
    /// 보드 비활성화 몬스터 턴 시작 이동력 남아있으면 로그 출력
    /// </summary>
    private void EndPlayerTurn()
    {
        if (_MovementPointCurrent <= 0)
        {
            tileBoardView.SetBoardActive(false);

            Debug.Log("플레이어 턴 종료");

            StartMonsterTurn();
        }
        else if(_MovementPointCurrent > 0)
        {
            Debug.Log("이동력이 남아있습니다.");
        }
    }

    /// <summary>
    /// 몬스터 턴 시작
    /// </summary>
    private void StartMonsterTurn()
    {
        if (_monsterHp <= 0)
        {
            Debug.Log("모든 몬스터 사망 상태입니다.");
            CurrentTurn = Turn.Nune;
            return;
        }
        else if (_monsterHp > 0)
        {
            // 몬스터 턴으로 변경
            CurrentTurn = Turn.MonsterTurn;

            // 몬스터 이동력 채우기
            _monsterPointCurrent = _monsterMaxEnergy;

            Debug.Log("몬스터 턴 시작");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EndMonsterTurn()
    {
        if(_monsterPointCurrent <= 0)
        {
            Debug.Log("몬스터 턴 종료");
            StartPlayerTurn();
        }

        else if(_monsterPointCurrent > 0)
        {
            Debug.Log("몬스터의 이동력이 남았습니다.");
        }
    }
}

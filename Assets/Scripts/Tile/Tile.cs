
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.Image;

public enum TileColor
{
    Black,
    White,
    Red,
    Green,
    Blue,
}

public class Tile : MonoBehaviour
{
    static private TileStatus[] _statusSequence = { TileStatus.Frenzy, TileStatus.Recovery, TileStatus.Growth, TileStatus.Rebirth, TileStatus.Destruction };

    private int _x, _y;
    [SerializeField] private TileSO _tileDataSO;
    private TileColor _curColor;
    private Dictionary<TileStatus, List<TileStatusBase>> _statusDictionary;
    private TileSO _nextTileSO = null;
    private bool _willDestroy = false;

    public int X => _x;
    public int Y => _y;
    public TileColor Color => _curColor;
    public TileSO TileData => _tileDataSO;
    public Dictionary<TileStatus, List<TileStatusBase>> StatusDictionarty => _statusDictionary;

    public void Awake()
    {
        _statusDictionary = new Dictionary<TileStatus, List<TileStatusBase>>();
        foreach (var seq in _statusSequence)
        {
            _statusDictionary.Add(seq, new List<TileStatusBase>());
        }
    }
    /// <summary>
    /// 타일의 스킬과 상태이상 알아서 실행
    /// </summary>
    /// <param name="board"></param>
    public void ExecuteTile(Tile[,] board)//추후에 플레이어, 몬스터배열 인자 추가 필요함
    {
        //스테이터스 딕셔너리 순서대로 쭉죽 스테이터스가 가지고있는 함수 실행
        foreach (var seq in _statusSequence)
        {
            if (_statusDictionary.TryGetValue(seq, out var statusList))
            {
                foreach (var status in statusList)
                {
                    status.Execute(board, this);
                }

                statusList.Clear();
            }
        }
        // 스킬리스트 순서대로 쭉쭉 실행
        foreach (var skill in _tileDataSO.SkillSOList)
        {
            skill.Execute();
        }
    }
    /// <summary>
    /// 해당 상태이상 리스트에 상태이상 추가
    /// </summary>
    /// <param name="statusType">상태이상 종류</param>
    /// <param name="StautsData">상태이상 SO</param>
    public void AddStatus(TileStatus statusType, TileStatusBase StautsData)
    {
        _statusDictionary[statusType].Add(StautsData);
    }

    /// <summary>
    /// 기본 수치값 value로 전달해주면 Growth 중첩수만큼 더해서 반환
    /// </summary>
    /// <param name="value">수치</param>
    /// <returns></returns>
    public int GetApplyGrowth(int value)
    {
        return value + _statusDictionary[TileStatus.Growth].Count;
    }
    /// <summary>
    /// 예약된 윤회,파괴 실행, 턴매니저? 같은곳에서 마지막에 호출해야함
    /// </summary>
    public void ApplyReserve()
    {
        if(_willDestroy)
        {
            //매니저에게 풀링반환요청해야함 
            _willDestroy = false;
            this.gameObject.SetActive(false);
            return;
        }
        
        if (_nextTileSO != null)
        {
            _tileDataSO = _nextTileSO;
            _nextTileSO = null;
            //매니저? 에게도 변경알림
            //컬러, 스프라이트도 변경해야함
        }
        
    }
    /// <summary>
    /// 윤회 예약
    /// </summary>
    /// <param name="nextSO"></param>
    public void ReserveRebirth(TileSO nextSO)
    {
        _nextTileSO = nextSO; 
    }
    /// <summary>
    /// 파괴 예약
    /// </summary>
    public void ReserveDestroy()
    {
        _willDestroy = true;
    }
}

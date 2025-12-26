
using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileColor
{
    Black,
    White,
    Red,
    Green,
    Blue,
    None,
}

public class Tile : MonoBehaviour
{
    static private TileStatus[] _statusSequence = { TileStatus.Frenzy, TileStatus.Recovery, TileStatus.Growth, TileStatus.Rebirth, TileStatus.Destruction };

    [SerializeField] private int _col, _row;
    [SerializeField] private TileSO _tileDataSO;
    [SerializeField] private int _frenzyNum, _recoveryNum, _growthNum, _destructionNum, _rebirthNum;
    private HashSet<TileKeyword> _activeKeywords = new HashSet<TileKeyword>();

    private TileEventBus _eventBus;
    private TileColor _curColor;
    private Dictionary<TileStatus, List<TileStatusBase>> _statusDictionary;
    private TileSO _nextTileSO = null;
    private bool _willDestroy = false;
    private bool _willRebirth = false;
    private SpriteRenderer _renderer;
    private Action<Tile> _returnTile;


    public int Col => _col;
    public int Row => _row;
    public TileColor Color => _curColor;
    public TileSO TileData => _tileDataSO;
    public Dictionary<TileStatus, List<TileStatusBase>> StatusDictionarty => _statusDictionary;
    public TileEventBus EventBus => _eventBus;
    public bool WillDestroy => _willDestroy;
    public bool WillRebirth => _willRebirth;
    public int FrenzyNum => _frenzyNum;
    public int RecoveryNum => _recoveryNum;
    public int GrowthNum => _growthNum;
    public int DestructionNum => _destructionNum;
    public int RebirthNum => _rebirthNum;
    public bool Matched { get; set; }
    public bool replaceDestruction { get; set; }
    public bool replaceWillDestroy { get; set; }
    public void Awake()
    {
        _statusDictionary = new Dictionary<TileStatus, List<TileStatusBase>>();
        foreach (var seq in _statusSequence)
        {
            _statusDictionary.Add(seq, new List<TileStatusBase>());
        }
        _renderer = GetComponent<SpriteRenderer>();

    }


    public void Init(int row, int col, TileSO tileSO, Action<Tile> returnTile)
    {
        _col = col;
        _row = row;
        _tileDataSO = tileSO;
        _curColor = _tileDataSO.Color;
        _renderer.color = _tileDataSO.SpriteColor;
        _renderer.sprite = _tileDataSO.Sprite;
        _nextTileSO = null;
        _willDestroy = false;

        _activeKeywords.Clear();
        if (_statusDictionary != null)
        {
            foreach (var list in _statusDictionary.Values) list.Clear();
        }

        ClearStatus();


        _returnTile = returnTile;


    }

    public void SetTIlePos(int row, int col)
    {
        _row = row;
        _col = col;
    }

    /// <summary>
    /// 상태이상 발동전에 발동해야하는 스킬들 쭉쭉
    /// </summary>
    /// <param name="board"></param>
    public void ExecutePreSkill(Tile[,] board)
    {
        if (_tileDataSO.PreSkillSOList == null)
            return;
        foreach (var skill in _tileDataSO.PreSkillSOList)
        {
            skill.TryExecute(board, this);
        }
    }

    public void ExecuteStatus(Tile[,] board)
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
            }
        }
    }
    public void ClearStatus()
    {
        _frenzyNum = 0;
        _recoveryNum = 0;
        _growthNum = 0;
        _destructionNum = 0;
        _rebirthNum = 0;
        foreach (var seq in _statusSequence)
        {
            if (_statusDictionary.TryGetValue(seq, out var statusList))
            {
                statusList.Clear();
            }
        }
    }

    /// <summary>
    /// 타일의 스킬을 순서, 조건에 따라 실행.
    /// </summary>
    /// <param name="board"></param>
    public void ExecuteTile(Tile[,] board)
    {


        if (_tileDataSO.SkillSOList == null)
            return;
        // 스킬리스트 순서대로 쭉쭉 실행
        foreach (var skill in _tileDataSO.SkillSOList)
        {
            skill.TryExecute(board, this);
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

        switch (statusType)
        {
            case TileStatus.Frenzy:
                _frenzyNum++;
                Debug.Log($"{_row},{_col}에 광분 부여됨");
                break;
            case TileStatus.Recovery:
                _recoveryNum++;
                Debug.Log($"{_row},{_col}에 회복 부여됨");
                break;
            case TileStatus.Growth:
                _growthNum++;
                Debug.Log($"{_row},{_col}에 성장 부여됨");
                break;
            case TileStatus.Destruction:
                _destructionNum++;
                Debug.Log($"{_row},{_col}에 파괴 부여됨");
                break;
            case TileStatus.Rebirth:
                _rebirthNum++;
                Debug.Log($"{_row},{_col}에 윤회 부여됨");
                break;
        }
    }
    public int GetStatusCount(TileStatus status)
    {
        if (_statusDictionary.TryGetValue(status, out var list))
        {
            return list.Count;
        }
        return 0;
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
    public void ApplyReserve(Tile[,] board)
    {
        if (_willDestroy)
        {
            if(replaceWillDestroy || SkillManager.Instance.IsExecuteNextDestory)
            {

                SkillManager.Instance.IsExecuteNextDestory = false;
                _willDestroy = false;
                this.ExecuteTile(board);
            }
            else
            {
                Destroy();
                return;
            }
        }

        if (_nextTileSO != null)
        {
            _tileDataSO = _nextTileSO;
            _nextTileSO = null;
            _willRebirth = false;
            _curColor = _tileDataSO.Color;
            _renderer.color = _tileDataSO.SpriteColor;
            _renderer.sprite = _tileDataSO.Sprite;

        }
    }

    public void Destroy()
    {
        SkillManager.Instance.IncreaseDestroyCount();
        SkillManager.Instance.TileEventBus.TriggerEvent(SkillEventType.OnTileDestroyed);

        _willDestroy = false;

        _returnTile?.Invoke(this);
    }
    public void ChangeTileColor(TileColor color)
    {
        _curColor = color;
        SkillManager.Instance.TileEventBus.TriggerEvent(SkillEventType.OnColorChanged);
    }
    /// <summary>
    /// 윤회 예약
    /// </summary>
    /// <param name="nextSO"></param>
    public void ReserveRebirth(TileSO nextSO)
    {
        _nextTileSO = nextSO;
        _willRebirth = true;
    }
    /// <summary>
    /// 파괴 예약
    /// </summary>
    public void ReserveDestroy()
    {
        _willDestroy = true;
    }

    /// <summary>
    /// 키워드 활성화
    /// </summary>
    /// <param name="keyword"></param>
    public void AddKeyword(TileKeyword keyword)
    {
        _activeKeywords.Add(keyword);
    }

    /// <summary>
    /// 키워드 확인
    /// </summary>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public bool HasKeyword(TileKeyword keyword)
    {
        return _activeKeywords.Contains(keyword);
    }

    public int GetSpeed()
    {
        return _tileDataSO.Speed;
    }
}

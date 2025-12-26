using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField]private TileEventBus _eventBus = new TileEventBus();

    [SerializeField] private BoardController _boardController;

    private Dictionary<int, int> _TileSkillStack = new Dictionary<int, int>();
    private int _destoryTileCount = 0;
    public TileEventBus TileEventBus => _eventBus;
    public BoardController BoardController => _boardController;
    public int DestroyedTileCount => _destoryTileCount;

    public bool notSelfDamagedFrenzy { get; set; } = false;
    public bool IsExecuteNextDestory { get; set; } = true;
    public int TotalOverchargeIncrease { get; private set; } = 0;
    public int LastOverchargeIncrease { get; private set; } = 0;

    private void Start()
    {
        isDestroyOnLoad = false;
        if (_boardController == null)
        {
            _boardController = FindAnyObjectByType<BoardController>();
        }
    }
    public void OverchargeIncrease(int amount)
    {
        if (amount <= 0) return;

        LastOverchargeIncrease = amount;
        TotalOverchargeIncrease += amount; 
        TileEventBus.TriggerEvent(SkillEventType.OnOvercharge);
    }
    public int GetStack(int Tile_ID)
    {
        if (_TileSkillStack.ContainsKey(Tile_ID))
            return _TileSkillStack[Tile_ID];
        return 0;
    }

    public void AddStack(int skill_ID)
    {
        if (!_TileSkillStack.ContainsKey(skill_ID))
            _TileSkillStack[skill_ID] = 0;

        _TileSkillStack[skill_ID]++;
    }
    public void IncreaseDestroyCount()
    {
        _destoryTileCount++;
    }
    public void TurnStartInit()
    {
        _TileSkillStack.Clear();
        notSelfDamagedFrenzy = false;
        IsExecuteNextDestory = false;
        _destoryTileCount = 0; 
        TotalOverchargeIncrease = 0;
        LastOverchargeIncrease = 0;
    }
    public void TurnEnd()
    {

    }

    public void SetBoardController(BoardController controller)
    {
        _boardController = controller;
    }
}

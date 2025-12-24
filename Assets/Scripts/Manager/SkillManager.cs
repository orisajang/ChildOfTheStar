using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField]private TileEventBus _eventBus = new TileEventBus();

    [SerializeField] private BoardController _boardController;

    private Dictionary<int, int> _TileSkillStack = new Dictionary<int, int>();

    public TileEventBus TileEventBus => _eventBus;
    public BoardController BoardController => _boardController;

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

    public void TurnEnd()
    {
        _TileSkillStack.Clear();
    }
}

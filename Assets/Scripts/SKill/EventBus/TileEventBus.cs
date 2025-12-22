using System.Collections.Generic;

public enum TileEventType
{

}
    

public class TileEventBus
{
    private Dictionary<TileEventType, List<TileSkillBase>> _tileEvents = new Dictionary<TileEventType, List<TileSkillBase>>();

    public void Register(TileEventType type, TileSkillBase skill)
    {
        if (!_tileEvents.ContainsKey(type))
        {
            _tileEvents[type] = new List<TileSkillBase>();
        }
        _tileEvents[type].Add(skill);
    }
    
    public void Publish(TileEventType type, Tile[,] board)
    {
        if (_tileEvents.TryGetValue(type, out var skills))
        {
            for (int i = skills.Count - 1; i >= 0; i--)
            {
                skills[i].TryExecute(board, null);
            }
        }
    }


    public void ClearAll() 
    { 
        _tileEvents.Clear(); 
    }
}

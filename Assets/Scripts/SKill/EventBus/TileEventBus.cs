using System.Collections.Generic;

public enum SkillEventType
{
    OnDamage,
    OnRecovery,
    OnOvercharge,
    OnShield,
    OnTurnEnd,
    OnColorChanged,
    OnTileDestroyed
}

public class TileEventBus
{
    private Dictionary<SkillEventType, List<TileSkillBase>> _battleEvents = new Dictionary<SkillEventType, List<TileSkillBase>>();
    private Dictionary<TileStatus, List<TileSkillBase>> _statusEvents = new Dictionary<TileStatus, List<TileSkillBase>>();

    public void ClearAll()
    {
        foreach (var list in _battleEvents.Values) list.Clear();
        foreach (var list in _statusEvents.Values) list.Clear();
    }
    /// <summary>
    /// 피해, 회복, 턴 종료 등 발생 시 실행될 스킬을 등록
    /// </summary>
    public void Register(SkillEventType type, TileSkillBase skill)
    {
        AddToDict(_battleEvents, type, skill);
    }

    /// <summary>
    /// 특정 상태가 발동 될 때 실행될 스킬을 등록
    /// </summary>
    public void Register(TileStatus addedStatus, TileSkillBase skill)
    {
        AddToDict(_statusEvents, addedStatus, skill);
    }


    /// <summary>
    /// 피해, 회복, 턴 종료 등 이벤트 시 등록된 스킬 실행
    /// </summary>
    public void TriggerEvent(SkillEventType type)
    {
        Execute(_battleEvents, type, SkillManager.Instance.BoardController.BoardModel.Tiles);
    }

    /// <summary>
    /// 특정 상태가 발동 될 시 등록된 스킬 실행
    /// </summary>
    public void TriggerEvent(TileStatus addedStatus)
    {
        Execute(_statusEvents, addedStatus, SkillManager.Instance.BoardController.BoardModel.Tiles);
    }


    private void AddToDict<T>(Dictionary<T, List<TileSkillBase>> dictionary, T key, TileSkillBase skill)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary[key] = new List<TileSkillBase>();
        }
        dictionary[key].Add(skill);
    }

    private void Execute<T>(Dictionary<T, List<TileSkillBase>> dictionary, T key, Tile[,] board)
    {

        if (dictionary.TryGetValue(key, out var list))
        {

            for (int i = list.Count - 1; i >= 0; i--)
            {

                list[i].TryExecute(board, null);
            }
        }
    }
}
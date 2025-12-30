using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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

    public void TurnStartInit()
    {
        ClearAll();
    }
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
#if UNITY_EDITOR
        Debug.Log($"이벤트버스에 {type} 이벤트로 {skill.name} 스킬 등록");
#endif
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

#if UNITY_EDITOR
        Debug.Log($"{type} 이벤트 발동");
#endif
    }


    public void TriggerEvent(SkillEventType type, int val)
    {
        Execute(_battleEvents, type, SkillManager.Instance.BoardController.BoardModel.Tiles,val);

#if UNITY_EDITOR
        Debug.Log($"{type} 이벤트 발동");
#endif
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

    private void Execute<T>(Dictionary<T, List<TileSkillBase>> dictionary, T key, Tile[,] board, int val = 0)
    {
        // 1. 딕셔너리에 키(이벤트 타입)가 있는지 확인
        if (dictionary.TryGetValue(key, out var list))
        {
            // 2. 리스트에 등록된 스킬이 있는지 확인
            if (list != null && list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    // [수정됨] 0 대신 val을 넘겨줘야 정확하게 계산됨!
                    list[i].TryExecute(board, null, val);

#if UNITY_EDITOR
                    Debug.Log($"<color=green>[성공]</color> {key} 이벤트 발동! -> {list[i].name} 스킬 실행함. (전달값: {val})");
#endif
                }
            }
            else
            {
#if UNITY_EDITOR
                // 리스트는 있는데 비어있음
                Debug.LogWarning($"<color=yellow>[실패]</color> {key} 이벤트가 왔는데, 등록된 스킬 리스트가 비어있습니다 (Count 0).");
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            // 딕셔너리에 키조차 없음 (한 번도 Register 된 적 없음)
            Debug.LogWarning($"<color=red>[실패]</color> {key} 이벤트는 아무도 등록(Register)하지 않았습니다.");
#endif
        }
    }
}
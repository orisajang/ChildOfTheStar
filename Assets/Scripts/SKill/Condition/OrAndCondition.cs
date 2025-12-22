
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "OrAndCondition", menuName = "Scriptable Objects/SkillCondition/OrAndCondition")]
public class OrAndCondition : SkillConditionBase
{
    public enum ConditionType
    {
        Or,
        And,
    }
    [SerializeField] ConditionType _conditionType;
    [SerializeField]List<SkillConditionBase> _conditionList = new List<SkillConditionBase>();
    public override bool CanExecute(Tile[,] board, Tile casterTile)
    {
        if (_conditionList == null || _conditionList.Count == 0)
            return true;

        if (_conditionType == ConditionType.Or)
        {
            foreach (var condition in _conditionList)
            {
                if (condition.CanExecute(board, casterTile))
                {
                    return true;
                }
            }
            return false;
        }
        else if (_conditionType == ConditionType.And)
        {
            foreach (var condition in _conditionList)
            {
                if (!condition.CanExecute(board, casterTile))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}

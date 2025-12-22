using UnityEngine;


[CreateAssetMenu(fileName = "IsSameColor", menuName = "Scriptable Objects/SkillCondition/IsSameColor")]
public class IsSameColor : SkillConditionBase
{
    public enum CompareType
    { 
        Equal, 
        NotEqual 
    }
    [Tooltip("효과 발동한 타일과 비교 할 색")]
    [SerializeField] private TileColor _color;
    [Tooltip("Equal = 색이 같을시 true반환, NotEqual = 색이 다를시 true반환")]
    [SerializeField] private CompareType _compareType = CompareType.Equal;

    public override bool CanExecute(Tile[,] board, Tile casterTile)
    {
        if (casterTile == null) return false;

        bool isColorMatch = (_color == casterTile.Color);

        return _compareType == CompareType.Equal ? isColorMatch : !isColorMatch;
    }
}

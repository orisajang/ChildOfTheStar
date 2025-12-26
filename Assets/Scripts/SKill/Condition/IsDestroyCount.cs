using UnityEngine;

[CreateAssetMenu(fileName = "IsDestroyCount", menuName = "Scriptable Objects/SkillCondition/IsDestroyCount")]
public class IsDestroyCount : SkillConditionBase
{
    [Tooltip("타일 효과로 파괴 횟수, 해당 숫자 횟수 마다 ")]
    [SerializeField] private int _count = 2;

    public override bool CanExecute(Tile[,] board, Tile casterTile)
    {
        int currentCount = SkillManager.Instance.DestroyedTileCount;

        return currentCount > 0 && (currentCount % _count == 0);
    }
}
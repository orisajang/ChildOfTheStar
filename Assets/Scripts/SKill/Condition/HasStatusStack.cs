using UnityEngine;

[CreateAssetMenu(fileName = "HasStatusStack", menuName = "Scriptable Objects/SkillCondition/HasStatusStack")]
public class HasStatusStack : SkillConditionBase
{
    [SerializeField] private bool _checkTotalCount; 

    [SerializeField] private TileStatus _targetStatus;

    [SerializeField] private int _stackCount;

    public override bool CanExecute(Tile[,] board, Tile casterTile)
    {
        int currentCount = 0;

        if (_checkTotalCount)
        {
            currentCount = casterTile.FrenzyNum +
                           casterTile.RecoveryNum +
                           casterTile.GrowthNum +
                           casterTile.DestructionNum + 
                           casterTile.RebirthNum;
        }
        else
        {
            currentCount = casterTile.GetStatusCount(_targetStatus);
        }

        return currentCount >= _stackCount;
    }
}

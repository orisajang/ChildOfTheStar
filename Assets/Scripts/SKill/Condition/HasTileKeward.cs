using UnityEngine;

[CreateAssetMenu(fileName = "HasTileKeward", menuName = "Scriptable Objects/SkillCondition/HasTileKeward")]
public class HasTileKeward : SkillConditionBase
{
    [SerializeField] private TileKeyword _targetKeyWord;


    public override bool CanExecute(Tile[,] board, Tile casterTile)
    {
        return casterTile.HasKeyword(_targetKeyWord);
    }
}

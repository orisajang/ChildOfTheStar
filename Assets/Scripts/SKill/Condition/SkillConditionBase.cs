using UnityEngine;

public abstract class SkillConditionBase : ScriptableObject
{
    [TextArea(3, 10)]
    public string descriptionText;
    public abstract bool CanExecute(Tile[,] board, Tile casterTile);
}

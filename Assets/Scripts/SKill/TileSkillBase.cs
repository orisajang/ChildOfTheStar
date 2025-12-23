using UnityEngine;

public abstract class TileSkillBase: ScriptableObject
{
    [TextArea(3, 10)]
    public string descriptionText;
    [Header("비어있으면 조건x 바로 실행")]
    [SerializeField]private SkillConditionBase _skillCondition;

    public  void TryExecute(Tile[,] board, Tile casterTile)
    {
        if (_skillCondition == null || _skillCondition.CanExecute(board,casterTile))
        {
           Execute(board, casterTile);
           Debug.Log($"{casterTile.TileData.Name}");
        }
    }
    protected abstract void Execute(Tile[,] board, Tile casterTile);
}

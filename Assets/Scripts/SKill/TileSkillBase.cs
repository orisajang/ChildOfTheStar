using UnityEngine;

public abstract class TileSkillBase: ScriptableObject
{
    [TextArea(3, 10)]
    public string descriptionText;
    [Header("비어있으면 조건x 바로 실행")]
    [SerializeField]private SkillConditionBase _skillCondition;
    public  void TryExecute(Tile[,] board, Tile casterTile, int val = 0)
    {
        if (_skillCondition == null || _skillCondition.CanExecute(board,casterTile))
        {
           Execute(board, casterTile);
        }
#if UNITY_EDITOR
        if(casterTile != null)
            Debug.Log($"{casterTile.TileData.Name}");
        else
            Debug.Log($"{descriptionText}");
#endif
    }
    protected abstract void Execute(Tile[,] board, Tile casterTile);
    protected virtual void Execute(Tile[,] board, Tile casterTile, int val = 0)
    {
        Execute(board, casterTile);
    }
}

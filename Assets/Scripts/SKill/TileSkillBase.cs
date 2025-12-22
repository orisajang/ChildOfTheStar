using UnityEngine;

public abstract class TileSkillBase: ScriptableObject
{
    [TextArea(3, 10)]
    public string descriptionText;
    [Header("비어있으면 조건x 바로 실행")]
    [SerializeField]private SkillConditionBase _skillCondition;
    [Tooltip("반복 횟수, 최소 1이상")]
    [SerializeField] private int repeatNum;

    public  void TryExecute(Tile[,] board, Tile casterTile)
    {
        if (_skillCondition == null || _skillCondition.CanExecute(board,casterTile))
        {
            for(int i = 0; i < repeatNum; i++)
            {
                Execute(board, casterTile);
            }
        }
    }
    protected abstract void Execute(Tile[,] board, Tile casterTile);
}

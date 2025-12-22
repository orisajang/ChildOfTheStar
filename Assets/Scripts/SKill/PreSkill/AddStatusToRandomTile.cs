using UnityEngine;

[CreateAssetMenu(fileName = "AddStatusToRandomTile", menuName = "Scriptable Objects/PreSkill/AddStatusToRandomTile")]
public class AddStatusToRandomTile : TileSkillBase
{
    [SerializeField] TileStatus _status;
    [SerializeField] TileStatusBase _tileStatus;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        if (_tileStatus == null) return;

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        int targetRow =Random.Range(0, row);
        int targetCol =Random.Range(0, col);

        if (row * col <= 1) return;


            while (board[targetRow, targetCol]==casterTile)
        {
            targetRow = Random.Range(0, row);
            targetCol = Random.Range(0, col);
        }
        board[targetRow, targetCol].AddStatus(_status, _tileStatus);
    }
}

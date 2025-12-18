using UnityEngine;

[CreateAssetMenu(fileName = "RebirthStatus", menuName = "Scriptable Objects/Status/RebirthStatus")]
public class RebirthStatus : TileStatusBase
{
    public override void Execute(Tile[,] board, Tile casterTile)
    {
        int cols = board.GetLength(1);
        int rows = board.GetLength(0);
         
        int col = Random.Range(0, cols);
        int row = Random.Range(0, rows);

        if (cols * rows > 1)
        {
            while (col == casterTile.Col && row == casterTile.Row)
            {

                col = Random.Range(0, cols);
                row = Random.Range(0, rows);
            }
        }
        board[row, col].ReserveRebirth(casterTile.TileData);

        Debug.Log("윤회 예약");
    }
}

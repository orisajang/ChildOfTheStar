using UnityEngine;

[CreateAssetMenu(fileName = "RebirthStatus", menuName = "Scriptable Objects/Status/RebirthStatus")]
public class RebirthStatus : TileStatusBase
{
    public override void Execute(Tile[,] board, Tile casterTile)
    {
        int cols = board.GetLength(1);
        int rows = board.GetLength(0);
         
        int x = Random.Range(0, cols);
        int y = Random.Range(0, rows);

        if (cols * rows > 1)
        {
            while (x == casterTile.X && y == casterTile.Y)
            {

                x = Random.Range(0, cols);
                y = Random.Range(0, rows);
            }
        }
        board[y, x].ReserveRebirth(casterTile.TileData);

        Debug.Log("윤회 예약");
    }
}

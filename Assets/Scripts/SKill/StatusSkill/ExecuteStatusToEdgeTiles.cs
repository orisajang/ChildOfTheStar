using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ExecuteStatusToEdgeTiles", menuName = "Scriptable Objects/StatusSkill/ExecuteStatusToEdgeTiles")]
public class ExecuteStatusToEdgeTiles : TileSkillBase
{
    protected override void Execute(Tile[,] board, Tile casterTile)
    {

        List<Tile> edgeTiles = ListPool<Tile>.Get();

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Tile target = board[r, c];

                if (target == null) continue;

              
                bool isEdge = (r == 0 || r == row - 1 || c == 0 || c == col - 1);

                if (isEdge)
                {
                    edgeTiles.Add(target);
                }
            }
        }

        if (edgeTiles.Count > 0)
        {
            foreach (Tile tile in edgeTiles)
            {
                tile.ExecuteStatus(board);
            }
        }

        ListPool<Tile>.Release(edgeTiles);
    }
}
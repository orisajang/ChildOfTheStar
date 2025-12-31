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
                if(board[r, c] == null) 
                    continue;
                Tile target = board[r, c];

                if (target == null) 
                    continue;

              
                bool isEdge = (r == 0 || r == row - 1 || c == 0 || c == col - 1);

                if (isEdge)
                {
                    edgeTiles.Add(target);
                }
            }
        }

#if UNITY_EDITOR
        Debug.Log($"가장자리 타일수 {edgeTiles.Count}");
#endif
        if (edgeTiles.Count > 0)
        {
            foreach (Tile tile in edgeTiles)
            {
                if (tile != null)
                    tile.ExecuteStatus(board,true);

#if UNITY_EDITOR
                Debug.Log($"가장 자리 상태 실행({tile.Row}, {tile.Col}).");
#endif
            }
        }

        ListPool<Tile>.Release(edgeTiles);
    }
}
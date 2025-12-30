using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "AddStatusToEdgeTiles", menuName = "Scriptable Objects/StatusSkill/AddStatusToEdgeTiles")]
public class AddStatusToEdgeTiles : TileSkillBase
{
    [SerializeField] private TileStatus _statusType;
    [SerializeField] private TileStatusBase _tileStatus;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {

        List<Tile> edgeTiles = ListPool<Tile>.Get();

        int rows = board.GetLength(0);
        int col = board.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < col; c++)
            {
                if (board[r, c] == null) 
                    continue;

                Tile target = board[r, c];

                if (target == null) 
                    continue;

              
                bool isEdge = (r == 0 || r == rows - 1 || c == 0 || c == col - 1);

                if (isEdge)
                {
                    edgeTiles.Add(target);
                }
            }
        }
#if UNITY_EDITOR
        Debug.Log("붕괴부여");
#endif 
        foreach (Tile tile in edgeTiles)
        {
            tile.AddStatus(_statusType, _tileStatus);
        }

        ListPool<Tile>.Release(edgeTiles);
    }
}
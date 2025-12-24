using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ConvertSurroundTileStatus", menuName = "Scriptable Objects/StatusSkill/ConvertSurroundTileStatus")]
public class ConvertSurroundTileStatus : TileSkillBase
{
    [SerializeField] TileStatusBase _statusData;
    [SerializeField] TileStatus _status;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        List<Tile> targetTiles = ListPool<Tile>.Get();

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        int centerRow = casterTile.Row;
        int centerCol = casterTile.Col;

        for (int r = centerRow - 1; r <= centerRow + 1; r++)
        {
            for (int c = centerCol - 1; c <= centerCol + 1; c++)
            {
                if (r < 0 || r >= row || c < 0 || c >= col) continue;

                Tile target = board[r, c];

                if (target == null) continue;
                if (target == casterTile) continue;

                targetTiles.Add(target);
            }
        }

        if (targetTiles.Count == 0)
        {
            ListPool<Tile>.Release(targetTiles);
            return;
        }

        for (int i = 0; i < targetTiles.Count; i++)
        {
            Tile target = targetTiles[i];
            int totalCount = 0;

            totalCount += target.GetStatusCount(TileStatus.Frenzy);
            totalCount += target.GetStatusCount(TileStatus.Recovery);
            totalCount += target.GetStatusCount(TileStatus.Growth);
            totalCount += target.GetStatusCount(TileStatus.Destruction);
            totalCount += target.GetStatusCount(TileStatus.Rebirth);

            if (totalCount > 0)
            {
                target.ClearStatus();

                for (int count = 0; count < totalCount; count++)
                {
                    target.AddStatus(_status, _statusData);
                }
            }
        }

        ListPool<Tile>.Release(targetTiles);
    }
}
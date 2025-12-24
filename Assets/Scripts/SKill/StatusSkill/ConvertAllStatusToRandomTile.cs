using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ConvertAllStatusToRandomTile", menuName = "Scriptable Objects/StatusSkill/ConvertAllStatusToRandomTile")]
public class ConvertAllStatusToRandomTile : TileSkillBase
{
    [SerializeField] TileStatusBase _statusData;
    [SerializeField] TileStatus tileStatus;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
       
        List<Tile> targetlist = ListPool<Tile>.Get();

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        int totalRemovedCount = 0;

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Tile target = board[r, c];

                if (target == null) continue;

                int currentTileStatusCount = 0;
                currentTileStatusCount += target.GetStatusCount(TileStatus.Frenzy);
                currentTileStatusCount += target.GetStatusCount(TileStatus.Recovery);
                currentTileStatusCount += target.GetStatusCount(TileStatus.Growth);
                currentTileStatusCount += target.GetStatusCount(TileStatus.Destruction);
                currentTileStatusCount += target.GetStatusCount(TileStatus.Rebirth);

                totalRemovedCount += currentTileStatusCount;

                if (currentTileStatusCount > 0)
                {
                    target.ClearStatus();
                }

                targetlist.Add(target);
            }
        }

        if (totalRemovedCount > 0 && targetlist.Count > 0)
        {
            for (int i = 0; i < totalRemovedCount; i++)
            {
                int randomIndex = Random.Range(0, targetlist.Count);
                Tile selectedTile = targetlist[randomIndex];

                selectedTile.AddStatus(tileStatus, _statusData);
            }
        }

        ListPool<Tile>.Release(targetlist);
    }
}
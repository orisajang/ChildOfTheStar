using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "DistributeStatusToRandomTiles", menuName = "Scriptable Objects/StatusSkill/DistributeStatusToRandomTiles")]
public class DistributeStatusToRandomTiles : TileSkillBase
{
    [SerializeField] TileColor _color = TileColor.None;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        List<Tile> targetTiles = ListPool<Tile>.Get();

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Tile target = board[r, c];

                if (target == null) continue;
                if (target == casterTile) continue;

                bool isColorMatch = (_color == TileColor.None) || (target.Color == _color);

                if (isColorMatch)
                {
                    targetTiles.Add(target);
                }
            }
        }

        List<TileStatus> statusTypeList = ListPool<TileStatus>.Get();
        List<TileStatusBase> statusDataList = ListPool<TileStatusBase>.Get();

        foreach (var statusGroup in casterTile.StatusDictionarty)
        {
            TileStatus status = statusGroup.Key;
            List<TileStatusBase> statusList = statusGroup.Value;

            foreach (var tileStatus in statusList)
            {
                statusTypeList.Add(status);
                statusDataList.Add(tileStatus);
            }
        }
        casterTile.ClearStatus();

        for (int i = 0; i < statusTypeList.Count; i++)
        {
            int randomIndex = Random.Range(0, targetTiles.Count);

            TileStatus status = statusTypeList[i];
            TileStatusBase tileStatus = statusDataList[i];

            targetTiles[randomIndex].AddStatus(status, tileStatus);
        }

        ListPool<Tile>.Release(targetTiles);
        ListPool<TileStatus>.Release(statusTypeList);
        ListPool<TileStatusBase>.Release(statusDataList);
    }
}
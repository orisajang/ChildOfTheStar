using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ConvertSurroundTileStatus", menuName = "Scriptable Objects/StatusSkill/ConvertSurroundTileStatus")]
public class ConvertSurroundTileStatus : TileSkillBase
{
    [SerializeField] TileStatus _statusType;
    [SerializeField] TileStatusBase _tileStatus;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
#if UNITY_EDITOR
        Debug.Log($"주변 타일 상태이상 변환 스킬 실행");
#endif
        List<Tile> targetTiles = ListPool<Tile>.Get();

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        int centerRow = casterTile.Row;
        int centerCol = casterTile.Col;

        for (int r = centerRow - 1; r <= centerRow + 1; r++)
        {
            for (int c = centerCol - 1; c <= centerCol + 1; c++)
            {
                if (r < 0 || r >= row || c < 0 || c >= col) 
                    continue;

                Tile target = board[r, c];

                if (target == null) continue;
                if (target == casterTile) continue;
                if (target.Matched) continue;

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
                    target.AddStatus(_statusType, _tileStatus);
#if UNITY_EDITOR
                    Debug.Log($"타일 {target.Row},{target.Col}에 {_statusType}상태이상 {count + 1}회 부여");
#endif
                }
            }
        }

        ListPool<Tile>.Release(targetTiles);
    }
}
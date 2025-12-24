using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "AddStatusToRandomTile", menuName = "Scriptable Objects/StatusSkill/AddStatusToRandomTile")]
public class AddStatusToRandomTile : TileSkillBase
{
    [SerializeField] TileStatus _status;
    [SerializeField] TileColor _color = TileColor.None;
    [SerializeField] TileStatusBase _tileStatus;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        if (_tileStatus == null) return;

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

        if (targetTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, targetTiles.Count);
            targetTiles[randomIndex].AddStatus(_status, _tileStatus);
        }
        ListPool<Tile>.Release(targetTiles);
    }
}

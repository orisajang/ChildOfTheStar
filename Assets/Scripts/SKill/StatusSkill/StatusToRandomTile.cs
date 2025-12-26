using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool; 

[CreateAssetMenu(fileName = "StatusToRandomTile", menuName = "Scriptable Objects/StatusSkill/StatusToRandomTile")]
public class StatusToRandomTile : TileSkillBase
{
    [SerializeField] private TileColor _targetColor = TileColor.None; 

    [SerializeField] private TileStatus _Status = TileStatus.Rebirth; 

    [SerializeField] private TileStatusBase _tileStatus;

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        List<Tile> tiles = ListPool<Tile>.Get();

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Tile target = board[r, c];

                if (target == null) continue;

                bool isColorMatch = (_targetColor == TileColor.None) || (target.Color == _targetColor);

                if (isColorMatch)
                {
                    tiles.Add(target);
                }
            }
        }

        if (tiles.Count > 0)
        {
            int rand = Random.Range(0, tiles.Count);
            Tile tile = tiles[rand];

            tile.AddStatus(_Status, _tileStatus);
        }

        ListPool<Tile>.Release(tiles);
    }
}
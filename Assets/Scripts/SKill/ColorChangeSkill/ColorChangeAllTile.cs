using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ColorChangeAllTile", menuName = "Scriptable Objects/ColorChange/ColorChangeAllTile")]
public class ColorChangeAllTile : TileSkillBase
{
    [Tooltip("탐색할 목표 타일의 색, None일 경우 모든 색")]
    [SerializeField] TileColor _searchColor = TileColor.None;
    [Tooltip("변경 후 색, None일경우 랜덤")]
    [SerializeField] TileColor _applyColor = TileColor.White;
    private TileColor[] _colors = new TileColor[] { TileColor.Black, TileColor.White, TileColor.Red, TileColor.Blue, TileColor.Green };
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        TileColor applyColor = _applyColor;
        

        int row = board.GetLength(0);
        int col = board.GetLength(1);

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                Tile target = board[r, c];

                if (target == null) continue;

                if (target == casterTile) continue;

                bool isColorMatch = (_searchColor == TileColor.None) || (target.Color == _searchColor);

                if (isColorMatch)
                {
                    if (_applyColor == TileColor.None)
                    {
                        int randomIndex = Random.Range(0, _colors.Length);
                        applyColor = _colors[randomIndex];
                    }
                    target.ChangeTileColor(applyColor);
                }
            }
        }

    }
}

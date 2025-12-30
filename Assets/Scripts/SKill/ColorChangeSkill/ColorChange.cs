using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ColorChange", menuName = "Scriptable Objects/ColorChange/ColorChange")]
public class ColorChange : TileSkillBase
{
    [Tooltip("탐색할 목표 타일의 색, None일 경우 모든 색")]
    [SerializeField] TileColor _searchColor = TileColor.None;
    [Tooltip("변경 후 색, None일경우 랜덤")]
    [SerializeField] TileColor _applyColor = TileColor.White;
    [Tooltip("색상별 스프라이트 리스트")]
    [SerializeField] private List<TileColorData> _colorDataList = new List<TileColorData>();
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

                bool isColorMatch = (_searchColor == TileColor.None) || (target.Color == _searchColor);

                if (isColorMatch)
                {
                    targetTiles.Add(target);
                }
            }
        }

        if (targetTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, targetTiles.Count);

            TileColor finalColor;
            Sprite finalSprite = null;

            if (_applyColor == TileColor.None)
            {
                int randomColor = Random.Range(0, _colorDataList.Count);
                TileColorData randomData = _colorDataList[randomColor];

                finalColor = randomData.Color;
                finalSprite = randomData._sprite;
            }
            else
            {
                finalColor = _applyColor;
                foreach (var data in _colorDataList)
                {
                    if (data.Color == finalColor)
                    {
                        finalSprite = data._sprite;
                    }
                }
            }

            targetTiles[randomIndex].ChangeTileColor(finalColor, finalSprite);
        }
        ListPool<Tile>.Release(targetTiles);
    }
}

using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
[System.Serializable]
public class TileColorData
{
    public TileColor Color;
    public Sprite _sprite;
}


[CreateAssetMenu(fileName = "ColorChangeAllTile", menuName = "Scriptable Objects/ColorChange/ColorChangeAllTile")]
public class ColorChangeAllTile : TileSkillBase
{
    [Tooltip("탐색할 목표 타일의 색, None일 경우 모든 색")]
    [SerializeField] TileColor _searchColor = TileColor.None;
    [Tooltip("변경 후 색, None일경우 랜덤")]
    [SerializeField] TileColor _applyColor = TileColor.White;
    [Tooltip("색상별 스프라이트 리스트")]
    [SerializeField] private List<TileColorData> _colorDataList = new List<TileColorData>();
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
                    TileColor finalColor;
                    Sprite finalSprite=null;

                    if (_applyColor == TileColor.None)
                    {
                        int randomIndex = Random.Range(0, _colorDataList.Count);
                        TileColorData randomData = _colorDataList[randomIndex];

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
                    target.ChangeTileColor(finalColor, finalSprite);
                }
            }
        }

    }
}

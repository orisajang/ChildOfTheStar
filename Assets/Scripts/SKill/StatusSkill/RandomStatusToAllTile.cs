using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "RandomStatusToAllTile", menuName = "Scriptable Objects/StatusSkill/RandomStatusToAllTile")]
public class RandomStatusToAllTile : TileSkillBase
{
    
    [Tooltip("None이면 모든 색 대상")]
    [SerializeField] TileColor _color = TileColor.None;
    [SerializeField] List<RandStatus> _StatusList;
   
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        if (_StatusList == null) return;

        List<Tile> _tileList = ListPool<Tile>.Get();

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
                    int randomIndex = Random.Range(0, _tileList.Count);
                    int randomStatusIndex = Random.Range(0, _StatusList.Count);
                    _tileList[randomIndex].AddStatus(_StatusList[randomStatusIndex].Status, _StatusList[randomStatusIndex].TileStatus);
                }
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
[System.Serializable]
public class RandStatus
{
    [SerializeField] private TileStatus _statustype;
    [SerializeField] private TileStatusBase _tileStatus;
    public TileStatus Status => _statustype;
    public TileStatusBase TileStatus => _tileStatus;
}

[CreateAssetMenu(fileName = "RandomStatusToRandomTile", menuName = "Scriptable Objects/StatusSkill/RandomStatusToRandomTile")]
public class RandomStatusToRandomTile : TileSkillBase
{
    
    [Tooltip("None이면 모든 색 대상")]
    [SerializeField] TileColor _color = TileColor.None;
    [SerializeField] List<RandStatus> _statusList;
   
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        if (_statusList == null) return;

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
                    _tileList.Add(target);
                }
            }
        }

        if (_tileList.Count > 0)
        {
            int randomIndex = Random.Range(0, _tileList.Count);
            int randomStatusIndex = Random.Range(0, _statusList.Count);
            _tileList[randomIndex].AddStatus(_statusList[randomStatusIndex].Status, _statusList[randomStatusIndex].TileStatus);
        }
        ListPool<Tile>.Release(_tileList);
    }
}

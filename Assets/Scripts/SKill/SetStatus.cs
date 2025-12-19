using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SetStatus", menuName = "Scriptable Objects/PreSkill/SetStatus")]
public class SetStatus : TileSkillBase
{
    [SerializeField] TileStatus _status;
    [SerializeField] Dictionary<TileStatus, TileStatusBase> _statusDictionary;
    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        int row = board.GetLength(0);
        int col = board.GetLength(1);

        int targetRow =Random.Range(0, row);
        int targetCol =Random.Range(0, col);

        board[targetRow, targetCol].AddStatus(_status, _statusDictionary[_status]);
        Debug.Log($"타일에 회복 부여");
    }
}

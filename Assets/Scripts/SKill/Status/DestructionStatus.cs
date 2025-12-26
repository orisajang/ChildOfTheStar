using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestructionStatus", menuName = "Scriptable Objects/Status/DestructionStatus")]
public class DestructionStatus : TileStatusBase
{
    private List<Tile> _targets = new List<Tile>(20);
    public override void Execute(Tile[,] board, Tile casterTile)
    {
        SkillManager.Instance.TileEventBus.TriggerEvent(TileStatus);
        int startRow = casterTile.Row;
        int startCol = casterTile.Col;
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        for (int r = 1; r <= 5; r++)
        {
            _targets.Clear();
            for (int y = -r; y <= r; y++)
            {
                for (int x = -r; x <= r; x++)
                {
                    if (Mathf.Abs(x) != r && Mathf.Abs(y) != r)
                        continue;
                    int targetRow = startRow + y;
                    int targetCol = startCol + x;
                    if (targetRow < 0 || targetRow >= rows || targetCol < 0 || targetCol >= cols)
                        continue;
                    Tile target = board[targetRow, targetCol];
                    if (target == null)
                        continue;
                    if (target.Color == casterTile.Color)
                        continue;
                    if (target.Matched)
                        continue;
                    if (target.WillDestroy)
                        continue;

                    _targets.Add(target);
                }
            }
            if (_targets.Count == 0)
                continue;
            int rand = Random.Range(0, _targets.Count);
            _targets[rand].ReserveDestroy();

            if (casterTile.replaceDestruction)
            {
                _targets[rand].replaceWillDestroy = true;
            }
            return;
        }
        casterTile.replaceDestruction = false;
        return;
    }
}

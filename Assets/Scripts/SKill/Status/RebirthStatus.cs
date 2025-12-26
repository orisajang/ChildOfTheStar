using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "RebirthStatus", menuName = "Scriptable Objects/Status/RebirthStatus")]

public class RebirthStatus : TileStatusBase
{
    private static List<Tile> _targets = new List<Tile>(30);
    public override void Execute(Tile[,] board, Tile casterTile)
    {
        SkillManager.Instance.TileEventBus.TriggerEvent(TileStatus);
        _targets.Clear();
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);


        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Tile tile = board[row, col];


                if (tile == null) 
                    continue;
                if (tile == casterTile) 
                    continue;

                _targets.Add(tile);
            }
        }

        for (int i = _targets.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);

            Tile temp = _targets[i];
            _targets[i] = _targets[rnd];
            _targets[rnd] = temp;
        }

        foreach (Tile tile in _targets)
        {
            if (tile.WillRebirth) 
                continue;

            tile.ReserveRebirth(casterTile.TileData);
            return;
        }

    }
}
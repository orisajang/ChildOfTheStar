using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RepeatMatchedSkills", menuName = "Scriptable Objects/Skill/RepeatMatchedSkills")]
public class RepeatMatchedSkills : TileSkillBase
{
    private static HashSet<Tile> _tileHashset = new HashSet<Tile>();

    protected override void Execute(Tile[,] board, Tile casterTile)
    {
        if (_tileHashset.Contains(casterTile)) return;

        _tileHashset.Add(casterTile);

        List<Tile> connectedTiles = GetMatchedTiles(board, casterTile);

        if (connectedTiles == null || connectedTiles.Count == 0) return;

        foreach (Tile otherTile in connectedTiles)
        {
            if (otherTile == casterTile) continue;

            if (otherTile != null)
            {
                otherTile.ExecuteTile(board);
            }
        }
        _tileHashset.Remove(casterTile);
    }

    private List<Tile> GetMatchedTiles(Tile[,] board, Tile startTile)
    {
        List<Tile> result = new List<Tile>();
        HashSet<Tile> visited = new HashSet<Tile>();
        Queue<Tile> queue = new Queue<Tile>();

        if (startTile.Matched)
        {
            queue.Enqueue(startTile);
            visited.Add(startTile);
            result.Add(startTile);
        }

        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        int[] dirRow = { -1, 1, 0, 0 };
        int[] dirCol = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            Tile current = queue.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int nextRow = current.Row + dirRow[i];
                int nextCol = current.Col + dirCol[i];

                if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= cols)
                    continue;

                Tile neighbor = board[nextRow, nextCol];

                if (neighbor == null) continue;
                if (visited.Contains(neighbor)) continue;

                if (neighbor.Color == startTile.Color && neighbor.Matched)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    result.Add(neighbor);
                }
            }
        }

        return result;
    }
}
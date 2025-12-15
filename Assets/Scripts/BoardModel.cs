using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum TileColor
{
    Red,
    Green,
    Blue,
    White,
    Black
}
public abstract class TileTest
{
    public TileColor color;
    public skillTest skillTest;

}

public abstract class skillTest
{
    public abstract void Excute();
}
public class BoardModel
{
    private int _rows = 5;
    public int Rows => _rows;

    private int _columns = 6;
    public int Columns => _columns;

    private TileTest[,] _tiles;

    public void Init()
    {
        _tiles = new TileTest[Rows, Columns];
    }

    public enum TileMoveDirection
    {
        Horizontal, // 가로 라인 이동 (row 한 줄을 좌 / 우로 이동)
        Vertical    // 세로 라인 이동 (col 한 줄을 상 / 하로 이동)
    }
    private struct Pos
    {
        public int r;
        public int c;

        public Pos(int r, int c)
        {
            this.r = r;
            this.c = c;
        }
    }

    /// <summary>
    /// 타일 이동시키는 함수
    /// </summary>
    /// <param name="direction">
    /// 이동할 라인의 방향
    /// Horizontal : 가로 라인(row) 한 줄을 좌 / 우로 이동
    /// Vertical   : 세로 라인(col) 한 줄을 상 / 하로 이동
    /// </param>
    /// <param name="lineIndex">
    /// 이동할 라인의 인덱스
    /// Horizontal일 경우 : row 인덱스
    /// Vertical일 경우   : col 인덱스
    /// </param>
    /// <param name="moveAmount">
    /// 이동할 칸 수
    /// + : 우 / 위
    /// - : 좌 / 아래
    /// </param>
    public void MoveTile(TileMoveDirection direction, int lineIndex, int moveAmount)
    {
        if (moveAmount == 0) return;

        if (direction == TileMoveDirection.Horizontal)
        {
            if (lineIndex < 0 || lineIndex >= _rows) return;

            TileTest[] tempRow = new TileTest[_columns];

            for (int col = 0; col < _columns; col++)
            {
                int newCol = ((col + moveAmount) % _columns + _columns) % _columns;
                tempRow[newCol] = _tiles[lineIndex, col];
            }

            for (int col = 0; col < _columns; col++)
            {
                _tiles[lineIndex, col] = tempRow[col];
            }
        }
        else if (direction == TileMoveDirection.Vertical)
        {
            if (lineIndex < 0 || lineIndex >= _columns) return;

            TileTest[] tempCol = new TileTest[_rows];

            for (int row = 0; row < _rows; row++)
            {
                int newRow = ((row + moveAmount) % _rows + _rows) % _rows;
                tempCol[newRow] = _tiles[row, lineIndex];
            }

            for (int row = 0; row < _rows; row++)
            {
                _tiles[row, lineIndex] = tempCol[row];
            }
        }

        HashSet<Pos> matched = new HashSet<Pos>();

        if (direction == TileMoveDirection.Horizontal)
        {
            MatchTile(TileMoveDirection.Horizontal, lineIndex, matched);

            for (int col = 0; col < _columns; col++)
            {
                MatchTile(TileMoveDirection.Vertical, col, matched);
            }
        }
        else
        {
            MatchTile(TileMoveDirection.Vertical, lineIndex, matched);

            for (int row = 0; row < _rows; row++)
            {
                MatchTile(TileMoveDirection.Horizontal, row, matched);
            }
        }

        ExplodeMatched(matched);

        if (matched.Count > 0)
        {
            ApplyGravity();
        }
    }

    /// <summary>
    /// 타일매치하는 함수, 3개이상이면 해시셋에 저장. 중복 방지 및 탐색 전에 없애는거 방지하기위해 해시셋 사용
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="lineIndex"></param>
    /// <param name="matched"></param>
    private void MatchTile(TileMoveDirection direction, int lineIndex, HashSet<Pos> matched)
    {
        if (direction == TileMoveDirection.Horizontal)
        {
            if (lineIndex < 0 || lineIndex >= _rows) return;

            int count = 1;

            TileTest prevTile = _tiles[lineIndex, 0];

            for (int col = 1; col <= _columns; col++)
            {
                TileTest currentTile = null;

                if (col < _columns)
                {
                    currentTile = _tiles[lineIndex, col];
                }

                bool isSame = false;
                if (prevTile != null && currentTile != null)
                {
                    if (prevTile.color == currentTile.color)
                    {
                        isSame = true;
                    }
                }

                if (isSame)
                {
                    count++;
                }
                else
                {

                    if (count >= 3 && prevTile != null)
                    {
                        for (int i = 1; i <= count; i++)
                        {
                            int targetCol = col - i;

                            matched.Add(new Pos(lineIndex, targetCol));
                        }
                    }

                    count = 1;
                    prevTile = currentTile;
                }
            }
        }
        else
        {
            if (lineIndex < 0 || lineIndex >= _columns) return;

            int count = 1;
            TileTest prevTile = _tiles[0, lineIndex];

            for (int row = 1; row <= _rows; row++)
            {
                TileTest currentTile = null;
                if (row < _rows)
                {
                    currentTile = _tiles[row, lineIndex];
                }

                bool isSame = false;
                if (prevTile != null && currentTile != null)
                {
                    if (prevTile.color == currentTile.color)
                    {
                        isSame = true;
                    }
                }

                if (isSame)
                {
                    count++;
                }
                else
                {
                    if (count >= 3 && prevTile != null)
                    {
                        for (int i = 1; i <= count; i++)
                        {
                            int targetRow = row - i;
                            matched.Add(new Pos(targetRow, lineIndex));
                        }
                    }

                    count = 1;
                    prevTile = currentTile;
                }
            }
        }
    }
    /// <summary>
    /// 해시 셋에 있는 타일들 제거 및 스킬실행
    /// </summary>
    /// <param name="matched"></param>
    private void ExplodeMatched(HashSet<Pos> matched)
    {
        if (matched == null) return;
        if (matched.Count == 0) return;

        foreach (Pos p in matched)
        {
            TileTest t = _tiles[p.r, p.c];
            if (t == null) continue;

            if (t.skillTest != null)
            {
                t.skillTest.Excute();
            }

            _tiles[p.r, p.c] = null;
        }
    }

    /// <summary>
    ///  중력 적용 함수
    /// 각 열을 탐색하여 비어있는 곳 있으면 위에서 당김, 
    /// </summary>
    private void ApplyGravity()
    {
        for (int col = 0; col < _columns; col++)
        {

            int writeRow = _rows - 1;

            for (int readRow = _rows - 1; readRow >= 0; readRow--)
            {
                TileTest tile = _tiles[readRow, col];

                if (tile != null)
                {
                    if (writeRow != readRow)
                    {
                        _tiles[writeRow, col] = tile;
                        _tiles[readRow, col] = null;
                    }

                    writeRow--;
                }
            }

        }
    }
}
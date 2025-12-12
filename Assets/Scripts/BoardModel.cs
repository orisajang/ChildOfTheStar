using UnityEngine;

public class BoardModel
{
    private int _rows = 5;
    public int Rows => _rows;

    private int _columns = 6;
    public int Columns => _columns;

    private int[,] _tiles;

    public void Init()
    {
        _tiles = new int[Rows, Columns];
    }

    public enum TileMoveDirection
    {
        Horizontal, // 가로 라인 이동 (row 한 줄을 좌 / 우로 이동)
        Vertical    // 세로 라인 이동 (col 한 줄을 상 / 하로 이동)
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

            int[] tempRow = new int[_columns];

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

            int[] tempCol = new int[_rows];

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
    }
}
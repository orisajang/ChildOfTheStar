using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileMoveDirection
{
    Horizontal, // 가로 라인 이동 (row 한 줄을 좌 / 우로 이동)
    Vertical,    // 세로 라인 이동 (col 한 줄을 상 / 하로 이동)
    _Null        // 아무것도 아닐 때
}

public class BoardModel
{


    private struct Pos
    {
        public int row;
        public int col;

        public Pos(int r, int c)
        {
            this.row = r;
            this.col = c;
        }
    }

    private int _rows = 5;
    public int Rows => _rows;

    private int _columns = 6;
    public int Columns => _columns;

    private Tile[,] _tiles;

    public Tile[,] Tiles => _tiles;
    public Func<int, int, Tile> CreateTile;//모델에서 좌표값 보내주면 컨트롤러에서 타일생성해서 Tile 반환
    public Action<Tile> ReturnTile;
    public Func<IEnumerator, Coroutine> StartCoroutineCallback;
    public Action OnBoardChanged;
    public Action OnResolveFinished;
    public Action OnResolveStart;
    public BoardModel()
    {
        _tiles = new Tile[Rows, Columns];
    }


    /// <summary>
    /// 해당 좌표에 타일 넣는 함수, 컨트롤러에서 타일 생성해서 넣어줘야한다고 생각함.
    /// </summary>
    /// <param name="row">0~4</param>
    /// <param name="col">0~5</param>
    /// <param name="tile"></param>
    public void SetTile(int row, int col, Tile tile)
    {
        if (row < 0 || row >= _rows || col < 0 || col >= _columns) return;

        _tiles[row, col] = tile;
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
    /// 이동할 칸 수. 
    /// + : 우 / 위
    /// - : 좌 / 아래
    /// </param>
    public void MoveTile(TileMoveDirection direction, int lineIndex, int moveAmount)
    {
        if (moveAmount == 0) return;

        if (direction == TileMoveDirection.Horizontal)
        {
            if (lineIndex < 0 || lineIndex >= _rows) return;

            Tile[] tempRow = new Tile[_columns];

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

            Tile[] tempCol = new Tile[_rows];

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
            StartCoroutineCallback(MatchChainCoroutine());
        }
    }

    private IEnumerator MatchChainCoroutine()
    {
        OnResolveStart?.Invoke();
        int loopSafety = 0;
        while (loopSafety < 20)
        {
            yield return new WaitForSeconds(0.2f);
            ApplyGravity();
            OnBoardChanged?.Invoke();
            yield return new WaitForSeconds(0.75f);

            RefillEmptyTile();
            OnBoardChanged?.Invoke();
            yield return new WaitForSeconds(0.75f);

            HashSet<Pos> newMatches = GetAllMatch();
            if (newMatches.Count == 0) break;

            ExplodeMatched(newMatches);
            OnBoardChanged?.Invoke();

            yield return new WaitForSeconds(0.55f);
            loopSafety++;
        }
        OnResolveFinished?.Invoke();
    }
    /// <summary>
    /// 빈 타일이 있다면 타일주머니에서 타일 하나꺼내서 채워준다
    /// </summary>
    private void RefillEmptyTile()
    {
        if (CreateTile == null) return;

        for (int col = 0; col < _columns; col++)
        {
            for (int row = 0; row < _rows; row++)
            {
                if (_tiles[row, col] == null)
                {
                    _tiles[row, col] = CreateTile(row, col);
                }
            }
        }
    }
    private HashSet<Pos> GetAllMatch()
    {
        HashSet<Pos> allMatches = new HashSet<Pos>();

        for (int row = 0; row < _rows; row++)
        {
            MatchTile(TileMoveDirection.Horizontal, row, allMatches);
        }
        for (int col = 0; col < _columns; col++)
        {
            MatchTile(TileMoveDirection.Vertical, col, allMatches);
        }

        return allMatches;
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

            Tile prevTile = _tiles[lineIndex, 0];

            for (int col = 1; col <= _columns; col++)
            {
                Tile currentTile = null;

                if (col < _columns)
                {
                    currentTile = _tiles[lineIndex, col];
                }

                bool isSame = false;
                if (prevTile != null && currentTile != null)
                {
                    if (prevTile.Color == currentTile.Color)
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
            Tile prevTile = _tiles[0, lineIndex];

            for (int row = 1; row <= _rows; row++)
            {
                Tile currentTile = null;
                if (row < _rows)
                {
                    currentTile = _tiles[row, lineIndex];
                }

                bool isSame = false;
                if (prevTile != null && currentTile != null)
                {
                    if (prevTile.Color == currentTile.Color)
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

        foreach (Pos position in matched)
        {
            Tile tile = _tiles[position.row, position.col];
            if (tile == null) continue;

            if (tile != null)
            {

                tile.ExecuteTile(Tiles);
                ReturnTile(tile);
                _tiles[position.row, position.col] = null;

                //자원 주머니에 터진 타일들 색상을 하나씩 넣어주기 위해서 추가
                ColorResourceManager.Instance.AddColorResource(tile.Color, 1);
            }

        }
    }

    /// <summary>
    ///  중력 적용 함수
    /// 각 열을 탐색하여 빈공간을 위에서 당겨서 채움, 
    /// </summary>
    private void ApplyGravity()
    {
        for (int col = 0; col < _columns; col++)
        {

            int writeRow = _rows - 1;

            for (int readRow = _rows - 1; readRow >= 0; readRow--)
            {
                Tile tile = _tiles[readRow, col];

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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public enum TileKeyword
{
    Rampage,    // 폭주 (4매치 이상)
    Wave,       // 파도 (이번 턴 같은색 연속 매치)
    Link,       // 연동 (2콤보 이상)
    Harmony,    // 조화 (해당 색 첫 매치)
    Crack        // 균열 (가장자리 매치)
}
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

    private List<Tile> matchedTiles = new List<Tile>(30);
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
    //매치중 한번이라도 터진 색들
    HashSet<TileColor> _MatchedColorHash = new HashSet<TileColor>();

    //과충전 관련 필드들
    //현재 터질 타일의 색상과 갯수들
    Dictionary<TileColor, int> _currentColorOverChargeDic = new Dictionary<TileColor, int>();
    //이전 매치때 터진 색상들이 무엇이었는지
    HashSet<TileColor> _beforeColorOverChargeHash = new HashSet<TileColor>();
    //터진 타일 인덱스 기록
    private List<Pos> _brokenTileIndex;
    //과충전 체크 해야하는지 여부
    bool _isOverChargeCheck;
    //과충전 상태인지 체크
    bool _isOverCharge;
    // 연속 매치 횟수
    int _loopMatchCount = 0;
    //과충전 게이지 (20되면 과충전상태 되도록 설정예정)
    int _overChargeValue = 0;
    public int OverChargeValue => _overChargeValue;
    //과충전 기준값(변동가능)
    int _limitOverChargeValue = 20;
    //타일 이동이 전부 끝났을때, Controller에 알려준다 (사용이유: 플레이어 턴이 끝났고 몬스터 턴인데 타일이 계속 터지고 있거나 판정하고있으면 안되서. 턴관리를 위해서)
    public event Action OnTileMoveEnd;

    private BoardViewer _boardViewer;

    public BoardModel()
    {
        _tiles = new Tile[Rows, Columns];
        _brokenTileIndex = new List<Pos>();
    }

    public void SetBoardViewer(BoardViewer bv)
    {
        _boardViewer = bv;
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
        //보드 비활성화 명령. moveAmount가 0이 아닐때만 비활성화 해야함
        OnResolveStart?.Invoke();
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
                _tiles[lineIndex, col].SetTIlePos(lineIndex, col);
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
                _tiles[row, lineIndex].SetTIlePos(row, lineIndex);
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

        _MatchedColorHash.Clear();
        //과충전 관련 체크 항목 초기화
        InitOverCharge(false);

        //블록 3매치 판정 시작
        ExplodeMatched(matched);

        if (matched.Count > 0)
        {
            StartCoroutineCallback(MatchChainCoroutine());
        }
        else
        {
            //타일 이동이 끝난다는 것을 알려야되는 구조로 변경되어 else문 추가
            OnTileMoveEnd?.Invoke();
        }
    }


    /// <summary>
    /// 초기(타일이동)에 초기화할 과충전 관련 체크항목들을 모아둔 메소드
    /// 타일 이동할때마다 과충전상태 해제, 및 플레이어 턴이 시작될때 과중전상태 해제해야함
    /// </summary>
    /// <param name="isplayerInit">플레이어 턴이 시작될때만 전용으로 지울값</param>
    public void InitOverCharge(bool isplayerInit)
    {
        //처음 1회는 과충전 체크 안함
        _isOverChargeCheck = false;
        //플레이어 턴이 시작될때만 과충전값 초기화
        if(isplayerInit)
        {
            _overChargeValue = 0;
        }
        //이전턴에 과충전상태였다면 다시 이동했을경우 풀기
        if (_isOverCharge)
        {
            _isOverCharge = false;
            Debug.LogWarning("과충전 해제");
        }
    }

    private IEnumerator MatchChainCoroutine()
    {
        //과충전 기능 추가
        //딕셔너리로 이전에 터진 색상들을 기억한다
        //초기1회 이후에 과충전 체크를 한다. (bool값 체크)
        _isOverChargeCheck = true;
        _loopMatchCount = 0;

        int loopSafety = 0;
        while (loopSafety < 20)
        {
            yield return new WaitForSeconds(0.20f);

            yield return ApplyGravity();
            OnBoardChanged?.Invoke();

            yield return RefillEmptyTile();
            OnBoardChanged?.Invoke();

            HashSet<Pos> newMatches = GetAllMatch();
            if (newMatches.Count == 0) break;

            //과충전 관련 매치횟수 체크하는 필드(_loopMatchCount)
            _loopMatchCount = loopSafety + 1;
            //과충전 상태가 아니면 계속 매치 판별
            if(!_isOverCharge)
            {
                ExplodeMatched(newMatches);
                //OnBoardChanged?.Invoke();
            }
            else 
            {
                //과충전 상태면 바로 break;
                break;
            }
            yield return new WaitForSeconds(0.55f);
            loopSafety++;
        }
        OnResolveFinished?.Invoke();
		Debug.LogWarning("타일이동 종료 이벤트 발송");
        OnTileMoveEnd?.Invoke();
    }
    /// <summary>
    /// 빈 타일이 있다면 타일주머니에서 타일 하나꺼내서 채워준다
    /// </summary>
    private Coroutine RefillEmptyTile()
    {
        if (CreateTile == null) return null;

        for (int col = 0; col < _columns; col++)
        {
            for (int row = 0; row < _rows; row++)
            {
                if (_tiles[row, col] == null)
                {
                    _tiles[row, col] = CreateTile(row, col);
                    _boardViewer.InitDropTile(row,col, row);
                }
            }
        }
        return _boardViewer.StartCheckDropComplate();
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
                        bool isCrack = false;
                        if (lineIndex == 0 || lineIndex == _rows - 1|| (col - 1) == _columns - 1|| (col - count) == 0)
                        {
                            isCrack = true;
                        }
                        for (int i = 1; i <= count; i++)
                        {
                            int targetCol = col - i;

                            matched.Add(new Pos(lineIndex, targetCol));
                            if (count >= 4)
                            {
                                _tiles[lineIndex, targetCol].AddKeyword(TileKeyword.Rampage);
                            }
                            if (isCrack)
                            {
                                _tiles[lineIndex, targetCol].AddKeyword(TileKeyword.Crack);
                            }
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
                        bool isCrack = false;

                        if (lineIndex == 0 || lineIndex == _columns - 1 || (row - 1) == _rows - 1 || (row - count) == 0)
                        {
                            isCrack = true;
                        }
                        for (int i = 1; i <= count; i++)
                        {
                            int targetRow = row - i;
                            matched.Add(new Pos(targetRow, lineIndex));
                            if (count >=4)
                            {
                                _tiles[targetRow, lineIndex].AddKeyword(TileKeyword.Rampage);
                            }
                            if (isCrack)
                            {
                                _tiles[targetRow, lineIndex].AddKeyword(TileKeyword.Crack);
                            }
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
        matchedTiles.Clear();
        if (matched == null) return;
        if (matched.Count == 0) return;
        
        foreach (Pos position in matched)
        {
            Tile tile = _tiles[position.row, position.col];
            if (tile != null)
                matchedTiles.Add(tile);
            tile.Matched = true;

        }

        foreach (Tile tile in matchedTiles)
        {
            if (_beforeColorOverChargeHash.Contains(tile.Color))
                tile.AddKeyword(TileKeyword.Wave);
            if (_loopMatchCount > 0)
                tile.AddKeyword(TileKeyword.Link);
            if (!_MatchedColorHash.Contains(tile.Color))
                tile.AddKeyword(TileKeyword.Harmony);
        }

        matchedTiles.Sort(CompareTilePriority);


        foreach (Tile tile in matchedTiles)
        {
            tile.ExecutePreSkill(Tiles);
        }
        foreach (Tile tile in matchedTiles)
        {
            tile.ExecuteStatus(Tiles);
        }

        foreach (Tile tile in matchedTiles)
        {
            tile.ExecuteTile(Tiles);
            _brokenTileIndex.Add(new Pos(tile.Row, tile.Col));
            _tiles[tile.Row, tile.Col] = null;
            ReturnTile(tile);

            //자원 주머니에 터진 타일들 색상을 하나씩 넣어주기 위해서 추가
            ColorResourceManager.Instance.AddColorResource(tile.Color, 1);
            //과충전 체크할때 색상별 타일이 얼마만큼 터졌는지 확인 필요
            //foreach문에서는 타일이 하나씩 터지기때문에 딕셔너리로 터진 타일들 몇개인지 묶음
            SetColorOverChargeInfo(tile.Color);
            _MatchedColorHash.Add(tile.Color);

            //과충전 증감 연산 체크
            CalcOverChargeValue();
        }
       
    }
    /// <summary>
    /// 과충전 체크할때 색상별 타일이 얼마만큼 터졌는지 확인 필요
    /// </summary>
    private void SetColorOverChargeInfo(TileColor color)
    {
        
        if (!_currentColorOverChargeDic.ContainsKey(color))
        {
            _currentColorOverChargeDic[color] = 1;
        }
        else
        {
            _currentColorOverChargeDic[color] += 1;
        }
    }
    /// <summary>
    /// 과충전 게이지 증감을 체크하는 메서드
    /// </summary>
    private void CalcOverChargeValue()
    {
        int totalOverChargeValue = 0;
        //디버그용. 계산1, 계산2가 각각 어떤값이 나왔는지
        int calc1 = 0;
        int calc2 = 0;

        //과충전 체크
        if (_isOverChargeCheck)
        {
            int sameColor = 0;
            int differentColor = 0;
            //과충전 체크1.타일마다 따로따로 하는 체크
            foreach (TileColor color in _currentColorOverChargeDic.Keys)
            {
                //해당 타일 갯수
                int amount = _currentColorOverChargeDic[color];

                //_beforeColorOverChargeHash 에 존재하는지 확인
                if (_beforeColorOverChargeHash.Contains(color))
                {
                    //존재하면 과부하 게이지 상승
                    //계산식: 그냥 같은 색상 타일 전부다 더한값 * (연속 매치횟수*2) 해버리면 됨
                    totalOverChargeValue += amount * (_loopMatchCount * 2);
                    sameColor++;
                }
                else
                {
                    //존재하지 않으면 과부하 게이지 하락
                    //계산식: 다른 색상 타일 갯수 * (연속매치횟수)
                    totalOverChargeValue -= amount * _loopMatchCount;
                    differentColor++;
                }
            }
            calc1 = totalOverChargeValue;
            //과충전 체크2. 타일 매치판정 2개이상인경우 파악해서 처리
            //매치 판정이 2개 이상으로 되었는지 체크
            int colorSum = sameColor + differentColor;
            if (colorSum >= 2)
            {
                //동일색 > 다른색 : 게이지 +1씩
                if (sameColor > differentColor)
                {
                    //그냥 매치 레벨 을 더해주면된다
                    totalOverChargeValue += _loopMatchCount;
                }
                else if (sameColor < differentColor)
                {
                    totalOverChargeValue -= _loopMatchCount;
                }
                else if (sameColor == differentColor)
                {
                    //아무것도 안함
                }
            }
            //계산값을 현재 과충전 게이지에 넣어준다
            SetOverChargeValue(totalOverChargeValue);
            calc2 = totalOverChargeValue - calc1;
            Debug.LogWarning($"현재 과충전 게이지: {_overChargeValue},체크1값:{calc1} 체크2값:{calc2} 계산했던 과충전게이지 {totalOverChargeValue} 현재 과충전 턴{_loopMatchCount}");
            UIManager.Instance.OverchargeUI.UpdateOverCharge(_overChargeValue);
        }

        //과충전 체크 끝. _beforeColorOverChargeHash를 설정한다. -> _currentColorOverChargeDic의 색깔이 있는거대로
        _beforeColorOverChargeHash.Clear();
        foreach (TileColor color in _currentColorOverChargeDic.Keys)
        {
            _beforeColorOverChargeHash.Add(color);
        }
        //다 쓰고나서 초기화
        _currentColorOverChargeDic.Clear();
    }

    private void SetOverChargeValue(int amount)
    {
        _overChargeValue += amount;
        if(_overChargeValue < 0)
        {
            _overChargeValue = 0;
        }
        else if(_overChargeValue >= _limitOverChargeValue)
        {
            //과충전이라는 것을 알리는 기능을 추가하자
            _isOverCharge = true;
            Debug.LogWarning("과충전 상태 진입!");
        }
    }

    /// <summary>
    ///  중력 적용 함수
    /// 각 열을 탐색하여 빈공간을 위에서 당겨서 채움, 
    /// </summary>
    private Coroutine ApplyGravity()
    {
        int dropIndex;
        for (int col = 0; col < _columns; col++)
        {
            dropIndex = 0;
            for (int row = _rows - 1; row >= 0; row--)
            {
                if (_tiles[row, col] == null)
                {
                    dropIndex++;
                }
                else if(dropIndex != 0)
                {
                    if(_boardViewer == null)
                    {
                        Debug.LogError("Board Model에 Board Viewer 가 없습니다.");
                        return null;
                    }
                    _boardViewer.DropTile(row, col, dropIndex);
                }
            }
        }

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
                        tile.SetTIlePos(writeRow, col);
                        _tiles[readRow, col] = null;
                    }
                    writeRow--;
                }
            }
        }
        return _boardViewer.StartCheckDropComplate();
    }

    private int GetColorPriority(TileColor color)
    {
        switch (color)
        {
            case TileColor.Red: return 0;
            case TileColor.Blue: return 1;
            case TileColor.Green: return 2;
            case TileColor.White: return 3;
            case TileColor.Black: return 4;
            default: return 99;
        }
    }

    private int CompareTilePriority(Tile a, Tile b)
    {
        int speedA = a.GetSpeed();
        int speedB = b.GetSpeed();

        if (speedA != speedB)
        {
            return speedA.CompareTo(speedB);
        }

        int colorPriorityA = GetColorPriority(a.Color);
        int colorPriorityB = GetColorPriority(b.Color);

        if (colorPriorityA != colorPriorityB)
        {
            return colorPriorityA.CompareTo(colorPriorityB);
        }

        return a.TileData.Id.CompareTo(a.TileData.Id);
    }

}
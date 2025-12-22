using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class BoardController : MonoBehaviour
{
	//보드에서 타일이동이 끝나면 바로 막아야 해서 추가.이유: 블록이 떨어지고 다시 재생성 되는것을 보여주기 위해 코루틴으로 처리하고있음. 턴이 끝났다는것을 턴매니저에 알리기전에 타일 이동이 가능해져서 추가
	[SerializeField] private BoardBlock boardBlock;
	//타일관련
    [SerializeField] TileBoardSizeSO _boardSize;
    [SerializeField] BoardViewer _boardViewer;
    private BoardModel _boardModel = new BoardModel();
    public BoardModel BoardModel => _boardModel;

    //타일 기준 포지션
    private Vector2[,] _tilePoints;

    //타일 클릭 인덱스
    private int startIndexRow;
    private int startIndexCol;
    private int fixedIndex;

    //타일간 갭
    private float _tileGapX;
    private float _tileGapY;

    private Vector2 _startPos;
    private Vector2 _oldMousePosition;
    private Vector2 _deltaMousePosition;
    private Vector2 _totalMoveMousePosition;

    //타일 이동 방향
    private TileMoveDirection _curTileMoveDir;
    private void Awake()
    {
        _curTileMoveDir = TileMoveDirection._Null;
    }
    private void Start()
    {
        _tilePoints = new Vector2[_boardSize.ySize, _boardSize.xSize];


        Transform child;

        string indexPattern = @"(\d+)_(\d+)";
        Match indexMatch;

        int indexCol;
        int indexRow;

        //자식 중 point 인 객체를 가져와 vector 배열에 위치를 기록
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            if (!child.name.Contains("point")) continue;
            indexMatch = Regex.Match(child.name, indexPattern);

            if (!indexMatch.Success) continue;
            if (indexMatch.Groups.Count != 3) continue;

            if (!int.TryParse(indexMatch.Groups[1].Value, out indexRow))
            {
                continue;
            }
            if (!int.TryParse(indexMatch.Groups[2].Value, out indexCol))
            {
                continue;
            }

            _tilePoints[indexRow - 1, indexCol - 1] = child.transform.position;

        }
        //갭 측정
        _tileGapX = (_tilePoints[0, 1] - _tilePoints[0, 0]).x;
        _tileGapY = (_tilePoints[0, 0] - _tilePoints[1, 0]).y;

        //뷰어 init
        _boardViewer.InitTilePoints(_tilePoints);
        _boardViewer.InitTileObject(_boardModel);
    }
    private void OnEnable()
    {
        _boardModel.OnBoardChanged += UpdateBoardView;
        _boardModel.StartCoroutineCallback += StartCoroutine;
        //턴관리를 위해서 이벤트 추가
        TurnManager.Instance.OnPlayerTurnStart += StartPlayerTurn;
        _boardModel.OnTileMoveEnd += DecreasePlayerMovePoint;
        //보드 비활성화를 위해 이벤트 추가
        _boardModel.OnResolveFinished += OnBoardResolveFinished;
        _boardModel.OnResolveStart += OnBoardResolveStart;
    }
    private void OnDisable()
    {
        _boardModel.OnBoardChanged -= UpdateBoardView;
        _boardModel.StartCoroutineCallback -= StartCoroutine;
        TurnManager.Instance.OnPlayerTurnStart -= StartPlayerTurn;
        _boardModel.OnTileMoveEnd -= DecreasePlayerMovePoint;
        _boardModel.OnResolveStart -= OnBoardResolveStart;
        _boardModel.OnResolveFinished -= OnBoardResolveFinished;
    }

    /// <summary>
    /// 타일 이동이 코루틴으로 되어있기때문에 모델에서 이벤트를 받아야함. 전부 끝난뒤에 플레이어 턴감소
    /// </summary>
    private void DecreasePlayerMovePoint()
    {
        //플레이어 1회 행동 처리 (1칸이라도 이동했을때)
        Debug.LogWarning("플레이어 행동1회끝");
        TurnManager.Instance.OnPlayerTurnEndOnce();
    }

    private void StartPlayerTurn()
    {
        _boardModel.InitOverCharge(true);
    }
        private void OnBoardResolveFinished()
    {
        boardBlock.SetBoardActive(false);
    }
    private void OnBoardResolveStart()
    {
        boardBlock.SetBoardActive(true);
    }
    private void UpdateBoardView()
    {
        _boardViewer.InitTileObject(_boardModel);
    }
    //임시코드
    public void SetTile(int row, int col, Tile tile)
    {
        _boardModel.SetTile(row, col, tile);
    }

    public void UpdateClickEvent(Vector2 clickPos, bool isClicked)
    {
        if (isClicked)
        {
            var indexs = GetAdjacentIndex(clickPos);
            startIndexRow = indexs[0];
            startIndexCol = indexs[1];
            _startPos = clickPos;
            _oldMousePosition = clickPos;
            _totalMoveMousePosition = Vector2.zero;
        }
        else
        {
            int moveAmount = 0;
            if (_curTileMoveDir == TileMoveDirection.Horizontal)
            {
                moveAmount = (int)(_totalMoveMousePosition.x / _tileGapX);
                if (Mathf.Abs(_totalMoveMousePosition.x % _tileGapX) > _tileGapX / 2)
                {
                    moveAmount = (_totalMoveMousePosition.x > 0) ? moveAmount + 1 : moveAmount - 1;
                }
                _boardModel.MoveTile(_curTileMoveDir, fixedIndex, moveAmount);
            }
            else if (_curTileMoveDir == TileMoveDirection.Vertical)
            {
                moveAmount = (int)(_totalMoveMousePosition.y / _tileGapY);
                if (Mathf.Abs(_totalMoveMousePosition.y % _tileGapY) > _tileGapY / 2)
                {
                    moveAmount = (_totalMoveMousePosition.y > 0) ? moveAmount + 1 : moveAmount - 1;
                }
                moveAmount = -moveAmount;
                _boardModel.MoveTile(_curTileMoveDir, fixedIndex, moveAmount);
            }
            _boardViewer.InitTileObject(_boardModel);
            //클릭 해제 시 방향도 해제
            _curTileMoveDir = TileMoveDirection._Null;
        }
    }
    public void UpdateMousePosition(Vector2 mousePos)
    {
        CheckMoveDirection(mousePos);
        if (_curTileMoveDir != TileMoveDirection._Null)
        {
            //방향에 따른 백터 값 필터링
            if (_curTileMoveDir == TileMoveDirection.Horizontal)
            {
                _deltaMousePosition = new Vector2(mousePos.x - _oldMousePosition.x, 0);
            }
            else if (_curTileMoveDir == TileMoveDirection.Vertical)
            {
                _deltaMousePosition = new Vector2(0, mousePos.y - _oldMousePosition.y);
            }


            //viewer 에 전달해서 tile을 움직 이도록 수정
            _boardViewer.MoveTilesPosition(_curTileMoveDir, _deltaMousePosition, fixedIndex);

            //이동 거리 누적
            _totalMoveMousePosition += _deltaMousePosition;
        }


        _oldMousePosition = mousePos;
    }

    private void CheckMoveDirection(Vector2 mousePos)
    {
        //드레그 시작 부근이라면 움직일 방향을 결정함
        if (_curTileMoveDir == TileMoveDirection._Null)
        {
            //너무 작은 값은 무시함
            if (Vector2.Distance(mousePos, _startPos) < 0.2) return;
            var dirVactor = mousePos - _startPos;

            if (Mathf.Abs(dirVactor.x) > (Mathf.Abs(dirVactor.y)))
            {
                _curTileMoveDir = TileMoveDirection.Horizontal;
                fixedIndex = startIndexRow;
            }
            else
            {
                _curTileMoveDir = TileMoveDirection.Vertical;
                fixedIndex = startIndexCol;
            }
        }
    }
    /// <summary>
    /// 입력 받은 position 에서 가장 가까운 타일 인덱스 가져오기
    /// </summary>
    /// <param name="position"></param>
    /// <returns>{row, col} 못찾으면 -1 로 채워짐</returns>
    private int[] GetAdjacentIndex(Vector2 position)
    {
        int row = -1;
        int col = -1;
        float closestDist = float.MaxValue;
        float curDist;

        for (int i = 0; i < _boardSize.ySize; i++)
        {
            for (int j = 0; j < _boardSize.xSize; j++)
            {
                curDist = Vector2.Distance(_tilePoints[i, j], position);
                if (closestDist > curDist)
                {
                    row = i;
                    col = j;
                    closestDist = curDist;
                }
            }
        }

        return new int[] { row, col };
    }
}
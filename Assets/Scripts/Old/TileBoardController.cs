#if IS_OLD
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static BoardModel;

public class TileBoardController : MonoBehaviour
{


    //1.View에서 가로로 드래그했다 입력 받기
    //2.Model에게 이동 요청
    //3.Model의 이동이 정상적으로 되었다면?
    //- 성공하면 View에게 어떻게 변하라고 전달 (이동만 해라, 매치 발생하면? Model에서 변한 배열을 View에 넘겨줘서 View에서 처리하게?? 
    //- 실패하면 View에게 변하지말라고 전달
    BoardModel tileBoardModel = new BoardModel();
    TileBoardView tileBoardView;

    //입력을 위해서 정의
    //화면클릭, 클릭해제를 알기위해서
    InputAction _uiAction;
    //클릭후 드래그중인지 알기위해서 사용
    InputAction _pointAction;

    // 방향 판단 임계값 이 값 이상 움직여여야지 방향 결정 수치 변경 예정----------------------------------------------------------
    [SerializeField] private float positionThreshold = 1f;

    [SerializeField] CreateTilePoint tilePoint;

    //이벤트
    //이동을 얼마나 해야하는지 
    public event Action<TileMoveDirection, int, int> OnMoveTile;

    //시작 관련
    bool _isDragging;
    Vector2 startPos;
    // 상하 또는 좌우 고정 여부
    private bool _isMoveFix;
    private TileMoveDirection _moveDirection;
    //// 상하
    //private bool _isUpDown;
    //// 좌우
    //private bool _isLeftRight;

    // 인덱스 저장용 스트럭트
    private struct index
    { public int x, y; };

    index startIndex;

    private void Awake()
    {
        //view를 어딘가에서 가져왔다고 가정
        tileBoardView = GetComponent<TileBoardView>();
        //tileBoardModel.Init();
        //InputSystem 입력값 액션
        _uiAction = InputSystem.actions.FindAction("UI/Click");
        _pointAction = InputSystem.actions.FindAction("UI/Point");
    }
    private void OnEnable()
    {
        //추후 추가예정(view에서 타일 이동이 끝났을떄?)
        //tileBoardView.OnTileMoved += OnTileMoveTriggered;
        _uiAction.started += OnMouseClickStart;
        _uiAction.canceled += OnMouseClickEnd;



    }

#if TEST
    public void InitTiles(int x, int y, Tile tile)
    {
        tileBoardModel.SetTile(x, y, tile);
    }
#endif
    private void OnDisable()
    {
        //tileBoardView.OnTileMoved -= OnTileMoveTriggered;
        _uiAction.started -= OnMouseClickStart;
        _uiAction.canceled -= OnMouseClickEnd;


    }
    private void Update()
    {
        //업데이트 수정 필요할듯? -> 클릭할때마다 View에서 보이도록, model에 데이터 전송할 수있도록?
        if (_isDragging)
        {
            Vector2 screenPos = _pointAction.ReadValue<Vector2>();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            tileBoardView.UpdateDrag(worldPos);
            //Debug.Log("클릭후 드래그중 " + screenPos);


            // 이동 거리 계산
            Vector2 changePosition = worldPos - startPos;

            if (_isMoveFix == false)
            {
                // 임계값을 넘었을 때만 방향 결정
                if (Mathf.Abs(changePosition.x) > positionThreshold || Mathf.Abs(changePosition.y) > positionThreshold)
                {
                    // 좌우 이동이 더 클 때
                    if (Mathf.Abs(changePosition.x) > Mathf.Abs(changePosition.y))
                    {
                        //_isLeftRight = true;
                        //_isUpDown = false;
                        _moveDirection = TileMoveDirection.Horizontal;
                    }
                    // 상하 이동이 더 클 때
                    else if (Mathf.Abs(changePosition.x) < Mathf.Abs(changePosition.y))
                    {
                        //_isUpDown = true;
                        //_isLeftRight = false;
                        _moveDirection = TileMoveDirection.Vertical;
                    }
                    _isMoveFix = true;
                }
            }
            // 좌우 이동이 더 크니 상하 이동값 무시
            //if (_isLeftRight == true)
            //{
            //    //return new Vector2(changePosition.x, 0);
            //}
            //// 상하 이동이 더 크니 좌우 이동값 무시
            //if (_isUpDown == true)
            //{
            //    //return new Vector2(0, changePosition.y);
            //}
        }
    }
    /// <summary>
    /// VIew에서 정보를 받아서 model에 전달 
    /// </summary>
    /// <param name="direction">가로,세로 방향중 어디로 이동중인지</param>
    /// <param name="currentLine">현재 몇번째 가로/세로 줄을 선택중인지 </param>
    /// <param name="moveValue">얼마나 이동시켜야하는지 </param>
    private void OnTileMoveTriggered(TileMoveDirection direction, int currentLine, int moveValue)
    {
        //modelTest.MoveTile(pos, moveValue);
        OnMoveTile?.Invoke(direction, currentLine, moveValue);
    }
    
    private void OnMouseClickStart(InputAction.CallbackContext ctx)
    {
        // 상태 초기화
        _isMoveFix = false;
        //_isUpDown = false;
        //_isLeftRight = false;
        _moveDirection = TileMoveDirection._Null;

        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        //Debug.Log("클릭시작 " + screenPos);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        //시작한 좌표 저장
        startPos = worldPos;

        //1. 클릭한 영역이 타일영역인지 확인
        //타일 바깥을 클릭한 경우 아무것도 하지않음 (타일의 왼쪽, 오른쪽 위, 아래쪽 체크)
        if (worldPos.x < tilePoint.standardTilePositionStart.x)
        {
            return;
        }
        if (worldPos.x > tilePoint.standardTilePositionEnd.x)
        {
            return;
        }
        if (worldPos.y > tilePoint.standardTilePositionStart.y)
        {
            return;
        }
        if (worldPos.y < tilePoint.standardTilePositionEnd.y)
        {
            return;
        }

        //Debug.Log("성공");
        _isDragging = true;

        //스탠다드 포지션과, 타일갭x, 타일갭y를 통해 처리
        //tilePoint.standardPosition;
        //tilePoint._tileGapX;
        //tilePoint._tileGapY;
        //둘의 값을 뺀다, tileGapx만큼 나눈다. 그럼 몇번째 X인지 체크 가능
        //tileBoard에서 그냥 좌표를 가지고있게 하자 (tempPointPosition)
        int xPos = (int)((worldPos.x - tilePoint.standardTilePositionStart.x) / tilePoint._tileGapX);
        int yPos = (int)((tilePoint.standardTilePositionStart.y - worldPos.y) / tilePoint._tileGapY);
        //선택된 타일의 좌표를 보낸다
        Vector2 selectedPos = tilePoint.tempPointPosition[xPos, yPos];
        tileBoardView.Drag(selectedPos);
        startIndex.x = xPos;
        startIndex.y = yPos;

        Debug.Log($"클릭시작. 좌표= {xPos}, {yPos}");
    }
    private void OnMouseClickEnd(InputAction.CallbackContext ctx)
    {
        //드래깅중일때만 End 동작
        if (!_isDragging) return;
        //입력 좌표를 가져옴
        _isDragging = false;
        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        //Debug.Log("클릭끝 " + screenPos);

        //마우스를 뗀 좌표가 좌표의 어느 지점인지를 보내주면 된다.
        //상하좌우 지점의 좌표를 벗어낫을때는 Gap값을 이용해서 좌표를 변환해준다

        //정상작동인지 체크위해 isFixed 선언
        bool isFixed = false;
        //좌표에서 X축과 Y축을 보내기 위해 선언
        int xPos = 0;
        int yPos = 0;
        //타일의 x축길이, y축 길이
        int xLength = tilePoint.tempPointPosition.GetLength(0);
        int yLength = tilePoint.tempPointPosition.GetLength(1);
        //예외처리 (if문 4개) 타일 바깥에서 마우스 해제한경우
        //x왼쪽 방향
        if (worldPos.x < tilePoint.standardTilePositionStart.x)
        {
            isFixed = true;
            float k = tilePoint.standardTilePositionStart.x - worldPos.x;
            //왼쪽으로 몇번이나 초과해서 이동했는지
            int a1 = (int)(k / tilePoint._tileGapX);
            //x축가로 최대 6개이므로 0~5까지 범위로만 되도록 나머지 연산
            int a2 = a1 % xLength;
            xPos = (xLength - 1) - a2;
        }
        if (worldPos.x > tilePoint.standardTilePositionEnd.x)
        {
            isFixed = true;
            float k = worldPos.x - tilePoint.standardTilePositionEnd.x;
            //왼쪽으로 몇번이나 초과해서 이동했는지
            int a1 = ((int)(k / tilePoint._tileGapX));
            //x축가로 최대 6개이므로 0~5까지 범위로만 되도록 나머지 연산
            int a2 = a1 % xLength;
            xPos = a2;
        }
        if (worldPos.y > tilePoint.standardTilePositionStart.y)
        {
            isFixed = true;
            float k = worldPos.y - tilePoint.standardTilePositionStart.y;
            //왼쪽으로 몇번이나 초과해서 이동했는지
            int a1 = (int)(k / tilePoint._tileGapY);
            //y축세로 최대 5개이므로 0~4까지 범위로만 되도록 나머지 연산
            int a2 = a1 % yLength;
            yPos = (yLength - 1) - a2;
        }
        if (worldPos.y < tilePoint.standardTilePositionEnd.y)
        {
            isFixed = true;
            float k = tilePoint.standardTilePositionEnd.y - worldPos.y;
            //왼쪽으로 몇번이나 초과해서 이동했는지
            int a1 = ((int)(k / tilePoint._tileGapY));
            //y축세로 최대 5개이므로 0~4까지 범위로만 되도록 나머지 연산
            int a2 = a1 % yLength;
            yPos = a2;
        }
        //보드 범위를 초과해서 마우스가 이동한 경우 반대좌표를 강제로 넣어준다
        if (isFixed)
        {
            if (_moveDirection == TileMoveDirection.Horizontal)
            {
                yPos = (int)((tilePoint.standardTilePositionStart.y - worldPos.y) / tilePoint._tileGapY);
            }
            else if (_moveDirection == TileMoveDirection.Vertical)
            {
                xPos = (int)((worldPos.x - tilePoint.standardTilePositionStart.x) / tilePoint._tileGapX);
            }
        }

        //타일 범위 안넘어가고 타일안에서 마우스 작동한 경우는 특별한 예외처리없이 그냥 현재 마우스좌표로 좌표를 계산
        if (!isFixed)
        {
            xPos = (int)((worldPos.x - tilePoint.standardTilePositionStart.x) / tilePoint._tileGapX);
            yPos = (int)((tilePoint.standardTilePositionStart.y - worldPos.y) / tilePoint._tileGapY);

        }
        //결과 출력
        Debug.Log($"클릭끝. 좌표= {xPos}, {yPos}");
        Vector2 selectedPos = tilePoint.tempPointPosition[xPos, yPos];
        //선택된 타일의 좌표를 보낸다
        tileBoardView.EndDrag(selectedPos);

        //Model 에 타일 이동 명령
        int lineIndex = 0;
        int moveIndex = 0;
        if (_moveDirection == TileMoveDirection.Horizontal)
        {
            lineIndex = startIndex.y;
            moveIndex = startIndex.x - xPos;
        }
        else if (_moveDirection == TileMoveDirection.Vertical)
        {
            lineIndex = startIndex.x;
            moveIndex = startIndex.y - yPos;
        }
        tileBoardModel.MoveTile(_moveDirection, lineIndex, moveIndex);

    }
}
#endif
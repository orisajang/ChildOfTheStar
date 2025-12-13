using UnityEngine.InputSystem;
using UnityEngine;
using System;
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
    //클릭중인지
    bool _isDragging;

    //이벤트
    //이동을 얼마나 해야하는지 
    public event Action<TileMoveDirection, int, int> OnMoveTile;

    private void Awake()
    {
        //view를 어딘가에서 가져왔다고 가정
        tileBoardView = GetComponent<TileBoardView>();
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
    private void OnDisable()
    {
        //tileBoardView.OnTileMoved -= OnTileMoveTriggered;
        _uiAction.started -= OnMouseClickStart;
        _uiAction.canceled -= OnMouseClickEnd;


    }
    private void Update()
    {
        if (_isDragging)
        {
            Vector2 screenPos = _pointAction.ReadValue<Vector2>();
            tileBoardView.UpdateDrag(screenPos);
            //Debug.Log("클릭후 드래그중 " + screenPos);
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
        _isDragging = true;
        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        tileBoardView.Drag(screenPos);
        //Debug.Log("클릭시작 " + screenPos);
    }
    private void OnMouseClickEnd(InputAction.CallbackContext ctx)
    {
        _isDragging = false;
        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        tileBoardView.EndDrag(screenPos);
        //Debug.Log("클릭끝 " + screenPos);
    }


}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerInputControll : MonoBehaviour
{
    // 보드 입력 막기 위해서 추가된 코드입니다. 문제 될시 삭제해 주세요
    [SerializeField] BoardBlock _boardBlock;
    [SerializeField] string _tilesTagName = "Tile";
    [SerializeField] BoardController _bordController;
    private InputAction _mouseMove;
    private InputAction _mouseClick;

    //move를 board 컨트롤 때만 보내기 위해 생성한 flag;
    private bool _isMouseDownForBoardMove;

    private Vector2 _mousePos;
    RaycastHit2D _hit;

    private void Awake()
    {
        _mouseMove = InputSystem.actions["ClickPos"];
        _mouseClick = InputSystem.actions["Click"];
    }

    private void OnEnable()
    {
        _mouseMove.performed += OnMovedMouse;
        _mouseClick.performed += OnClicked;
        _mouseClick.canceled += CancleClicked;
    }
    private void OnDisable()
    {
        _mouseMove.performed -= OnMovedMouse;
        _mouseClick.performed -= OnClicked;
        _mouseClick.canceled -= CancleClicked;
    }

    private void OnClicked(InputAction.CallbackContext ctx)
    {
        // 보드 입력 막기 위해서 추가된 코드입니다. 문제 될시 삭제해 주세요 아래 if문 2개
        if (_boardBlock != null && _boardBlock._IsBlocked)
            return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        if (ctx.ReadValueAsButton())
        {
            if (_mousePos == null) return;
            Vector2 mouseWorldpos = Camera.main.ScreenToWorldPoint(_mousePos);

            _hit = Physics2D.Raycast(mouseWorldpos, Vector2.zero);

            if (!_hit) return;
            if (_hit.transform.CompareTag(_tilesTagName))
            {
                _bordController.UpdateClickEvent(_hit.point, true);
                _isMouseDownForBoardMove = true;
            }
        }
    }

    private void CancleClicked(InputAction.CallbackContext ctx)
    {
        _isMouseDownForBoardMove = false;
        _bordController.UpdateClickEvent(Vector2.zero, false);
    }
    private void OnMovedMouse(InputAction.CallbackContext ctx)
    {
        _mousePos = ctx.ReadValue<Vector2>();
        //_mousePos = Camera.main.ScreenToWorldPoint(_mousePos);
        if (_isMouseDownForBoardMove)
        {
            _bordController.UpdateMousePosition(Camera.main.ScreenToWorldPoint(_mousePos));
        }
    }
}

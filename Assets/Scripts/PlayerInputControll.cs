using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerInputControll : MonoBehaviour
{
    // 보드 입력 막기 위해서 추가된 코드입니다. 문제 될시 삭제해 주세요
    [SerializeField] BoardBlock _boardBlock;
    [SerializeField] string _tilesTagName = "Tile";
    [SerializeField] BoardController _bordController;
    [SerializeField] SpriteRenderer _boardSpriteRenderer;
    [SerializeField] UITileSkileInfoDisplay _uiTileSkilInfoDisplay;
    private InputAction _mouseMove;
    private InputAction _mouseClick;

    //move를 board 컨트롤 때만 보내기 위해 생성한 flag;
    private bool _isMouseDownForBoardMove;

    private Vector2 _mousePos;
    private Vector2 _oldMousePos;
    RaycastHit2D _hit;
    RaycastHit2D _hoveringHit;

    private Transform _oldHoverHit;

    private struct BoardArea
    {
        public float minX;
        public float maxX;
        public float minY;
        public float maxY;
    }
    private BoardArea _boardArea;

    private void Awake()
    {
        _mouseMove = InputSystem.actions["ClickPos"];
        _mouseClick = InputSystem.actions["Click"];
        _boardArea.minX = _boardSpriteRenderer.transform.position.x - _boardSpriteRenderer.size.x / 2;
        _boardArea.maxX = _boardSpriteRenderer.transform.position.x + _boardSpriteRenderer.size.x / 2;
        _boardArea.minY = _boardSpriteRenderer.transform.position.y - _boardSpriteRenderer.size.y / 2;
        _boardArea.maxY = _boardSpriteRenderer.transform.position.y + _boardSpriteRenderer.size.y / 2;
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

            _hit = Physics2D.Raycast(_mousePos, Vector2.zero);

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
        _mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        //_mousePos = Camera.main.ScreenToWorldPoint(_mousePos);
        if (_isMouseDownForBoardMove)
        {
            _bordController.UpdateMousePosition(_mousePos);
        }

        CheckHoveringEvent();
    }
    private void CheckHoveringEvent()
    {
        //타일 호버링을 위해 레이케스팅을 할 건데, 리소스 낭비를 방지하기위해 
        if (_mousePos.x > _boardArea.minX &&
           _mousePos.x < _boardArea.maxX &&
           _mousePos.y > _boardArea.minY &&
           _mousePos.y < _boardArea.maxY)
        {
            //이동량이 적다면 넘김
            if (Vector2.Distance(_mousePos, _oldMousePos) < 0.2f) return;

            _hoveringHit = Physics2D.Raycast(_mousePos, Vector2.zero);

            if (!_hoveringHit) return;
            if (_oldHoverHit == _hoveringHit.transform) return;
            _oldHoverHit = _hoveringHit.transform;

            if (_hoveringHit.transform.CompareTag(_tilesTagName))
            {
                _uiTileSkilInfoDisplay.UpdateTile(_hoveringHit.transform.GetComponent<Tile>());
            }
        }
    }
}

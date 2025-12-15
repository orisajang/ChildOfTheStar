using UnityEngine;


// 1. 위치값 이동을 몇 칸을 이동했는지 추가
public class TileBoardView : MonoBehaviour
{
    // 클릭이 시작된 마우스 위치
    private Vector2 startPosition;

    // 드래그 중인지
    private bool _isDragging;

    // 상하 또는 좌우 고정 여부
    private bool _isMoveFix;

    // 보드 활성화 여부
    private bool _isBoardActive = true;

    // 상하
    private bool _isUpDown;
    // 좌우
    private bool _isLeftRight;

    // 방향 판단 임계값 이 값 이상 움직여여야지 방향 결정 수치 변경 예정----------------------------------------------------------
    [SerializeField] private float positionThreshold = 1f;

    // 시작지점 근처 확인용 나중에 수치 변경 예정----------------------------------------------------------------------------
    [SerializeField] private float returnStartPoint = 1f;

    /// <summary>
    /// 시작 마우스위치를 저장하고 드래그 상태를 변경(클릭 했을 떄)
    /// </summary>
    /// <param name="mousePosition">클릭 마우스 위치</param>
    public void Drag(Vector2 mousePosition)
    {
        // 보드 비활성화 상태시
        if (_isBoardActive == false)
        {
            return;
        }

        startPosition = mousePosition;
        _isDragging = true;

        // 상태 초기화
        _isMoveFix = false;
        _isUpDown = false;
        _isLeftRight = false;
    }
    /// <summary>
    /// 현재 마우스 위치를 받아 방향 결정(드래그 중)
    /// </summary>
    /// <param name="currentMousePosition">현재 마우스 위치</param>
    /// <returns>상하 이동값 또는 좌우 이동값</returns>
    public Vector2 UpdateDrag(Vector2 currentMousePosition)
    {
        if (_isBoardActive == false)
        {
            return Vector2.zero;
        }
        // 드래그 중이 아니라면 이동 없음
        if (_isDragging == false)
        {
            return Vector2.zero;
        }
        // 이동 거리 계산
        Vector2 changePosition = currentMousePosition - startPosition;

        if (_isMoveFix == false)
        {
            // 임계값을 넘었을 때만 방향 결정
            if (Mathf.Abs(changePosition.x) > positionThreshold || Mathf.Abs(changePosition.y) > positionThreshold)
            {
                // 좌우 이동이 더 클 때
                if (Mathf.Abs(changePosition.x) > Mathf.Abs(changePosition.y))
                {
                    _isLeftRight = true;
                    _isUpDown = false;
                    Debug.Log("좌우 이동 고정" + changePosition);
                }
                // 상하 이동이 더 클 때
                else if (Mathf.Abs(changePosition.x) < Mathf.Abs(changePosition.y))
                {
                    _isUpDown = true;
                    _isLeftRight = false;
                    Debug.Log("상하 이동 고정" + changePosition);
                }
                _isMoveFix = true;
            }
        }
        // 좌우 이동이 더 크니 상하 이동값 무시
        if (_isLeftRight == true)
        {
            return new Vector2(changePosition.x, 0);
        }
        // 상하 이동이 더 크니 좌우 이동값 무시
        if (_isUpDown == true)
        {
            return new Vector2(0, changePosition.y);
        }
        return Vector2.zero;
    }
    /// <summary>
    /// 마우스를 놓았을 때
    /// </summary>
    /// <param name="mousePosition">현재 마우스 위치</param>
    public void EndDrag(Vector2 currentMousePosition)
    {
        if (_isBoardActive == false)
        {
            return;
        }
        // 드래그중이 아니면 종료
        if (_isDragging == false)
        {
            return;
        }

        // 이동 거리 계산
        Vector2 changePosition = currentMousePosition - startPosition;

        // 이동거리가 시작 지점으로 돌아왔다고 판단 되면 고정 해제
        if (changePosition.magnitude < returnStartPoint)
        {
            _isMoveFix = false;
            _isUpDown = false;
            _isLeftRight = false;
        }
        // 드래그 종료
        _isDragging = false;
    }

    /// <summary>
    /// 보드 비/활성화
    /// </summary>
    /// <param name="active">true 또는 false를 넣어서 활성화 또는 비활성화</param>
    public void SetBoardActive(bool active)
    {
        _isBoardActive = active;

        if (active)
        {
            Debug.Log("보드 활성화");
        }
        else
        {
            Debug.Log("보드 비활성화");
        }
    }
}